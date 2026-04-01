using System.ComponentModel.DataAnnotations;

namespace CardGameP.Models
{
    public class Produto
    {
        [Key]
        public int IdProduto { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Jogo { get; set; }

        public string Raridade { get; set; }

        [Required]
        public decimal Preco { get; set; }

        public int Estoque { get; set; }

        public string imagem { get; set; }
    }
}