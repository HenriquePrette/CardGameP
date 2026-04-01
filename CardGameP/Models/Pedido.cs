using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardGameP.Models
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }

        public DateTime DataPedido { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }

        public int IdCliente { get; set; }
        public int IdFuncionario { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Funcionario Funcionario { get; set; }
        public virtual ICollection<ItemPedido> Itens { get; set; }
    }
}
