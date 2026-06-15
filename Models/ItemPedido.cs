namespace Interdisciplinar.Models
{
    public class ItemPedido
    {
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }
        public int ServicoId { get; set; }
        public Servico Servico { get; set; }
    }
}