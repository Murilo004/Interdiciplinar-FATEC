namespace Interdisciplinar.Web.Models;
public class ItemPedido
{
    public string Id { get; set; }
    public string ServicoId { get; set; }
    public Servicos Servico { get; set; }
    public int Quantidade { get; set; }
    public decimal Subtotal { get; set; }
}