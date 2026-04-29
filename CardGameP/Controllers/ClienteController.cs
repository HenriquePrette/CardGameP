using System;
using System.Linq;
using System.Web.Mvc;
using CardGameP.Models;
using BCrypt.Net; 

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
                        ModelState.AddModelError("Email", "Este e-mail já está sendo utilizado.");
                        return View(cliente);
                    }

                    cliente.Senha = BCrypt.Net.BCrypt.HashPassword(cliente.Senha);
                    cliente.DataCadastro = DateTime.Now;

                    db.Clientes.Add(cliente);
                    db.SaveChanges();

                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao salvar no banco: " + ex.Message);
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
                var clienteNoBanco = db.Clientes.FirstOrDefault(c => c.Email == clienteFormulario.Email);

                if (clienteNoBanco != null)
                {
                    bool senhaValida = false;
                    bool precisaMigrarParaHash = false;

                    if (clienteNoBanco.Senha.StartsWith("$2"))
                    {
                        senhaValida = BCrypt.Net.BCrypt.Verify(clienteFormulario.Senha, clienteNoBanco.Senha);
                    }
                    else
                    {
                        if (clienteNoBanco.Senha == clienteFormulario.Senha)
                        {
                            senhaValida = true;
                            precisaMigrarParaHash = true; 
                        }
                    }

                    if (senhaValida)
                    {
                        if (precisaMigrarParaHash)
                        {
                            clienteNoBanco.Senha = BCrypt.Net.BCrypt.HashPassword(clienteFormulario.Senha);
                            db.Entry(clienteNoBanco).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }

                        Session["ClienteLogado"] = clienteNoBanco;
                        Session["ClienteNome"] = clienteNoBanco.Nome;

                        return RedirectToAction("Index", "Produto");
                    }
                }

                ModelState.AddModelError("", "E-mail ou senha incorretos.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao conectar com o banco: " + ex.Message);
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