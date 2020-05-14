using CBA.Web.Models;
using System.Web.Mvc;

using System.Web.Security;

namespace CBA.Web.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel login, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(login);

            var achou = UsuarioModel.ValidarUsuario(login.Usuario, login.Senha);

            if (achou)
            {
                FormsAuthentication.SetAuthCookie(login.Usuario, login.LembrarMe);
                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Login Inválido");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}