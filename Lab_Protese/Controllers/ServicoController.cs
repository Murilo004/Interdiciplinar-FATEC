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

    // BUG 19 CORRIGIDO: Qualquer usuário (inclusive Dentista) podia acessar Create/Update/Delete de serviços.
    // Apenas Protéticos devem poder gerenciar serviços.
    private bool IsProtetico() =>
        HttpContext.Session.GetString("TipoUsuario") == "Protetico";

    private bool IsLogado() =>
        HttpContext.Session.GetInt32("IdUsuario") != null;

    public IActionResult Create()
    {
        if (!IsLogado()) return RedirectToAction("Login", "Auth");
        if (!IsProtetico()) return Forbid();
        return View();
    }

    [HttpPost]
    public IActionResult Create(Servico s)
    {
        if (!IsLogado()) return RedirectToAction("Login", "Auth");
        if (!IsProtetico()) return Forbid();

        if (!ModelState.IsValid)
            return View(s);

        _context.Servicos.Add(s);
        _context.SaveChanges();
        return RedirectToAction("Servicos");
    }

    public IActionResult Servicos()
    {
        if (!IsLogado()) return RedirectToAction("Login", "Auth");
        return View(_context.Servicos.ToList());
    }

    public IActionResult Update(int id)
    {
        if (!IsLogado()) return RedirectToAction("Login", "Auth");
        if (!IsProtetico()) return Forbid();

        var servico = _context.Servicos.FirstOrDefault(x => x.Id == id);
        if (servico == null) return NotFound();
        return View(servico);
    }

    [HttpPost]
    public IActionResult Update(Servico s)
    {
        if (!IsLogado()) return RedirectToAction("Login", "Auth");
        if (!IsProtetico()) return Forbid();

        if (!ModelState.IsValid)
            return View(s);

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
        if (!IsLogado()) return RedirectToAction("Login", "Auth");
        if (!IsProtetico()) return Forbid();

        var servico = _context.Servicos.FirstOrDefault(x => x.Id == id);
        if (servico != null)
        {
            _context.Servicos.Remove(servico);
            _context.SaveChanges();
        }

        return RedirectToAction("Servicos");
    }
}