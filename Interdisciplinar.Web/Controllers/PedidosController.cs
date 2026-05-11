using Microsoft.AspNetCore.Mvc;
using Interdisciplinar.Web.Models;
using Interdisciplinar.Web.Data;
using Microsoft.EntityFrameworkCore;
namespace Interdisciplinar.Web.Controllers;
public class PedidosController : Controller
{
    private readonly AppDbContext _context;

    public PedidosController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        if (!Lista.Carrinho.Any())
        {
            return RedirectToAction("Carrinho", "Carrinho");
        }

        var pedido = new Pedido();
        pedido.Id = Guid.NewGuid().ToString();
        pedido.Data = DateOnly.FromDateTime(DateTime.Now);
        pedido.Dentista = HttpContext.Session.GetString("NomeUsuario");
        pedido.Status = "Pendente";
        pedido.Itens = Lista.Carrinho.GroupBy(x => x.Id).Select(g => new ItemPedido {
            Id = Guid.NewGuid().ToString(),
            ServicoId = g.First().Id,
            Quantidade = g.Count(),
            Subtotal = g.Sum(x => x.Preco)
        }).ToList();
        pedido.Total = pedido.Itens.Sum(i => i.Subtotal);
        _context.Pedidos.Add(pedido);
        _context.SaveChanges();
        Lista.Carrinho.Clear();

        return RedirectToAction("Pedidos");
    }

    public IActionResult Pedidos() //Read
    {
        return View(_context.Pedidos.Include(p => p.Itens).ThenInclude(i => i.Servico).ToList());
    }

    public IActionResult Update(string id)
    {
        var pedido = _context.Pedidos.FirstOrDefault(x => x.Id == id);

        return View(pedido);
    }
    [HttpPost]
    public IActionResult Update(Pedido p)
    {
        var pedido = _context.Pedidos.FirstOrDefault(x => x.Id == p.Id);

        if (pedido != null)
        {
            pedido.Status = p.Status;
            _context.SaveChanges();
        }
        
        return RedirectToAction("Pedidos");
    }

    [HttpPost]
    public IActionResult Delete(string id)
    {
        var pedido = _context.Pedidos.FirstOrDefault(x => x.Id == id);

        if (pedido != null)
        {
            _context.Pedidos.Remove(pedido);
            _context.SaveChanges();
        }

        return RedirectToAction("Pedidos");
    }
}