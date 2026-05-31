using System.ComponentModel.DataAnnotations;

namespace Interdisciplinar.Models
{
    public class Servico 
    {
        public int Id { get; set;}

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set;}

        [Required(ErrorMessage = "O valor é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set;}

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set;}

        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
    }
}