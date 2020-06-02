using System.ComponentModel.DataAnnotations;


namespace CBA.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O campo usuário é obrigatório ¯\\_(ツ)_ /¯")]
        [Display(Name = "Usuário:")]
        public string Usuario { get; set; }


        [Required(ErrorMessage = "O campo senha é obrigatório ¯\\_(ツ)_/¯")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha:")]
        public string Senha { get; set; }

        [Display(Name = "Lembre-se de mim")]
        public bool LembrarMe { get; set; }
    }
}