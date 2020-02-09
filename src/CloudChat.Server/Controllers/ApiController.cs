using CloudChat.Core;
using CloudChat.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CloudChat.Server.Controllers
{
    [AuthorizeToken]
    public class ApiController : Controller
    {
        #region Protected Members
        protected IDataRepository mRepository;
        protected ApplicationDbContext mContext;
        protected ChatMessageDbContext mMessageContext;
        protected UserManager<ApplicationUser> mUserManager;
        protected SignInManager<ApplicationUser> mSignInManager;
        #endregion

        #region Constructor
        public ApiController(ApplicationDbContext context,
                             ChatMessageDbContext messageContext,
                             UserManager<ApplicationUser> userManager,
                             SignInManager<ApplicationUser> signInManager)
        {
            mContext = context;
            mMessageContext = messageContext;
            mMessageContext = messageContext;
            mUserManager = userManager;
            mSignInManager = signInManager;
        }
        #endregion

        #region User Profile / Update / Login / Register
        [AllowAnonymous]
        [Route(ApiRoutes.Register)]
        public async Task<ApiResponse<RegisterResultApiModel>> RegisterAsync([FromBody]RegisterCredentialsApiModel registerCredentials)
        {
            var invalidErrorMessage = "Please provide all required details to register  for an account";

            var errorResponse = new ApiResponse<RegisterResultApiModel>
            {
                ErrorMessage = invalidErrorMessage
            };

            if (registerCredentials == null)
                return errorResponse;

            if (string.IsNullOrWhiteSpace(registerCredentials?.Username))
                return errorResponse;

            var user = new ApplicationUser
            {
                UserName = registerCredentials?.Username,
                FirstName = registerCredentials?.FirstName,
                LastName = registerCredentials?.LastName,
                Email = registerCredentials?.Email,
            };

            // Try and create a user
            var result = await mUserManager.CreateAsync(user, registerCredentials.Password);

            //If the registration was succesfull...
            if (result.Succeeded)
            {
                var userEntity = new UsersEntity
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName,
                    CreatedAt = DateTimeOffset.UtcNow,
                    PasswordHash = user.PasswordHash,
                    Email = user.Email,
                    PasswordChangedDate = DateTimeOffset.UtcNow,
                };

                mMessageContext.Users.Add(userEntity);

                var userProfilePictureEntity = new UserProfilePictureEntity
                {
                    User = userEntity,
                };

                mMessageContext.UserProfilePicture.Add(userProfilePictureEntity);
                await mMessageContext.SaveChangesAsync();
                await SendEmailUserVerificationAsync(user);
                var userIdentity = await mUserManager.FindByNameAsync(user.UserName);
                return new ApiResponse<RegisterResultApiModel>
                {
                    Response = new RegisterResultApiModel
                    {
                        FirstName = userIdentity.FirstName,
                        LastName = userIdentity.LastName,
                        Username = userIdentity.UserName,
                        Email = userIdentity.Email,
                        Token = userIdentity.GenerateJwtToken(),
                    }
                };
            }
            // Otherwise, it failed...
            else
                return new ApiResponse<RegisterResultApiModel>
                {
                    ErrorMessage = result.Errors.AgregateErrors()
                };
        }

        [AllowAnonymous]
        [Route(ApiRoutes.Login)]
        public async Task<ApiResponse<UserProfileDetailsApiModel>> LogInAsync([FromBody]LoginCredentialsApiModel loginCredentials)
        {
            var invalidErrorMessage = "Invalid username or password";

            var errorResponse = new ApiResponse<UserProfileDetailsApiModel>
            {
                ErrorMessage = invalidErrorMessage,
            };

            if (loginCredentials?.UsernameOrEmail == null || 
                string.IsNullOrWhiteSpace(loginCredentials.UsernameOrEmail))
                return errorResponse;

            var isEmail = loginCredentials.UsernameOrEmail.Contains("@");

            var user = isEmail ?
                await mUserManager.FindByEmailAsync(loginCredentials.UsernameOrEmail) :
                await mUserManager.FindByNameAsync(loginCredentials.UsernameOrEmail);

            if (user == null)
                return errorResponse;

            var isValidPassword = await mUserManager.CheckPasswordAsync(user, loginCredentials.Password);

            if (!isValidPassword)
                return errorResponse;

            var userEntity = await mMessageContext.GetUserByUsernameAsync(user.UserName);

            // Return token to user
            return new ApiResponse<UserProfileDetailsApiModel>
            {
                Response = new UserProfileDetailsApiModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName,
                    Email = user.Email,
                    Token = user.GenerateJwtToken(),
                    ProfilePictureName = await mMessageContext.GetUserProfilePictureUrlAsync(userEntity),
                    ProfilePictureThumbName = await mMessageContext.GetUserProfilePictureThumbUrlAsync(userEntity),
                }
            };
        }

        [AllowAnonymous]
        [Route(ApiRoutes.VerifyEmail)]
        [HttpGet]
        public async Task<IActionResult> VerifyEmailAsync(string userId, string emailToken)
        {
            var user = await mUserManager.FindByIdAsync(userId);
            if (user == null)
                //TODO: Set UI
                return Content("User not found");

            var result = await mUserManager.ConfirmEmailAsync(user, emailToken);

            if (result.Succeeded)
                //TODO: Set UI
                return Content("Email verified :)");

            //TODO: Set UI
            return Content("Invalid email verification token :(");
        }

        [AllowAnonymous]
        [Route(ApiRoutes.GetUserProfile)]
        public async Task<ApiResponse<UserProfileDetailsApiModel>> GetUserProfileAsync()
        {
            //var errors = new List<string>();
            var user = await mUserManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return new ApiResponse<UserProfileDetailsApiModel>
                {
                    ErrorMessage = "User not found!"
                };
            }
            var userEntity = await mMessageContext.GetUserByUsernameAsync(user.UserName);

            return new ApiResponse<UserProfileDetailsApiModel>
            {
                Response = new UserProfileDetailsApiModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName,
                    Email = user.Email,
                    ProfilePictureName = await mMessageContext.GetUserProfilePictureUrlAsync(userEntity),
                    ProfilePictureThumbName = await mMessageContext.GetUserProfilePictureThumbUrlAsync(userEntity),
                }
            };
        }

        [Route(ApiRoutes.UpdateUserProfile)]
        public async Task<ApiResponse> UpdateUserProfileAsync([FromBody]UpdateUserProfileApiModel model)
        {
            //var errors = new List<string>();
            var emailChanged = false;
            var user = await mUserManager.GetUserAsync(HttpContext.User);

            var userEntity = await mMessageContext.GetUserByUsernameAsync(user.UserName);
            var conversationEntities = mMessageContext.GetConversations(userEntity);

            if (user == null)
            {
                return new ApiResponse<UserProfileDetailsApiModel>
                {
                    ErrorMessage = "User not found!"
                };
            }

            #region Update User Details

            if (model.FirstName != null)
            {
                user.FirstName = model.FirstName;
                userEntity.FirstName = model.FirstName;
            }

            if (model.LastName != null)
            {
                user.LastName = model.LastName;
                userEntity.LastName = model.LastName;
            }

            if (model.Email != null && !string.Equals(model.Email.Replace(" ", ""), user.NormalizedEmail))
            {
                var contacts = mMessageContext.GetContacts(user.UserName);
                mMessageContext.ChangeContactsEmailAsync(contacts,model.Email);

                user.Email = model.Email;
                userEntity.Email = model.Email;
                user.EmailConfirmed = false;
                emailChanged = true;
            }

            if (model.Username != null)
            {
                user.UserName = model.Username;
                userEntity.Username = model.Username;
                var contacts = mMessageContext.GetContacts(user.UserName);
                mMessageContext.ChangeContactsUsernameAsync(contacts, model.Username);
            }

            if (model.ProfilePictureName != null)
            {
                var userProfilePictureEntity = await mMessageContext.UserProfilePicture.Where(p => p.User.Id == userEntity.Id)
                                                                                       .FirstOrDefaultAsync();
                var contactsEntities = mMessageContext.Contacts.Where(c => c.User.Id == userEntity.Id);

                if (userProfilePictureEntity != null)
                {
                    userProfilePictureEntity.PictureUrl = model.ProfilePictureName;
                    userProfilePictureEntity.PictureThumbUrl = await ThumbCreator.CreateThumbnailAndUpload(model.ProfilePictureName, FileType.ProfilePicture);
                    mMessageContext.Entry(userProfilePictureEntity).State = EntityState.Modified;
                }
                else
                {
                    userProfilePictureEntity = new UserProfilePictureEntity
                    {
                        User = userEntity,
                        PictureUrl = model.ProfilePictureName,
                        PictureThumbUrl = await ThumbCreator.CreateThumbnailAndUpload(model.ProfilePictureName, FileType.ProfilePicture)
                    };
                    await mMessageContext.UserProfilePicture.AddAsync(userProfilePictureEntity);
                }
            } 

            #endregion

            var result = await mUserManager.UpdateAsync(user);

            // If successfull, send out email verification
            if (result.Succeeded && emailChanged)
                await SendEmailUserVerificationAsync(user);

            if (result.Succeeded)
            {
                mMessageContext.Entry(userEntity).State = EntityState.Modified;
                await mMessageContext.SaveChangesAsync();
                return new ApiResponse();
            }
            else
                return new ApiResponse
                {
                    ErrorMessage = result.Errors.AgregateErrors()
                };
        }

        [Route(ApiRoutes.UpdateUserPassword)]
        public async Task<ApiResponse> UpdateUserPasswordAsync([FromBody]UpdateUserPasswordApiModel model)
        {
            //  var errors = new List<string>();

            #region Get User

            // Get current user
            var user = await mUserManager.GetUserAsync(HttpContext.User);
            var userEntity = await mMessageContext.Users.Where(u => u.Username.ToString() == user.UserName.ToString())
                                                                              .FirstOrDefaultAsync();
            // IF we have no user ...
            if (user == null)
            {
                return new ApiResponse
                {
                    ErrorMessage = "User not found!"
                };
            }
            #endregion

            #region Update Password

            // Attempt to commit changes to datastore
            var result = await mUserManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            #endregion

            if (result.Succeeded)
            {
                userEntity.PasswordHash = user.PasswordHash;
                userEntity.PasswordChangedDate = DateTimeOffset.UtcNow;
                mMessageContext.Entry(userEntity).State = EntityState.Modified;
                await mMessageContext.SaveChangesAsync();

                return new ApiResponse();
            }
            else
            {
                return new ApiResponse
                {
                    ErrorMessage = result.Errors.AgregateErrors()
                };
            }
        }
        #endregion

        #region Private Helpers

        private async Task SendEmailUserVerificationAsync(ApplicationUser user)
        {
            var userIdentity = await mUserManager.FindByNameAsync(user.UserName);

            var emailVerificationCode = await mUserManager.GenerateEmailConfirmationTokenAsync(user);

            // TODO: change http to https if ssl sertifiate is needed
            var confirmationUrl = $"http://{Request.Host.Value}/api/verify/?userId={userIdentity.Id}&emailToke={emailVerificationCode}";

            await KommunikativEmailSender.SendUserEmailVerificationAsync(userIdentity.FirstName ?? userIdentity.UserName, userIdentity.Email, confirmationUrl);
        }
        #endregion

    }
}
