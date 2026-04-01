using System;
using System.Linq;
using System.Web.Mvc;
using CardGameP.Models;

namespace CardGameP.Controllers
{
    public class ClienteController : Controller
    {
        private LojaContext db = new LojaContext();

        public ActionResult AdminIndex()
        {
            if (Session["FuncionarioLogado"] == null)
            {
                return RedirectToAction("Login", "Funcionario");
            }

            try
            {
                var listaDeClientes = db.Clientes.ToList();
                return View(listaDeClientes);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao carregar a lista de clientes: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Cadastrar()
        {
            if (Session["ClienteLogado"] != null)
            {
                return RedirectToAction("Index", "Produto");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var emailExistente = db.Clientes.FirstOrDefault(c => c.Email == cliente.Email);
                    if (emailExistente != null)
                    {
                        ModelState.AddModelError("Email", "Este e-mail já está a ser utilizado por outra conta.");
                        return View(cliente);
                    }

                    cliente.DataCadastro = DateTime.Now;

                    db.Clientes.Add(cliente);
                    db.SaveChanges();

                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    string mensagemErro = ex.Message;
                    if (ex.InnerException != null)
                    {
                        mensagemErro = ex.InnerException.Message;
                        if (ex.InnerException.InnerException != null)
                        {
                            mensagemErro = ex.InnerException.InnerException.Message;
                        }
                    }

                    ModelState.AddModelError("", "Erro no banco: " + mensagemErro);
                }
            }

            return View(cliente);
        }

        public ActionResult Login()
        {
            if (Session["ClienteLogado"] != null)
            {
                return RedirectToAction("Index", "Produto");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Cliente clienteFormulario)
        {
            try
            {
                var clienteNoBanco = db.Clientes.FirstOrDefault(c => c.Email == clienteFormulario.Email && c.Senha == clienteFormulario.Senha);

                if (clienteNoBanco != null)
                {
                    Session["ClienteLogado"] = clienteNoBanco;
                    Session["ClienteNome"] = clienteNoBanco.Nome;

                    return RedirectToAction("Index", "Produto");
                }

                ModelState.AddModelError("", "E-mail ou senha incorretos.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao tentar conectar com o banco de dados: " + ex.Message);
            }

            return View(clienteFormulario);
        }

        public ActionResult Logout()
        {
            Session["ClienteLogado"] = null;
            Session["ClienteNome"] = null;

            return RedirectToAction("Index", "Home");
        }
    }
}