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
        public ActionResult PerfilUsuario()
        {
            return View();
        }
    }
}