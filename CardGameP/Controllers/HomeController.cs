using System.Linq;
using System.Web.Mvc;
using CardGameP.Models;

namespace CardGameP.Controllers
{
    public class HomeController : Controller
    {
        private LojaContext db = new LojaContext();

        public ActionResult Index()
        {
            var produtosDestaque = db.Produtos
                                     .OrderByDescending(p => p.IdProduto)
                                     .Take(4)
                                     .ToList();

            return View(produtosDestaque);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}