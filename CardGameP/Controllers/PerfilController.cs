using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CardGameP.Models;

namespace CardGameP.Controllers
{
    public class PerfilController : Controller
    {
        private LojaContext db = new LojaContext();

        public ActionResult Index()
        {
            var cliente = Session["ClienteLogado"] as Cliente;
            if (cliente == null)
            {
                TempData["Erro"] = "Você precisa estar logado para ver seu perfil.";
                return RedirectToAction("Login", "Cliente");
            }

            ViewBag.CarrinhoAtual = Session["Carrinho"] as List<ItemCarrinho> ?? new List<ItemCarrinho>();

            var historico = db.Pedidos
                              .Where(p => p.IdCliente == cliente.IdCliente)
                              .OrderByDescending(p => p.DataPedido)
                              .ToList();

            return View(historico);
        }
    }
}