using System.Collections.Generic;

namespace Interdisciplinar.Models
{
    public class Dentista : Pessoa  
    {
        public string Cro { get; set;}
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}