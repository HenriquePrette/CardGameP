using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardGameP.Models
{
    public class Produto
    {
        [Key]
        public int IdProduto { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; }

        public int IdJogo { get; set; }

        [ForeignKey("IdJogo")]
        public virtual Jogo Jogo { get; set; }

        public string Raridade { get; set; }

        [Required]
        public decimal Preco { get; set; }

        public int Estoque { get; set; }

        public string imagem { get; set; }
    }
}