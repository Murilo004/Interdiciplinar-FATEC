using Microsoft.AspNetCore.Mvc;
using Interdisciplinar.Data;
using Interdisciplinar.Models;
using Interdisciplinar.ViewModels.Auth;

namespace Interdisciplinar.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(CadastroViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.TipoUsuario == "Dentista")
            {
                var dentista = new Dentista
                {
                    Nome = model.Nome,
                    Endereco = model.Endereco,
                    Telefone = model.Telefone,
                    Email = model.Email,
                    Senha = model.Senha,
                    Cro = model.Cro
                };

                _context.Dentistas.Add(dentista);
            }
            else if (model.TipoUsuario == "Protetico")
            {
                var protetico = new Protetico
                {
                    Nome = model.Nome,
                    Endereco = model.Endereco,
                    Telefone = model.Telefone,
                    Email = model.Email,
                    Senha = model.Senha,
                    Cnpj = model.Cnpj
                };

                _context.Proteticos.Add(protetico);
            }

            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Console.WriteLine(_context.Dentistas.Count());
            Console.WriteLine(_context.Proteticos.Count());

            var dentista = _context.Dentistas.FirstOrDefault(x =>
                x.Email == model.Email &&
                x.Senha == model.Senha);

            if (dentista != null)
            {
                HttpContext.Session.SetInt32("IdUsuario", dentista.Id);
                HttpContext.Session.SetString("NomeUsuario", dentista.Nome);
                HttpContext.Session.SetString("TipoUsuario", "Dentista");

                return RedirectToAction("Servicos", "Servicos");
            }

            var protetico = _context.Proteticos.FirstOrDefault(x =>
                x.Email == model.Email &&
                x.Senha == model.Senha);

            if (protetico != null)
            {
                HttpContext.Session.SetInt32("IdUsuario", protetico.Id);
                HttpContext.Session.SetString("NomeUsuario", protetico.Nome);
                HttpContext.Session.SetString("TipoUsuario", "Protetico");

                return RedirectToAction("Servicos", "Servicos");
            }

            ViewBag.Erro = "E-mail ou senha inválidos";

            return View(model);
        }
        public IActionResult Update()
        {
            int ?idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            string tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

            if (tipoUsuario == "Dentista")
            {
                var dentista = _context.Dentistas.FirstOrDefault(x => x.Id == idUsuario);

                if (dentista == null)
                {
                    return RedirectToAction("Login");
                }

                return View(dentista);
            }

            var protetico = _context.Proteticos.FirstOrDefault(x => x.Id == idUsuario);

            if (protetico == null)
            {
                return RedirectToAction("Login");
            }

            return View(protetico);
        }
        [HttpPost]
        public IActionResult Update(Pessoa pessoa)
        {
            string tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

            if (tipoUsuario == "Dentista")
            {
                var dentista = _context.Dentistas.FirstOrDefault(x => x.Id == pessoa.Id);

                if (dentista == null)
                {
                    return RedirectToAction("Login");
                }

                dentista.Nome = pessoa.Nome;
                dentista.Email = pessoa.Email;
                dentista.Telefone = pessoa.Telefone;
                dentista.Endereco = pessoa.Endereco;

                if (!string.IsNullOrWhiteSpace(pessoa.Senha))
                {
                    dentista.Senha = pessoa.Senha;
                }
            }
            else
            {
                var protetico = _context.Proteticos.FirstOrDefault(x => x.Id == pessoa.Id);

                if (protetico == null)
                {
                    return RedirectToAction("Login");
                }

                protetico.Nome = pessoa.Nome;
                protetico.Email = pessoa.Email;
                protetico.Telefone = pessoa.Telefone;
                protetico.Endereco = pessoa.Endereco;

                if (!string.IsNullOrWhiteSpace(pessoa.Senha))
                {
                    protetico.Senha = pessoa.Senha;
                }
            }

            _context.SaveChanges();

            HttpContext.Session.SetString("NomeUsuario", pessoa.Nome);

            return RedirectToAction("Update");
        }

        public IActionResult Delete(int id)
        {
            string tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

            if (tipoUsuario == "Dentista")
            {
                var dentista = _context.Dentistas.Find(id);

                if (dentista == null)
                {
                    return NotFound();
                }

                return View(dentista);
            }

            var protetico = _context.Proteticos.Find(id);

            if (protetico == null)
            {
                return NotFound();
            }

            return View(protetico);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            string tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

            if (tipoUsuario == "Dentista")
            {
                var dentista = _context.Dentistas.Find(id);

                if (dentista != null)
                {
                    _context.Dentistas.Remove(dentista);
                }
            }
            else
            {
                var protetico = _context.Proteticos.Find(id);

                if (protetico != null)
                {
                    _context.Proteticos.Remove(protetico);
                }
            }

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
}