using System.Web.Mvc;

namespace CBA.Web.Controllers
{
    public class RelatorioController : Controller
    {
        // GET: Relatorio

        [Authorize]
        public ActionResult QtdeBem()
        {
            return View();
        }

        [Authorize]
        public ActionResult QtdeBemDestinado()
        {
            return View();
        }

        [Authorize]
        public ActionResult BemPorTipo()
        {
            return View();
        }

        [Authorize]
        public ActionResult BemPorRegiao()
        {
            return View();
        }

        [Authorize]
        public ActionResult BemPorDestinatario()
        {
            return View();
        }
    }
}