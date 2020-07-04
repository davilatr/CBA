using System;
using CBA.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CBA.Web.Controllers.Cadastro
{
    public class CadastroUnidadeMedidaController : Controller
    {
        private const int _qtdeMaxLinhasPorPagina = 10;

        // GET: Cadastro

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _qtdeMaxLinhasPorPagina, 20, 30 }, _qtdeMaxLinhasPorPagina);
            ViewBag.QtdeMaxLinhasPorPagina = _qtdeMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;
            var lista = UnidadeMedidaModel.RecuperarUnidadeMedida(ViewBag.PaginaAtual, _qtdeMaxLinhasPorPagina);
            var qtdeReg = UnidadeMedidaModel.RecuperarUnidadeMedidaQtde();


            ViewBag.QtdeDePaginas = (qtdeReg / ViewBag.QtdeMaxLinhasPorPagina);
            if (qtdeReg % ViewBag.QtdeMaxLinhasPorPagina > 0)
                ViewBag.QtdeDePaginas++;


            return View(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult PaginacaoUnidadeMedida(int pagina, int tamPag)
        {
            var lista = UnidadeMedidaModel.RecuperarUnidadeMedida(pagina, tamPag);
            return Json(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult ListaUnidadeMedida(int id)
        {
            return Json(UnidadeMedidaModel.RecuperarUnidadeMedida(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirUnidadeMedida(int id)
        {
            return Json(UnidadeMedidaModel.ExcluirUnidadeMedida(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarUnidadeMedida(UnidadeMedidaModel obj)
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
                    var id = obj.SalvarUnidadeMedida();
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