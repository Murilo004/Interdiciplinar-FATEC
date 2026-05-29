namespace Interdisciplinar.Models
{
    public class Entrega  
    {
        public int Id { get; set;}
        public DateOnly Data { get; set;}
        public TimeOnly Hora { get; set;}
        public int EntregadorId { get; set; }
        public Entregador Entregador { get; set; }
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }
    }
}