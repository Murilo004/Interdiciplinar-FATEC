using Microsoft.AspNetCore.Mvc;
using Interdisciplinar.Web.Models;
using Interdisciplinar.Web.Data;
namespace Interdisciplinar.Web.Controllers;
public class ServicosController : Controller
{
    private readonly AppDbContext _context;

    public ServicosController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Servicos s)
    {
        s.Id = Guid.NewGuid().ToString();
        _context.Servicos.Add(s);
        _context.SaveChanges();

        return RedirectToAction("Servicos");
    }

    public IActionResult Servicos() //Read
    {
        return View(_context.Servicos.ToList());
    }

    public IActionResult Update(string id)
    {
        var servico = _context.Servicos.FirstOrDefault(x => x.Id == id);

        return View(servico);
    }
    [HttpPost]
    public IActionResult Update(Servicos s)
    {
        var servico = _context.Servicos.FirstOrDefault(x => x.Id == s.Id);

        if (servico != null)
        {
            servico.Nome = s.Nome;
            servico.Descricao = s.Descricao;
            servico.Preco = s.Preco;
            _context.SaveChanges();
        }
        
        return RedirectToAction("Servicos");
    }

    [HttpPost]
    public IActionResult Delete(string id)
    {
        var servico = _context.Servicos.FirstOrDefault(x => x.Id == id);

        if (servico != null)
        {
            _context.Servicos.Remove(servico);
            _context.SaveChanges();
        }

        return RedirectToAction("Servicos");
    }
}