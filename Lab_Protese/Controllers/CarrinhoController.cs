using Interdisciplinar.Data;
using Interdisciplinar.Models;
using Interdisciplinar.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Interdisciplinar.Controllers;
public class CarrinhoController : Controller
{
    private readonly AppDbContext _context;

    public CarrinhoController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Adicionar(int id, int quantidade) //Create e Update
    {
        var servico = _context.Servicos.FirstOrDefault(s => s.Id == id);

        if (servico != null)
        {
            for (int i = 0; i < quantidade; i++)
            {
                Lista.Carrinho.Add(servico);
            }
        }

        return RedirectToAction("Carrinho", "Carrinho");
    }

    public IActionResult Carrinho() //Read
    {
        return View("Carrinho", Lista.Carrinho);
    }
    
    public IActionResult Remover(int id) //Delete e Update
    {
        var item = Lista.Carrinho.FirstOrDefault(s => s.Id == id);

        if (item != null)
        {
            Lista.Carrinho.Remove(item);
        }

        return RedirectToAction("Carrinho");
    }

    [HttpPost]
    public IActionResult Cancelar() //Delete
    {
        Lista.Carrinho.Clear();
        
        return RedirectToAction("Carrinho", "Carrinho");
    }
}