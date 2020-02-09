using CloudChat.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CloudChat.Server.Controllers
{
    /// <summary>
    /// Manages the standart web pages
    /// </summary>

    public class HomeController : Controller
    {
        #region Protected Members

        protected ApplicationDbContext mContext;
        protected UserManager<ApplicationUser> mUserManager;
        protected SignInManager<ApplicationUser> mSignInManager;
        #endregion

        #region Constructor

        public HomeController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            mContext = context;
            mUserManager = userManager;
            mSignInManager = signInManager;
        }
        #endregion

        public IActionResult Index()
        {
            mContext.Database.EnsureCreated();
            return View();
        }

        [Route(WebRoutes.CreateUser)]
        public async Task<IActionResult> UserCreateAsync()
        {
            var result = await mUserManager.CreateAsync(new ApplicationUser
            {
                UserName = "kuvondik",
                Email = "quvondiqbek@outlook.com",
                FirstName = "Kuvondik",
                LastName = "Sayfiddinov",

            }, "password");

            if (result.Succeeded)
                return Content("User was created", "text/html");
            return Content("User creation failed", "text/html");
        }

        [Route(WebRoutes.Logout)]
        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Content("done", "text/html");
        }

        [Route(WebRoutes.Login)]
        public async Task<IActionResult> LoginAync(string returnUri)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            var result = await mSignInManager.PasswordSignInAsync("kuvondik", "password", true, false);

            if (result.Succeeded)
            {
                if (string.IsNullOrEmpty(returnUri))
                    return RedirectToAction(nameof(Index));
                else
                    return Redirect(returnUri);
            }
            return Content("User creation failed", "text/html");
        }
    }
}
