using System;
using CBA.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CBA.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private const int _qtdeMaxLinhasPorPagina = 10;

        // GET: Cadastro

        #region Usuarios do Sistema

        private const string _senhaPadrao = "{EsTeLaR}";

        [Authorize]
        public ActionResult Usuario()
        {
            ViewBag.SenhaPadrao = _senhaPadrao;
            return View(UsuarioModel.RecuperarUsuario());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ListaUsuario(int id)
        {
            return Json(UsuarioModel.RecuperarUsuario(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirUsuario(int id)
        {
            return Json(UsuarioModel.ExcluirUsuario(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarUsuario(UsuarioModel obj)
        {
            var resultado = "ok";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "aviso";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    if (obj.Senha == _senhaPadrao)
                        obj.Senha = "";

                    var id = obj.SalvarUsuario();
                    if (id > 0)
                        idSalvo = id.ToString();

                    else
                        resultado = "erro";

                }
                catch (Exception ex)
                {
                    resultado = "erro";
                }
            }
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }

        #endregion

        
    }
}