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
        private readonly RepositorioUsuario _repositorio;

        public UsuarioController(RepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
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
            var usuario = _repositorio.ValidarUsuario(email, contrasena);

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
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Usuario");
            }
            var usuario = _repositorio.ObtenerPorEmail(email);
            return View(usuario);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            var usuarios = _repositorio.ObtenerTodos();
            return View(usuarios);
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
            try
            {
                if (u.AvatarFile != null && u.AvatarFile.Length > 0)
                {
                    var carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "avatares");
                    if (!Directory.Exists(carpetaDestino))
                        Directory.CreateDirectory(carpetaDestino);

                    var nombreArchivo = $"avatar_{Guid.NewGuid():N}" + Path.GetExtension(u.AvatarFile.FileName);
                    var rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        u.AvatarFile.CopyTo(stream);
                    }

                    u.avatar = "/img/avatares/" + nombreArchivo;
                }

                u.contrasena = HashHelper.CalcularHash(u.contrasena ?? "");
                _repositorio.Alta(u);

                TempData["Mensaje"] = "Usuario creado correctamente";
                TempData["Tipo"] = "success";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al crear el usuario: " + ex.Message;
                TempData["Tipo"] = "error";
                return View(u);
            }
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            var usuario = _repositorio.ObtenerPorId(id);
            if (usuario == null)
            {
                TempData["Mensaje"] = "Usuario no encontrado.";
                TempData["Tipo"] = "warning";
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(Usuario u)
        {
            try
            {
                if (u.AvatarFile != null && u.AvatarFile.Length > 0)
                {
                    var nombreArchivo = $"avatar_{Guid.NewGuid():N}" + Path.GetExtension(u.AvatarFile.FileName);
                    var ruta = Path.Combine("wwwroot/img/avatares", nombreArchivo);
                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        u.AvatarFile.CopyTo(stream);
                    }
                    u.avatar = "/img/avatares/" + nombreArchivo;
                }

                _repositorio.ActualizarUsuarioAdmin(u);
                TempData["Mensaje"] = "Usuario actualizado correctamente";
                TempData["Tipo"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al actualizar usuario: " + ex.Message;
                TempData["Tipo"] = "error";
                return View(u);
            }
        }

        [Authorize(Roles = "Empleado,Administrador")]
        public IActionResult EditarPerfil()
        {
            var email = User.Identity?.Name ?? "";
            var usuario = _repositorio.ObtenerPorEmail(email);
            return View(usuario);
        }

        [HttpPost]
        [Authorize(Roles = "Empleado,Administrador")]
        public IActionResult EditarPerfil(Usuario u, string nuevaContrasena)
        {
            try
            {
                var email = User.Identity?.Name ?? "";
                var original = _repositorio.ObtenerPorEmail(email);

                if (original == null)
                {
                    TempData["Mensaje"] = "Error: Usuario no encontrado.";
                    TempData["Tipo"] = "error";
                    return View(u);
                }

                original.nombre = u.nombre;
                original.apellido = u.apellido;
                original.email = u.email;

                if (!string.IsNullOrEmpty(nuevaContrasena))
                {
                    original.contrasena = HashHelper.CalcularHash(nuevaContrasena);
                }

                if (u.AvatarFile != null && u.AvatarFile.Length > 0)
                {
                    var nombreArchivo = $"avatar_{Guid.NewGuid():N}" + Path.GetExtension(u.AvatarFile.FileName);
                    var ruta = Path.Combine("wwwroot/img/avatares", nombreArchivo);
                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        u.AvatarFile.CopyTo(stream);
                    }
                    original.avatar = "/img/avatares/" + nombreArchivo;
                }

                _repositorio.ActualizarUsuarioEmpleado(original);
                TempData["Mensaje"] = "Perfil actualizado con éxito";
                TempData["Tipo"] = "success";
                return RedirectToAction("Perfil");
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al actualizar perfil: " + ex.Message;
                TempData["Tipo"] = "error";
                return View(u);
            }
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult EliminarConfirmado(int id)
        {
            _repositorio.EliminarUsuario(id);
            TempData["Mensaje"] = "Usuario eliminado correctamente";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }
    }
}
