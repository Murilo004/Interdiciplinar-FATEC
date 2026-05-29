namespace Interdisciplinar.Models
{
    public class Servico 
    {
        public int Id { get; set;}
        public string Nome { get; set;}
        public decimal Valor { get; set;}
        public string Descricao { get; set;}
        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
    }
}