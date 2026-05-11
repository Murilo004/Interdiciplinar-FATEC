using Microsoft.AspNetCore.Mvc;
using Interdisciplinar.Web.Models;
using Interdisciplinar.Web.Data;
namespace Interdisciplinar.Web.Controllers;
public class AuthController : Controller
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Cadastro() //Create
    {
        return View();
    }
    [HttpPost]
    public IActionResult Cadastro(Usuario usuario)
    {
        usuario.Id = Guid.NewGuid().ToString();
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
        
        return RedirectToAction("Login");
    }

    public IActionResult Login() //Read
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(Usuario usuario)
    {
        var usuarioEncontrado = _context.Usuarios.FirstOrDefault(x =>
            x.Email == usuario.Email &&
            x.Senha == usuario.Senha);

        if (usuarioEncontrado != null)
        {
            HttpContext.Session.SetString("TipoUsuario", usuarioEncontrado.Perfil);
            HttpContext.Session.SetString("NomeUsuario", usuarioEncontrado.Nome);

            return RedirectToAction("Servicos", "Servicos");
        }

        return View();
    }

    public IActionResult Update()
    {
        string nomeUsuario = HttpContext.Session.GetString("NomeUsuario");

        Usuario usuario = _context.Usuarios.FirstOrDefault(u => u.Nome == nomeUsuario);

        if (usuario == null)
        {
            return RedirectToAction("Login");
        }

        return View(usuario);
    }
    [HttpPost]
    public IActionResult Update(Usuario usuario)
    {
        Usuario? usuarioBanco = _context.Usuarios.FirstOrDefault(u => u.Id == usuario.Id);

        if (usuarioBanco == null)
        {
            return RedirectToAction("Login");
        }

        usuarioBanco.Nome = usuario.Nome;
        usuarioBanco.Email = usuario.Email;
        if (!string.IsNullOrWhiteSpace(usuario.Senha))
        {
            usuarioBanco.Senha = usuario.Senha;
        }
        _context.SaveChanges();

        HttpContext.Session.SetString("NomeUsuario", usuarioBanco.Nome);

        return RedirectToAction("Update");
    }

    public IActionResult Delete(string id)
    {
        var usuario = _context.Usuarios.Find(id);

        if (usuario == null)
            return NotFound();

        return View(usuario);
    }
    [HttpPost]
    public IActionResult DeleteConfirmed(string id)
    {
        var usuario = _context.Usuarios.Find(id);

        if (usuario == null)
            return NotFound();

        _context.Usuarios.Remove(usuario);
        _context.SaveChanges();

        HttpContext.Session.Clear();

        return RedirectToAction("Login");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Login");
    }
}