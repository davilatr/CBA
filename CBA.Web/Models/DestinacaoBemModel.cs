using System.ComponentModel.DataAnnotations;

namespace CBA.Web.Models
{
    public class DestinacaoBemModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        public bool Ativo { get; set; }

    }
} 