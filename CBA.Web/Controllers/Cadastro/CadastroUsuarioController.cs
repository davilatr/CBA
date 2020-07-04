using System;
using CBA.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CBA.Web.Controllers
{
    public class CadastroUsuarioController : Controller
    {
        private const int _qtdeMaxLinhasPorPagina = 10;
        private const string _senhaPadrao = "{EsTeLaR}";

        // GET: Cadastro

        [Authorize]
        public ActionResult Index()
        {

            ViewBag.ListaTamPag = new SelectList(new int[] { _qtdeMaxLinhasPorPagina, 20, 30 }, _qtdeMaxLinhasPorPagina);
            ViewBag.QtdeMaxLinhasPorPagina = _qtdeMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;
            ViewBag.SenhaPadrao = _senhaPadrao;

            var lista = UsuarioModel.RecuperarUsuario(ViewBag.PaginaAtual, _qtdeMaxLinhasPorPagina);
            var qtdeReg = UsuarioModel.RecuperarUsuarioQtde();

            ViewBag.QtdeDePaginas = (qtdeReg / ViewBag.QtdeMaxLinhasPorPagina);
            if (qtdeReg % ViewBag.QtdeMaxLinhasPorPagina > 0)
                ViewBag.QtdeDePaginas++;


            return View(lista);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult PaginacaoUsuario(int pagina, int tamPag, string filtro)
        {
            var lista = UsuarioModel.RecuperarUsuario(pagina, tamPag, filtro);
            return Json(lista);
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
                catch (Exception)
                {
                    resultado = "erro";
                }
            }
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
    }
}