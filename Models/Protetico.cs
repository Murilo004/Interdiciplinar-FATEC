using System.Collections.Generic;

namespace Interdisciplinar.Models
{
    public class Protetico : Pessoa  
    {
        public string Cnpj { get; set;}
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}