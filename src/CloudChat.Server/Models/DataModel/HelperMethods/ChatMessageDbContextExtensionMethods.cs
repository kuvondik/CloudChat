using CloudChat.Core;
using CloudChat.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudChat.Server
{
    public static class ChatMessageDbContextExtensionMethods
    {
        // Get User Entity By Username
        public static async Task<UsersEntity> GetUserByUsernameAsync(this ChatMessageDbContext context, string username)
        {
            return await context.Users.Where(u => u.Username == username)
                                      .FirstOrDefaultAsync();
        }

        // Get User Entity By Email
        public static async Task<UsersEntity> GetUserByEmailAsync(this ChatMessageDbContext context, string email)
        {
            return await context.Users.Where(u => u.Email == email)
                                      .FirstOrDefaultAsync();
        }


        // Get User Profile Picture
        public static async Task<string> GetUserProfilePictureUrlAsync(this ChatMessageDbContext context, UsersEntity userEntity)
        {
            return await context.UserProfilePicture.Where(p => p.User == userEntity)
                                                   .Select(p => p.PictureUrl)
                                                   .FirstOrDefaultAsync();
        }

        // Get User Profile Picture Thumb
        public static async Task<string> GetUserProfilePictureThumbUrlAsync(this ChatMessageDbContext context, UsersEntity userEntity)
        {
            return await context.UserProfilePicture.Where(p => p.User == userEntity)
                                                   .Select(p => p.PictureThumbUrl)
                                                   .FirstOrDefaultAsync();
        }

        // Get User Profile Picture By Username
        public static string GetUserProfilePictureUrl(this ChatMessageDbContext context, string username)
        {
            return context.UserProfilePicture.Where(p => p.User.Username == username)
                                             .Select(p => p.PictureUrl)
                                             .FirstOrDefault();
        }

        // Get User Profile Picture Thumb By Username
        public static string GetUserProfilePictureThumbUrl(this ChatMessageDbContext context, string username)
        {
            return context.UserProfilePicture.Where(p => p.User.Username == username)
                                             .Select(p => p.PictureThumbUrl)
                                             .FirstOrDefault();
        }

        // Assign pictures to Contacts List
        public static async Task AssignContactPicturesAsync(this ChatMessageDbContext context, IEnumerable<ContactApiModel> contactApiModels)
        {
            var contactPictures = new List<ContactPictures>();

            foreach (var contactApiModel in contactApiModels)
            {
                 await context.UserProfilePicture.Where(p => p.User.Username == contactApiModel.Username)
                                            .ForEachAsync(p =>
                                            {
                                                contactApiModel.PictureThumbName = p.PictureThumbUrl;
                                                contactApiModel.PictureName = p.PictureUrl;
                                            });
            }
        }
        
        /// <summary>
        /// Gets all conversations that the user participates in
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userEntity">user Enity</param>
        /// <returns></returns>
        public static IEnumerable<ConversationsEntity> GetConversations(this ChatMessageDbContext context, UsersEntity userEntity)
        {
            return context.Participants.Where(p => p.User.Id == userEntity.Id)
                                       .Select(p => p.Conversation);
        }

        public static async Task<ConversationsEntity> GetConversationByChannelId(this ChatMessageDbContext context, string channelId)
        {
            return await context.Conversations.Where(conv => conv.ChannelId == channelId)
                                              .FirstOrDefaultAsync();
        }

        public static async Task<ParticipantsEntity> GetParticipantByUsername(this ChatMessageDbContext context, string username)
        {
            return await context.Participants.Where(p => p.User.Username == username)
                                             .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets contacts
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static IEnumerable<ContactsEntity> GetContacts(this ChatMessageDbContext context, string username)
        {
            return context.Contacts.Where(p => p.User.Username == username);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contactsEntities"></param>
        /// <param name="newEmail"></param>
        /// <returns></returns>
        /// <remarks>SaveChanges Required!</remarks>
        public static void ChangeContactsEmailAsync(this ChatMessageDbContext context, IEnumerable<ContactsEntity> contactsEntities, string newEmail)
        {
            foreach (var o in contactsEntities)
            {
                o.User.Email = newEmail;
                context.Entry(o).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contactsEntities"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <remarks>SaveChanges Required!</remarks>
        public static void ChangeContactsUsernameAsync(this ChatMessageDbContext context, IEnumerable<ContactsEntity> contactsEntities, string username)
        {
            foreach (var o in contactsEntities)
            {
                o.User.Username = username;
                context.Entry(o).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="participantApiModels"></param>
        /// <param name="conversation"></param>
        /// <returns></returns>
        public static async Task AddParticipantsToConversationAsync(this ChatMessageDbContext context, IEnumerable<ParticipantApiModel> participantApiModels, ConversationsEntity conversation)
        {
            var participantsEntities = new List<ParticipantsEntity>();

            foreach (var participant in participantApiModels)
            {
                participantsEntities.Add(new ParticipantsEntity
                {
                    Conversation = conversation,
                    User = await context.GetUserByUsernameAsync(participant.UserUsername),
                });
            }

            await context.Participants.AddRangeAsync(participantsEntities);
        }

        public static async Task<ConversationApiModel> GetConversationWithTitleAsync(this ChatMessageDbContext context,
                                                                                          ParticipantsEntity participant,
                                                                                          ConversationsEntity conversation,
                                                                                          UsersEntity creator)
        {
                var conversationApiModel = new ConversationApiModel
                {
                    ChannelId = conversation.ChannelId,
                    CreatedAt = conversation.CreatedAt,
                    UpdatedAt = conversation.UpdatedAt,
                    Type = conversation.Type,
                    CreatorUsername = creator.Username,
                    UsernameList = await context.Participants.Where(partic => partic.Conversation.Id == conversation.Id)
                                                        .Select(partic => partic.User.Username)
                                                        .ToListAsync(),
                    PreMessage = await context.Messages.Where(message => message.Conversation.ChannelId == conversation.ChannelId)
                                                   .OrderBy(message => message.CreatedAt)
                                                   .Select(message => message.Message)
                                                   .LastOrDefaultAsync(),
                };

                if (conversationApiModel.Type == ConversationType.Group)
                    conversationApiModel.Title = participant.Conversation.Title;
                else
                {
                    conversationApiModel.Title = StringHelpers.GetShortName(firstName: participant.User?.FirstName,
                                                                            lastName: participant.User?.LastName,
                                                                            userName: participant.User.Username);
                }

            return conversationApiModel;
        }

        
    }
}
