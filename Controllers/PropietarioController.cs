using Microsoft.AspNetCore.Mvc;
using inmobiliaria_santi.Models;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria_santi.Controllers
{
    [Authorize]
    public class PropietarioController : Controller
    {
        private readonly RepositorioPropietario _repositorio;

        public PropietarioController(RepositorioPropietario repositorio)
        {
            _repositorio = repositorio;
        }
       

        // GET: Propietario
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Index(string q)
        {
            var propietarios = _repositorio.ObtenerTodos();

            if (!string.IsNullOrEmpty(q))
            {
                q = q.ToLower();
                propietarios = propietarios.Where(p =>
                    (p.nombre != null && p.nombre.ToLower().Contains(q)) ||
                    (p.apellido != null && p.apellido.ToLower().Contains(q)) ||
                    p.dni.ToString().Contains(q) ||
                    (p.email != null && p.email.ToLower().Contains(q))
                ).ToList();
            }
            return View(propietarios);
        }

        // GET: Propietario/Crear
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Propietario/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear(Propietario propietario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Validación de duplicados por DNI o Email
                    if (_repositorio.ExistePorDniOEmail(propietario.dni, propietario.email))
                    {
                        TempData["Mensaje"] = "Ya existe un propietario con ese DNI o Email.";
                        TempData["Tipo"] = "warning";
                        return View(propietario);
                    }

                    _repositorio.CrearPropietario(propietario);
                    TempData["Mensaje"] = "Propietario creado correctamente.";
                    TempData["Tipo"] = "success";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Mensaje"] = "Error en la validación del formulario.";
                TempData["Tipo"] = "warning";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error inesperado: " + ex.Message;
                TempData["Tipo"] = "error";
            }

            return View(propietario);
        }


        // GET: Propietario/Detalle/5
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Detalle(int id)
        {
            var propietario = _repositorio.ObtenerPorId(id);
            if (propietario == null)
            {
                return NotFound();
            }
            return View(propietario);
        }

        // GET: Propietario/Editar/5
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            var propietario = _repositorio.ObtenerPorId(id);
            if (propietario == null)
            {
                return NotFound();
            }
            return View(propietario);
        }

        // POST: Propietario/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id, Propietario propietario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repositorio.ActualizarPropietario(propietario);
                    TempData["Mensaje"] = "Propietario editado correctamente.";
                    TempData["Tipo"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Mensaje"] = "Error en la validación del formulario.";
                TempData["Tipo"] = "warning";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error inesperado: " + ex.Message;
                TempData["Tipo"] = "error";
            }
            return View(propietario);
        }  

        // GET: Propietario/Eliminar/5
        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            var propietario = _repositorio.ObtenerPorId(id);
            if (propietario == null)
            {
                return NotFound();
            }
            return View(propietario);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult EliminarConfirmado(int id)
        {
        try
            {
                _repositorio.EliminarPropietario(id);
                TempData["Mensaje"] = "¡Propietario eliminado correctamente!";
                TempData["Tipo"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al eliminar: " + ex.Message;
                TempData["Tipo"] = "error";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
