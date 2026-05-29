using Interdisciplinar.Data;
using Interdisciplinar.Models;
using Microsoft.AspNetCore.Mvc;
namespace Interdisciplinar.Controllers;
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
    public IActionResult Create(Servico s)
    {
        _context.Servicos.Add(s);
        _context.SaveChanges();

        return RedirectToAction("Servicos");
    }

    public IActionResult Servicos() //Read
    {
        return View(_context.Servicos.ToList());
    }

    public IActionResult Update(int id)
    {
        var servico = _context.Servicos.FirstOrDefault(x => x.Id == id);

        return View(servico);
    }
    [HttpPost]
    public IActionResult Update(Servico s)
    {
        var servico = _context.Servicos.FirstOrDefault(x => x.Id == s.Id);

        if (servico != null)
        {
            servico.Nome = s.Nome;
            servico.Descricao = s.Descricao;
            servico.Valor = s.Valor;
            _context.SaveChanges();
        }
        
        return RedirectToAction("Servicos");
    }

    [HttpPost]
    public IActionResult Delete(int id)
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