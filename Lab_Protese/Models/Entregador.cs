namespace Interdisciplinar.Models
{
    public class Entregador  
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public ICollection<Entrega> Entregas { get; set; } = new List<Entrega>();
        public ICollection<Coleta> Coletas { get; set; } = new List<Coleta>();
    }
}