//Importa as bibliotecas
using Microsoft.AspNetCore.Mvc;
using SistemaVoltCar.Models;
using SistemaVoltCar.Repositorio;

namespace SistemaVoltCar.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepositorio _usuarioRepositorio;

        public UsuarioController(UsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string senha)
        {
            var usuario = _usuarioRepositorio.ObterUsuario(email);

            if (usuario != null && usuario.Senha == senha)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Email / Senha Inválidos");

            return View();
        }

        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _usuarioRepositorio.CadastrarUsuario(usuario);
                TempData["MensagemSucesso"] = "Cadastro realizado com sucesso! Faça login.";
                return RedirectToAction("Cadastro", "Usuario");
            }
            return View(usuario);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
