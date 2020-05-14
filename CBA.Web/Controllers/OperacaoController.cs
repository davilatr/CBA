using System.Web.Mvc;

namespace CBA.Web.Controllers
{
    public class OperacaoController : Controller
    {
        // GET: Operacao

        [Authorize]
        public ActionResult RealizarDestinacaoBem()
        {
            return View();
        }

        [Authorize]
        public ActionResult RealizarVistoriaBem()
        {
            return View();
        }
    }
}