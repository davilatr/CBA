using CBA.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CBA.Web.Controllers
{
    public class CadastroController : Controller
    {
        private static List<DestinacaoBemModel> _ListaDestinacaoBem = new List<DestinacaoBemModel>()
        {
            new DestinacaoBemModel() {Id=1, Nome="Doação", Ativo=true},
            new DestinacaoBemModel() {Id=2, Nome="Leilão", Ativo=true},
            new DestinacaoBemModel() {Id=3, Nome="Destruição", Ativo=true},
            new DestinacaoBemModel() {Id=4, Nome="Abandono", Ativo=true},
            new DestinacaoBemModel() {Id=5, Nome="Deterioração", Ativo=true},
            new DestinacaoBemModel() {Id=6, Nome="Pendente", Ativo=true},
            new DestinacaoBemModel() {Id=7, Nome="Liberado", Ativo=true}
        };


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

        [Authorize]
        public ActionResult DestinacaoBem()
        {
            return View(_ListaDestinacaoBem);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SalvarDestinacaoBem(DestinacaoBemModel obj)
        {
            var registroBD = _ListaDestinacaoBem.Find(x => x.Id == obj.Id);
            if (registroBD == null)
            {
                //incluir
                registroBD = obj;
                registroBD.Id = _ListaDestinacaoBem.Max(x => x.Id) + 1;
                _ListaDestinacaoBem.Add(registroBD);
            }
            else
            {
                //alterar
                registroBD.Nome = obj.Nome;
                registroBD.Ativo = obj.Ativo;
            }
            return Json(registroBD);
        }

        [HttpPost]
        [Authorize]
        public ActionResult RecuperarDestinacaoBem(int id)
        {
            return Json(_ListaDestinacaoBem.Find(x => x.Id == id));
        }

        [HttpPost]
        [Authorize]
        public ActionResult ExcluirDestinacaoBem(int id)
        {
            var ret = false;
            var registroBD = _ListaDestinacaoBem.Find(x => x.Id == id);

            if (registroBD != null)
            {
                _ListaDestinacaoBem.Remove(registroBD);
                ret = true;
            }
            return Json(ret);
        }

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