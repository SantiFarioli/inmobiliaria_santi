using inmobiliaria_santi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace inmobiliaria_santi.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly RepositorioUsuario _repo;

        public UsuarioController()
        {
            _repo = new RepositorioUsuario();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string contrasena)
        {
            var usuario = _repo.ValidarUsuario(email, contrasena);

            if (usuario == null)
            {
                ViewBag.Mensaje = "Usuario o contraseña incorrectos";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.email ?? ""),
                new Claim("NombreCompleto", usuario.nombre + " " + usuario.apellido),
                new Claim(ClaimTypes.Role, usuario.rol == 1 ? "Administrador" : "Empleado"),
                new Claim("IdUsuario", usuario.idUsuario.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
            );

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuario");
        }

        [Authorize]
        public IActionResult Perfil()
        {
            var email = User.Identity?.Name ?? "";
            if(string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Usuarios");
            }
            var usuario = _repo.ObtenerPorEmail(email);
            return View(usuario);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Alta()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Alta(Usuario u)
        {
            _repo.Alta(u);
            TempData["Mensaje"] = "Usuario creado correctamente";
            return RedirectToAction("Index", "Home");
        }
    }
}
