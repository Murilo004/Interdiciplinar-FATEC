using Interdisciplinar.Data;
using Interdisciplinar.Models;
using Interdisciplinar.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Interdisciplinar.Controllers;
public class PedidosController : Controller
{
    private readonly AppDbContext _context;

    public PedidosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Create()
    {
        if (!Lista.Carrinho.Any())
        {
            return RedirectToAction("Carrinho", "Carrinho");
        }

        var pedido = new Pedido
        {
            Data = DateOnly.FromDateTime(DateTime.Now),

            DentistaId = HttpContext.Session.GetInt32("IdUsuario"),

            Status = StatusPedido.Pendente
        };

        pedido.Itens = Lista.Carrinho
            .GroupBy(x => x.Id)
            .Select(g => new ItemPedido
            {
                PedidoId = pedido.Id,

                ServicoId = g.First().Id,

                Quantidade = g.Count(),

                ValorUnitario = g.First().Valor
            })
            .ToList();

        pedido.ValorTotal = pedido.Itens.Sum(i => i.Quantidade * i.ValorUnitario);

        _context.Pedidos.Add(pedido);

        _context.SaveChanges();

        Lista.Carrinho.Clear();

        return RedirectToAction("Pedidos");
    }

    public IActionResult Pedidos() //Read
    {
        return View(_context.Pedidos.Include(p => p.Dentista).Include(p => p.Protetico).Include(p => p.Itens).ThenInclude(i => i.Servico).ToList());
    }

    public IActionResult Update(int id)
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
    public IActionResult Delete(int id)
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