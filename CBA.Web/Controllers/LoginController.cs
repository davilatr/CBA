using CBA.Web.Models;
using System;
using System.Web;
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

            var usuario = UsuarioModel.ValidarUsuario(login.Usuario, login.Senha);

            if (usuario != null)
            {
                //FormsAuthentication.SetAuthCookie(usuario.Nome, login.LembrarMe);
                var ticket =FormsAuthentication.Encrypt
                    (new FormsAuthenticationTicket(1, usuario.Nome, DateTime.Now, DateTime.Now.AddMinutes(30), login.LembrarMe, PerfilModel.RecuperarPerfil(usuario.IdPerfil).Nome));
                
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticket);

                Response.Cookies.Add(cookie);

                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Login ou senha incorreto!");
            }
            return View(login);
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