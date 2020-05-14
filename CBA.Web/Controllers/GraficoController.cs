using System.Web.Mvc;

namespace CBA.Web.Controllers
{
    public class GraficoController : Controller
    {
        // GET: Grafico

        [Authorize]
        public ActionResult EntradaVsDestinacao()
        {
            return View();
        }
    }
}