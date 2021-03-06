﻿using System;
using CBA.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CBA.Web.Controllers
{
    [Authorize(Roles ="Gerente,Operador,Administrador")]
    public class CadastroDestinacaoBemController : Controller
    {
        private const int _qtdeMaxLinhasPorPagina = 10;

        // GET: Cadastro

        
        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _qtdeMaxLinhasPorPagina, 20, 30 }, _qtdeMaxLinhasPorPagina);
            ViewBag.QtdeMaxLinhasPorPagina = _qtdeMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;
            var lista = DestinacaoBemModel.RecuperarDestinacaoBem(ViewBag.PaginaAtual, _qtdeMaxLinhasPorPagina);
            var qtdeReg = DestinacaoBemModel.RecuperarDestinacaoBemQtde();


            ViewBag.QtdeDePaginas = (qtdeReg / ViewBag.QtdeMaxLinhasPorPagina);
            if (qtdeReg % ViewBag.QtdeMaxLinhasPorPagina > 0)
                ViewBag.QtdeDePaginas++;


            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PaginacaoDestinacaoBem(int pagina, int tamPag, string filtro)
        {
            var lista = DestinacaoBemModel.RecuperarDestinacaoBem(pagina, tamPag, filtro);
            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ListaDestinacaoBem(int id)
        {
            return Json(DestinacaoBemModel.RecuperarDestinacaoBem(id));
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirDestinacaoBem(int id)
        {
            return Json(DestinacaoBemModel.ExcluirDestinacaoBem(id));
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarDestinacaoBem(DestinacaoBemModel obj)
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
                    var id = obj.SalvarDestinacaoBem();
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