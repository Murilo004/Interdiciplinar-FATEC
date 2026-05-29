using System.ComponentModel.DataAnnotations;

namespace Interdisciplinar.ViewModels.Auth
{
    public class CadastroViewModel
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Endereco { get; set; }

        [Required]
        public string Telefone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]
        public string TipoUsuario { get; set; }

        public string? Cro { get; set; }
        public string? Cnpj { get; set; }
    }
}