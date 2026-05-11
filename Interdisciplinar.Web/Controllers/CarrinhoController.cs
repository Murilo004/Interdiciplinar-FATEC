using Microsoft.AspNetCore.Mvc;
using Interdisciplinar.Web.Models;
using Interdisciplinar.Web.Data;
namespace Interdisciplinar.Web.Controllers;
public class CarrinhoController : Controller
{
    private readonly AppDbContext _context;

    public CarrinhoController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Adicionar(string id, int quantidade) //Create e Update
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
    
    public IActionResult Remover(string id) //Delete e Update
    {
        var item = Lista.Carrinho.FirstOrDefault(s => s.Id == id);

        if (item != null)
        {
            Lista.Carrinho.Remove(item);
        }

        return RedirectToAction("Carrinho");
    }

    public IActionResult Cancelar() //Delete
    {
        Lista.Carrinho.Clear();
        
        return RedirectToAction("Carrinho", "Carrinho");
    }
}