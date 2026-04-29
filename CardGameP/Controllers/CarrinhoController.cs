using System;
using System.Collections.Generic;
using System.Linq; 
using System.Web.Mvc;
using CardGameP.Models;

namespace CardGameP.Controllers
{
    public class CarrinhoController : Controller
    {
        private LojaContext db = new LojaContext();

        public ActionResult Index()
        {
            var carrinho = Session["Carrinho"] as List<ItemCarrinho> ?? new List<ItemCarrinho>();
            return View(carrinho);
        }

        public ActionResult Adicionar(int id)
        {
            try
            {
                var produto = db.Produtos.Find(id);
                if (produto == null) return HttpNotFound();

                var carrinho = Session["Carrinho"] as List<ItemCarrinho> ?? new List<ItemCarrinho>();

                var item = carrinho.FirstOrDefault(x => x.IdProduto == id);
                if (item == null)
                {
                    carrinho.Add(new ItemCarrinho
                    {
                        IdProduto = produto.IdProduto,
                        Nome = produto.Nome,
                        Preco = produto.Preco,
                        Quantidade = 1
                    });
                }
                else
                {
                    item.Quantidade++;
                }

                Session["Carrinho"] = carrinho;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao adicionar: " + ex.Message;
                return RedirectToAction("Index", "Produto");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalizarPedido()
        {
            var cliente = Session["ClienteLogado"] as Cliente;
            if (cliente == null) return RedirectToAction("Login", "Cliente");

            var carrinho = Session["Carrinho"] as List<ItemCarrinho>;
            if (carrinho == null || !carrinho.Any()) return RedirectToAction("Index", "Produto");

            using (var transacao = db.Database.BeginTransaction())
            {
                try
                {
                    var novoPedido = new Pedido
                    {
                        IdCliente = cliente.IdCliente,
                        DataPedido = DateTime.Now,
                        Status = "Finalizado"
                    };
                    db.Pedidos.Add(novoPedido);
                    db.SaveChanges();

                    foreach (var item in carrinho)
                    {
                        var detalhe = new ItemPedido
                        {
                            IdPedido = novoPedido.IdPedido,
                            IdProduto = item.IdProduto,
                            Quantidade = item.Quantidade,
                            PrecoUnitario = item.Preco
                        };

                        db.ItensPedido.Add(detalhe);

                        var p = db.Produtos.Find(item.IdProduto);
                        if (p != null)
                        {
                            p.Estoque -= item.Quantidade;
                        }
                    }

                    db.SaveChanges();
                    transacao.Commit();

                    Session["Carrinho"] = null;
                    return RedirectToAction("Sucesso");
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    TempData["Erro"] = "Erro ao processar pedido: " + ex.Message;
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Sucesso()
        {
            return View();
        }
    }
}