using System.Web.Mvc;
using Data.Sql.Services;
using Scrambles.Models;

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

            Session.Add(PasswordService.SessionKeyName, true);
            return Redirect("~/");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("~/");

        }
    }
}