using System.Linq;
using System.Web.Mvc;
using CardGameP.Models;

namespace CardGameP.Controllers
{
    public class FuncionarioController : Controller
    {
        private LojaContext db = new LojaContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string senha)
        {
            var funcionario = db.Funcionarios
                                .FirstOrDefault(f => f.email == email && f.senha == senha);

            if (funcionario != null)
            {
                Session["FuncionarioLogado"] = funcionario.id_funcionario;
                Session["NomeFuncionario"] = funcionario.nome;

                return RedirectToAction("Painel");
            }

            ViewBag.Erro = "Email ou senha inválidos.";
            return View();
        }

        public ActionResult Painel()
        {
            if (Session["FuncionarioLogado"] == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}