using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardGameP.Models
{
    public class ItemPedido
    {
        [Key]
        public int IdItem { get; set; }

        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }

        public int IdPedido { get; set; }
        public int IdProduto { get; set; }

        public virtual Pedido Pedido { get; set; }
        public virtual Produto Produto { get; set; }
    }
}