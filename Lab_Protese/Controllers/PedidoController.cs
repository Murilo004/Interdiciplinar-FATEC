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

    private bool IsLogado() =>
        HttpContext.Session.GetInt32("IdUsuario") != null;

    [HttpPost]
    public IActionResult Create()
    {
        int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
        string? tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

        if (idUsuario == null || tipoUsuario != "Dentista")
            return RedirectToAction("Login", "Auth");

        if (!Lista.Carrinho.Any())
            return RedirectToAction("Carrinho", "Carrinho");

        var pedido = new Pedido
        {
            Data = DateOnly.FromDateTime(DateTime.Now),
            DentistaId = idUsuario,
            Status = StatusPedido.Pendente
        };

        // BUG 2 (mantido): Salvar antes de criar os itens para gerar o Id
        _context.Pedidos.Add(pedido);
        _context.SaveChanges();

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
        _context.SaveChanges();

        Lista.Carrinho.Clear();
        return RedirectToAction("Pedidos");
    }

    public IActionResult Pedidos()
    {
        int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
        string? tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

        if (idUsuario == null)
            return RedirectToAction("Login", "Auth");

        IQueryable<Pedido> query = _context.Pedidos
            .Include(p => p.Dentista)
            .Include(p => p.Protetico)
            .Include(p => p.Itens).ThenInclude(i => i.Servico);

        // BUG 3 (mantido): Filtrar por usuário logado
        if (tipoUsuario == "Dentista")
            query = query.Where(p => p.DentistaId == idUsuario);

        return View(query.ToList());
    }

    public IActionResult Update(int id)
    {
        if (!IsLogado()) return RedirectToAction("Login", "Auth");

        // BUG 21 CORRIGIDO: Apenas Protéticos devem poder alterar o status do pedido
        if (HttpContext.Session.GetString("TipoUsuario") != "Protetico")
            return Forbid();

        var pedido = _context.Pedidos.FirstOrDefault(x => x.Id == id);
        if (pedido == null) return NotFound();
        return View(pedido);
    }

    [HttpPost]
    public IActionResult Update(Pedido p)
    {
        if (!IsLogado()) return RedirectToAction("Login", "Auth");
        if (HttpContext.Session.GetString("TipoUsuario") != "Protetico")
            return Forbid();

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
        if (!IsLogado()) return RedirectToAction("Login", "Auth");

        // BUG 5 (mantido): Remover itens antes do pedido
        var pedido = _context.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefault(x => x.Id == id);

        if (pedido != null)
        {
            _context.ItensPedido.RemoveRange(pedido.Itens);
            _context.Pedidos.Remove(pedido);
            _context.SaveChanges();
        }

        return RedirectToAction("Pedidos");
    }
}