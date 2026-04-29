using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGameP.Models;
using System.Collections.Generic; 

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
                produtos = produtos.Where(p => p.Nome.Contains(busca) || (p.Jogo != null && p.Jogo.Nome.Contains(busca)));
            }
            else
            {
                produtos = produtos.OrderBy(p => Guid.NewGuid()).Take(8);
            }

            return View(produtos.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Produto produto = db.Produtos.Find(id);
            if (produto == null) return HttpNotFound();

            return View(produto);
        }

        public ActionResult AdminIndex(string busca)
        {
            if (Session["FuncionarioLogado"] == null) return RedirectToAction("Login", "Funcionario");

            var produtos = db.Produtos.AsQueryable();

            if (!string.IsNullOrEmpty(busca))
            {
                produtos = produtos.Where(p => p.Nome.Contains(busca) || (p.Jogo != null && p.Jogo.Nome.Contains(busca)));
            }

            return View(produtos.ToList());
        }

        public ActionResult Create()
        {
            if (Session["FuncionarioLogado"] == null) return RedirectToAction("Login", "Funcionario");

            ViewBag.IdJogo = new SelectList(db.Jogos, "IdJogo", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Produto produto, HttpPostedFileBase fotoUpload)
        {
            if (Session["FuncionarioLogado"] == null) return RedirectToAction("Login", "Funcionario");

            if (ModelState.IsValid)
            {
                if (fotoUpload != null && fotoUpload.ContentLength > 0)
                {
                    string nomeUnico = Guid.NewGuid().ToString() + "_" + Path.GetFileName(fotoUpload.FileName);
                    string caminhoPasta = Server.MapPath("~/Content/Image/");
                    if (!Directory.Exists(caminhoPasta)) Directory.CreateDirectory(caminhoPasta);
                    fotoUpload.SaveAs(Path.Combine(caminhoPasta, nomeUnico));
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
                    ModelState.AddModelError("", "Erro ao salvar: " + ex.Message);
                }
            }

            ViewBag.IdJogo = new SelectList(db.Jogos, "IdJogo", "Nome", produto.IdJogo);
            return View(produto);
        }

        public ActionResult Edit(int? id)
        {
            if (Session["FuncionarioLogado"] == null) return RedirectToAction("Login", "Funcionario");
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Produto produto = db.Produtos.Find(id);
            if (produto == null) return HttpNotFound();

            ViewBag.IdJogo = new SelectList(db.Jogos, "IdJogo", "Nome", produto.IdJogo);
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Produto produto, HttpPostedFileBase fotoUpload)
        {
            if (Session["FuncionarioLogado"] == null) return RedirectToAction("Login", "Funcionario");

            if (ModelState.IsValid)
            {
                var produtoNoBanco = db.Produtos.AsNoTracking().FirstOrDefault(p => p.IdProduto == produto.IdProduto);

                if (fotoUpload != null && fotoUpload.ContentLength > 0)
                {
                    if (produtoNoBanco != null && !string.IsNullOrEmpty(produtoNoBanco.imagem))
                    {
                        string caminhoAntigo = Server.MapPath("~" + produtoNoBanco.imagem);
                        if (System.IO.File.Exists(caminhoAntigo))
                        {
                            System.IO.File.Delete(caminhoAntigo);
                        }
                    }
                    string nomeUnico = Guid.NewGuid().ToString() + "_" + Path.GetFileName(fotoUpload.FileName);
                    fotoUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/Image/"), nomeUnico));
                    produto.imagem = "/Content/Image/" + nomeUnico;
                }
                else
                {
                    produto.imagem = produtoNoBanco?.imagem;
                }

                try
                {
                    db.Entry(produto).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("AdminIndex");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao atualizar: " + ex.Message);
                }
            }

            ViewBag.IdJogo = new SelectList(db.Jogos, "IdJogo", "Nome", produto.IdJogo);
            return View(produto);
        }

        // DELETE (Confirmado) - Limpeza de código
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["FuncionarioLogado"] == null) return RedirectToAction("Login", "Funcionario");

            Produto produto = db.Produtos.Find(id);
            if (produto != null)
            {
                if (!string.IsNullOrEmpty(produto.imagem))
                {
                    string caminho = Server.MapPath("~" + produto.imagem);
                    if (System.IO.File.Exists(caminho)) System.IO.File.Delete(caminho);
                }
                db.Produtos.Remove(produto);
                db.SaveChanges();
            }
            return RedirectToAction("AdminIndex");
        }

        public ActionResult Delete(int? id)
        {
            if (Session["FuncionarioLogado"] == null) return RedirectToAction("Login", "Funcionario");
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            Produto produto = db.Produtos.Find(id);
            if (produto == null) return HttpNotFound();
            return View(produto);
        }
    }
}