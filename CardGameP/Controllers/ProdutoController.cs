using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGameP.Models;
using System.Collections.Generic; // Necessário para Listas

namespace CardGameP.Controllers
{
    public class ProdutoController : Controller
    {
        private LojaContext db = new LojaContext();

        // 1. INDEX PÚBLICA - Corrigido para evitar erro se Jogo for nulo
        public ActionResult Index(string busca)
        {
            var produtos = db.Produtos.AsQueryable();

            if (!string.IsNullOrEmpty(busca))
            {
                // Usamos ?. para evitar erro caso algum produto não tenha jogo associado
                produtos = produtos.Where(p => p.Nome.Contains(busca) || (p.Jogo != null && p.Jogo.Nome.Contains(busca)));
            }
            else
            {
                // Se não houver busca, pegamos aleatórios
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

        // 2. ADMIN INDEX - Corrigido filtro de busca
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

        // 3. CREATE (GET) - Corrigido para carregar a lista de Jogos
        public ActionResult Create()
        {
            if (Session["FuncionarioLogado"] == null) return RedirectToAction("Login", "Funcionario");

            // Esta linha permite que o DropDownList na View funcione
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
                // Lógica de upload de imagem mantida
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

            // Recarrega a lista se houver erro de validação para a página não quebrar
            ViewBag.IdJogo = new SelectList(db.Jogos, "IdJogo", "Nome", produto.IdJogo);
            return View(produto);
        }

        // 4. EDIT (GET) - Corrigido para carregar a lista de Jogos
        public ActionResult Edit(int? id)
        {
            if (Session["FuncionarioLogado"] == null) return RedirectToAction("Login", "Funcionario");
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Produto produto = db.Produtos.Find(id);
            if (produto == null) return HttpNotFound();

            // Carrega os jogos e já deixa selecionado o jogo atual do produto
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
                    // Lógica de troca de imagem mantida
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

        // Método auxiliar para o GET do Delete
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