using Interdisciplinar.Data;
using Interdisciplinar.Models;
using Microsoft.AspNetCore.Mvc;
namespace Interdisciplinar.Controllers;
public class CarrinhoController : Controller
{
    private readonly AppDbContext _context;

    public CarrinhoController(AppDbContext context)
    {
        _context = context;
    }
    
    private bool IsDentista() =>
        HttpContext.Session.GetString("TipoUsuario") == "Dentista";

    private bool IsLogado() =>
        HttpContext.Session.GetInt32("IdUsuario") != null;

    public IActionResult Adicionar(int id, int quantidade)
    {
        if (!IsLogado()) return RedirectToAction("Login", "Auth");
        if (!IsDentista()) return Forbid();

        if (quantidade <= 0) quantidade = 1;

        var servico = _context.Servicos.FirstOrDefault(s => s.Id == id);
        if (servico != null)
        {
            for (int i = 0; i < quantidade; i++)
                Lista.Carrinho.Add(servico);
        }

        return RedirectToAction("Carrinho", "Carrinho");
    }

    public IActionResult Carrinho()
    {
        if (!IsLogado()) return RedirectToAction("Login", "Auth");
        if (!IsDentista()) return Forbid();
        return View("Carrinho", Lista.Carrinho);
    }

    public IActionResult Remover(int id)
    {
        if (!IsLogado()) return RedirectToAction("Login", "Auth");
        if (!IsDentista()) return Forbid();

        var item = Lista.Carrinho.FirstOrDefault(s => s.Id == id);
        if (item != null)
            Lista.Carrinho.Remove(item);

        return RedirectToAction("Carrinho");
    }

    [HttpPost]
    public IActionResult Cancelar()
    {
        if (!IsLogado()) return RedirectToAction("Login", "Auth");
        Lista.Carrinho.Clear();
        return RedirectToAction("Carrinho", "Carrinho");
    }
}