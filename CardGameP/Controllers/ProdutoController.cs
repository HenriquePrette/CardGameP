using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGameP.Models;

namespace CardGameP.Controllers
{
    public class ProdutoController : Controller
    {
        private LojaContext db = new LojaContext();

        public ActionResult Index(string busca)
        {
            var produtos = db.Produtos.AsQueryable();

            if (!string.IsNullOrEmpty(busca))
            {
                produtos = produtos.Where(p => p.Nome.Contains(busca) || p.Jogo.Contains(busca));

                return View(produtos.ToList());
            }

            var produtosAleatorios = produtos
                                       .OrderBy(p => Guid.NewGuid())
                                       .Take(8)
                                       .ToList();

            return View(produtosAleatorios);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Produto produto = db.Produtos.Find(id);

            if (produto == null)
            {
                return HttpNotFound();
            }

            return View(produto);
        }

        public ActionResult AdminIndex(string busca)
        {
            if (Session["FuncionarioLogado"] == null)
            {
                return RedirectToAction("Login", "Funcionario");
            }

            var produtos = db.Produtos.AsQueryable();

            if (!string.IsNullOrEmpty(busca))
            {
                produtos = produtos.Where(p => p.Nome.Contains(busca) || p.Jogo.Contains(busca));
            }

            return View(produtos.ToList());
        }

        public ActionResult Create()
        {
            if (Session["FuncionarioLogado"] == null)
            {
                return RedirectToAction("Login", "Funcionario");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Produto produto, HttpPostedFileBase fotoUpload)
        {
            if (Session["FuncionarioLogado"] == null)
            {
                return RedirectToAction("Login", "Funcionario");
            }

            if (ModelState.IsValid)
            {
                if (fotoUpload != null && fotoUpload.ContentLength > 0)
                {
                    string nomeArquivo = Path.GetFileName(fotoUpload.FileName);
                    string nomeUnico = Guid.NewGuid().ToString() + "_" + nomeArquivo;

                    string caminhoPasta = Server.MapPath("~/Content/Image/");

                    if (!Directory.Exists(caminhoPasta))
                    {
                        Directory.CreateDirectory(caminhoPasta);
                    }

                    string caminhoCompleto = Path.Combine(caminhoPasta, nomeUnico);

                    fotoUpload.SaveAs(caminhoCompleto);

                    produto.imagem = "/Content/Image/" + nomeUnico;
                }

                try
                {
                    db.Produtos.Add(produto);
                    db.SaveChanges();
                    return RedirectToAction("AdminIndex");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao salvar o produto no banco: " + ex.Message);
                }
            }

            return View(produto);
        }

        public ActionResult Delete(int? id)
        {
            if (Session["FuncionarioLogado"] == null)
            {
                return RedirectToAction("Login", "Funcionario");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Produto produto = db.Produtos.Find(id);

            if (produto == null)
            {
                return HttpNotFound();
            }

            return View(produto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["FuncionarioLogado"] == null)
            {
                return RedirectToAction("Login", "Funcionario");
            }

            try
            {
                Produto produto = db.Produtos.Find(id);

                if (produto != null)
                {
                    if (!string.IsNullOrEmpty(produto.imagem))
                    {
                        string caminhoFisico = Server.MapPath("~" + produto.imagem);

                        if (System.IO.File.Exists(caminhoFisico))
                        {
                            System.IO.File.Delete(caminhoFisico);
                        }
                    }

                    db.Produtos.Remove(produto);
                    db.SaveChanges();
                }

                return RedirectToAction("AdminIndex");
            }
            catch (Exception ex)
            {
                TempData["ErroDelete"] = "Não foi possível excluir o produto. Erro: " + ex.Message;
                return RedirectToAction("AdminIndex");
            }
        }

        public ActionResult Edit(int? id)
        {
            if (Session["FuncionarioLogado"] == null)
            {
                return RedirectToAction("Login", "Funcionario");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Produto produto = db.Produtos.Find(id);

            if (produto == null)
            {
                return HttpNotFound();
            }

            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Produto produto, HttpPostedFileBase fotoUpload)
        {
            if (Session["FuncionarioLogado"] == null)
            {
                return RedirectToAction("Login", "Funcionario");
            }

            if (ModelState.IsValid)
            {
                var produtoNoBanco = db.Produtos.AsNoTracking().FirstOrDefault(p => p.IdProduto == produto.IdProduto);

                if (fotoUpload != null && fotoUpload.ContentLength > 0)
                {
                    if (produtoNoBanco != null && !string.IsNullOrEmpty(produtoNoBanco.imagem))
                    {
                        string caminhoFotoAntiga = Server.MapPath("~" + produtoNoBanco.imagem);
                        if (System.IO.File.Exists(caminhoFotoAntiga))
                        {
                            System.IO.File.Delete(caminhoFotoAntiga);
                        }
                    }

                    string nomeArquivo = Path.GetFileName(fotoUpload.FileName);
                    string nomeUnico = Guid.NewGuid().ToString() + "_" + nomeArquivo;
                    string caminhoPasta = Server.MapPath("~/Content/Image/");

                    if (!Directory.Exists(caminhoPasta))
                    {
                        Directory.CreateDirectory(caminhoPasta);
                    }

                    string caminhoCompleto = Path.Combine(caminhoPasta, nomeUnico);
                    fotoUpload.SaveAs(caminhoCompleto);

                    produto.imagem = "/Content/Image/" + nomeUnico;
                }
                else
                {
                    if (produtoNoBanco != null)
                    {
                        produto.imagem = produtoNoBanco.imagem;
                    }
                }

                try
                {
                    db.Entry(produto).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("AdminIndex");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao atualizar o produto no banco: " + ex.Message);
                }
            }
            return View(produto);
        }
    }
}