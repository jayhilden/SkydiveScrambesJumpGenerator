using System.Web.Mvc;
using System.Web.Security;
using Data.Sql.Services;
using Scrambles.Models;
using Scrambles.Services;

namespace Scrambles.Controllers
{
    public class AdminController : Controller
    {
        private readonly PasswordService _passwordService;

        public AdminController(PasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        // GET: Admin
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var success = _passwordService.VerifyPassword(model.Password);
            if (!success)
            {
                ModelState.AddModelError(nameof(model.Password), "Password is invalid." );
                return View(new LoginModel());
            }

            FormsAuthentication.SetAuthCookie("username", true);
            Session.Add(UserService.SessionKeyName, true);
            return Redirect("~/");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return Redirect("~/");

        }
    }
}