using Microsoft.EntityFrameworkCore;
using Interdisciplinar.Models;

namespace Interdisciplinar.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Dentista> Dentistas { get; set; }
        public DbSet<Protetico> Proteticos { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<Entrega> Entregas { get; set; }
        public DbSet<Coleta> Coletas { get; set; }
        public DbSet<Entregador> Entregadores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ItemPedido>().HasKey(ip => new { ip.PedidoId, ip.ServicoId });
            modelBuilder.Entity<Pessoa>().ToTable("Pessoas");
            modelBuilder.Entity<Protetico>().ToTable("Proteticos");
            modelBuilder.Entity<Dentista>().ToTable("Dentistas");
        }
    }
}