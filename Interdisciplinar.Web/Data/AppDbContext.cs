using Microsoft.EntityFrameworkCore;
using Interdisciplinar.Web.Models;

namespace Interdisciplinar.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Servicos> Servicos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
}