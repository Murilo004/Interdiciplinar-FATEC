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
            // BUG 14 CORRIGIDO: CRO obrigatório para Dentista e CNPJ para Protético
            // precisam ser validados no servidor (não só escondidos via JS)
            if (model.TipoUsuario == "Dentista" && string.IsNullOrWhiteSpace(model.Cro))
            {
                ModelState.AddModelError("Cro", "O CRO é obrigatório para Dentistas.");
            }
            if (model.TipoUsuario == "Protetico" && string.IsNullOrWhiteSpace(model.Cnpj))
            {
                ModelState.AddModelError("Cnpj", "O CNPJ é obrigatório para Protéticos.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // BUG 6 (mantido): Verificar e-mail duplicado
            bool emailExiste = _context.Dentistas.Any(x => x.Email == model.Email)
                            || _context.Proteticos.Any(x => x.Email == model.Email);

            if (emailExiste)
            {
                ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");
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
            else
            {
                ModelState.AddModelError("TipoUsuario", "Tipo de usuário inválido.");
                return View(model);
            }

            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            // BUG 15 CORRIGIDO: Usuário já logado acessando /Login ficava na página de login
            if (HttpContext.Session.GetInt32("IdUsuario") != null)
            {
                return RedirectToAction("Servicos", "Servicos");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

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

        // BUG 16 CORRIGIDO: Actions sensíveis sem proteção de sessão — criado filtro auxiliar
        public IActionResult Update()
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            string? tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

            if (idUsuario == null || tipoUsuario == null)
                return RedirectToAction("Login");

            if (tipoUsuario == "Dentista")
            {
                var dentista = _context.Dentistas.FirstOrDefault(x => x.Id == idUsuario);
                if (dentista == null) return RedirectToAction("Login");
                return View(dentista);
            }

            var protetico = _context.Proteticos.FirstOrDefault(x => x.Id == idUsuario);
            if (protetico == null) return RedirectToAction("Login");
            return View(protetico);
        }

        [HttpPost]
        public IActionResult Update(Pessoa pessoa)
        {
            string? tipoUsuario = HttpContext.Session.GetString("TipoUsuario");
            int? idSessao = HttpContext.Session.GetInt32("IdUsuario");

            if (tipoUsuario == null || idSessao == null)
                return RedirectToAction("Login");

            // BUG 17 CORRIGIDO: O Id vinha do formulário (campo hidden), permitindo
            // que um usuário alterasse dados de outro passando id diferente na requisição.
            // A correção usa o id da sessão como fonte de verdade.
            pessoa.Id = idSessao.Value;

            if (tipoUsuario == "Dentista")
            {
                var dentista = _context.Dentistas.FirstOrDefault(x => x.Id == pessoa.Id);
                if (dentista == null) return RedirectToAction("Login");

                dentista.Nome = pessoa.Nome;
                dentista.Email = pessoa.Email;
                dentista.Telefone = pessoa.Telefone;
                dentista.Endereco = pessoa.Endereco;

                if (!string.IsNullOrWhiteSpace(pessoa.Senha))
                    dentista.Senha = pessoa.Senha;
            }
            else
            {
                var protetico = _context.Proteticos.FirstOrDefault(x => x.Id == pessoa.Id);
                if (protetico == null) return RedirectToAction("Login");

                protetico.Nome = pessoa.Nome;
                protetico.Email = pessoa.Email;
                protetico.Telefone = pessoa.Telefone;
                protetico.Endereco = pessoa.Endereco;

                if (!string.IsNullOrWhiteSpace(pessoa.Senha))
                    protetico.Senha = pessoa.Senha;
            }

            _context.SaveChanges();
            HttpContext.Session.SetString("NomeUsuario", pessoa.Nome);
            return RedirectToAction("Update");
        }

        public IActionResult Delete(int id)
        {
            string? tipoUsuario = HttpContext.Session.GetString("TipoUsuario");
            if (tipoUsuario == null) return RedirectToAction("Login");

            if (tipoUsuario == "Dentista")
            {
                var dentista = _context.Dentistas.Find(id);
                if (dentista == null) return NotFound();
                return View(dentista);
            }

            var protetico = _context.Proteticos.Find(id);
            if (protetico == null) return NotFound();
            return View(protetico);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            string? tipoUsuario = HttpContext.Session.GetString("TipoUsuario");
            int? idSessao = HttpContext.Session.GetInt32("IdUsuario");

            // BUG 18 CORRIGIDO: Qualquer usuário logado podia deletar qualquer conta
            // passando um id diferente. A correção força o id da sessão.
            if (idSessao == null || tipoUsuario == null)
                return RedirectToAction("Login");

            if (tipoUsuario == "Dentista")
            {
                var dentista = _context.Dentistas.Find(idSessao.Value);
                if (dentista != null) _context.Dentistas.Remove(dentista);
            }
            else
            {
                var protetico = _context.Proteticos.Find(idSessao.Value);
                if (protetico != null) _context.Proteticos.Remove(protetico);
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