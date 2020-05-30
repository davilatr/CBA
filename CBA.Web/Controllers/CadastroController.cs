using System;
using CBA.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CBA.Web.Controllers
{
    public class CadastroController : Controller
    {
       // GET: Cadastro

        //-------------------------------------------------------------------
        //Destinação de Bens

        [Authorize]
        public ActionResult DestinacaoBem()
        {
            return View(DestinacaoBemModel.RecuperarDestinacaoBem());
        }

        [HttpPost]
        [Authorize]
        public ActionResult ListaDestinacaoBem(int id)
        {
            return Json(DestinacaoBemModel.RecuperarDestinacaoBem(id));
        }

        [HttpPost]
        [Authorize]
        public ActionResult ExcluirDestinacaoBem(int id)
        {
            return Json(DestinacaoBemModel.ExcluirDestinacaoBem(id));
        }

        [HttpPost]
        [Authorize]
        public ActionResult SalvarDestinacaoBem(DestinacaoBemModel obj)
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

        //-------------------------------------------------------------------

        [Authorize]
        public ActionResult UnidadeOrganizacional()
        {
            return View();
        }

        [Authorize]
        public ActionResult BemApreendido()
        {
            return View();
        }

        [Authorize]
        public ActionResult VistoriaBem()
        {
            return View();
        }

        [Authorize]
        public ActionResult Pessoa()
        {
            return View();
        }

        [Authorize]
        public ActionResult AutoInfracao()
        {
            return View();
        }

        [Authorize]
        public ActionResult TermoApreensao()
        {
            return View();
        }

        [Authorize]
        public ActionResult TermoDeposito()
        {
            return View();
        }

        [Authorize]
        public ActionResult ProcessoAdministrativo()
        {
            return View();
        }

        [Authorize]
        public ActionResult TipoBem()
        {
            return View();
        }

        [Authorize]
        public ActionResult UnidadeMedida()
        {
            return View();
        }

        //--------------------------------------------------------------------

        [Authorize]
        public ActionResult UnidadeFederativa()
        {
            return View();
        }

        [Authorize]
        public ActionResult Cidade()
        {
            return View();
        }

        [Authorize]
        public ActionResult EstadoConservacao()
        {
            return View();
        }

        [Authorize]
        public ActionResult RotaVistoria()
        {
            return View();
        }

        [Authorize]
        public ActionResult Usuario()
        {
            return View();
        }

        [Authorize]
        public ActionResult PerfilUsuario()
        {
            return View();
        }
    }
}