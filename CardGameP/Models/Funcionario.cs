using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CardGameP.Models
{
    public class Funcionario
    {
        [Key]
        public int id_funcionario { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100)]
        public string nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress]
        [StringLength(150)]
        public string email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100)]
        public string senha { get; set; }

        [StringLength(20)]
        public string telefone { get; set; }

        [Required(ErrorMessage = "O cargo é obrigatório")]
        [StringLength(50)]
        public string cargo { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }
    }
}