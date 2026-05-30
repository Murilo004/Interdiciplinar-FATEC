using Interdisciplinar.Enums;
namespace Interdisciplinar.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public StatusPedido Status { get; set; } = StatusPedido.Pendente;
        public DateOnly Data { get; set; }
        public decimal ValorTotal { get; set; }
        public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
        public int ?DentistaId { get; set; }
        public Dentista Dentista { get; set; }
        public int ?ProteticoId { get; set; }
        public Protetico Protetico { get; set; }
        public int ?EnderecoId { get; set; }
        public Entrega Entrega { get; set; }
        public Coleta Coleta { get; set; }
    }
}