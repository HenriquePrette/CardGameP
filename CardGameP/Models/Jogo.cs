using CardGameP.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Jogo
{
    [Key]
    public int IdJogo { get; set; }
    [Required]
    public string Nome { get; set; }

    public virtual ICollection<Produto> Produtos { get; set; }
}