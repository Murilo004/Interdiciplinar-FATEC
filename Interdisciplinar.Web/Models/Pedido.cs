namespace Interdisciplinar.Web.Models;
public class Pedido
{
    public string Id { get; set; }
    public string Dentista { get; set; }
    public DateOnly Data { get; set; }
    public string Status { get; set; }
    public List<ItemPedido> Itens { get; set; } = new();
    public int Quantidade { get; set; }
    public decimal Total { get; set; }
}