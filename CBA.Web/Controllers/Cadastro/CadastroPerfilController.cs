using CBA.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CBA.Web.Controllers.Cadastro
{
    [Authorize(Roles = "Administrador")]
    public class CadastroPerfilController : Controller
    {

        private const int _qtdeMaxLinhasPorPagina = 10;

        public ActionResult Index()
        {
            ViewBag.ListaUsuario = UsuarioModel.RecuperarUsuario();
            ViewBag.ListaTamPag = new SelectList(new int[] { _qtdeMaxLinhasPorPagina, 20, 30 }, _qtdeMaxLinhasPorPagina);
            ViewBag.QtdeMaxLinhasPorPagina = _qtdeMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;
            var lista = PerfilModel.RecuperarPerfil(ViewBag.PaginaAtual, _qtdeMaxLinhasPorPagina);
            var qtdeReg = PerfilModel.RecuperarPerfilQtde();


            ViewBag.QtdeDePaginas = (qtdeReg / ViewBag.QtdeMaxLinhasPorPagina);
            if (qtdeReg % ViewBag.QtdeMaxLinhasPorPagina > 0)
                ViewBag.QtdeDePaginas++;


            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PaginacaoPerfil(int pagina, int tamPag)
        {
            var lista = PerfilModel.RecuperarPerfil(pagina, tamPag);
            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ListaPerfil(int id)
        {
            var retorno = PerfilModel.RecuperarPerfil(id);
            retorno.SelecionarUsuario();


            return Json(retorno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirPerfil(int id)
        {
            return Json(PerfilModel.ExcluirPerfil(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarPerfil(PerfilModel obj)
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
                    var id = obj.SalvarPerfil();
                    if (id > 0)
                        idSalvo = id.ToString();

                    else
                        resultado = "erro";

                }
                catch (Exception ex)
                {
                    resultado = "erro";
                    Console.WriteLine(ex);
                    throw;
                }
            }
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
    }
}