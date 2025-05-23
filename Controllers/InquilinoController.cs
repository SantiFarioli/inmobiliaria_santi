using Microsoft.AspNetCore.Mvc;
using inmobiliaria_santi.Models;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria_santi.Controllers
{
    [Authorize]
    public class InquilinoController : Controller
    {
        private readonly RepositorioInquilino _repositorio;

        public InquilinoController(RepositorioInquilino repositorio)
        {
            _repositorio = repositorio;
        }

        // GET: Inquilino
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Index(string q)
        {
            var inquilinos = _repositorio.ObtenerTodos();
            if (!string.IsNullOrEmpty(q))
            {
                q = q.ToLower();
                inquilinos = inquilinos.Where(i =>
                    (i.nombre != null && i.nombre.ToLower().Contains(q)) ||
                    (i.apellido != null && i.apellido.ToLower().Contains(q)) ||
                    i.dni.Contains(q) ||
                    (i.email != null && i.email.ToLower().Contains(q))
                ).ToList();
            }
            return View(inquilinos);
        }

        // GET: Inquilino/Crear
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Inquilino/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear(Inquilino inquilino)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_repositorio.ExistePorDniOEmail(inquilino.dni, inquilino.email))
                    {
                        TempData["Mensaje"] = "Ya existe un inquilino con ese DNI o Email.";
                        TempData["Tipo"] = "warning";
                        return View(inquilino);
                    }

                    _repositorio.CrearInquilino(inquilino);
                    TempData["Mensaje"] = "¡Inquilino creado correctamente!";
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

            return View(inquilino);
        }


        // GET: Inquilino/Detalle/5
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Detalle(int id)
        {
            var inquilino = _repositorio.ObtenerPorId(id);
            if (inquilino == null)
            {
                return NotFound();
            }
            return View(inquilino);
        }

        // GET: Inquilino/Editar/5
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            var inquilino = _repositorio.ObtenerPorId(id);
            if (inquilino == null)
            {
                return NotFound();
            }
            return View(inquilino);
        }

        // POST: Inquilino/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(Inquilino inquilino)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repositorio.ActualizarInquilino(inquilino);
                    TempData["Mensaje"] = "¡Inquilino actualizado correctamente!";
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
            return View(inquilino);
        }

        // GET: Inquilino/EliminarConfirmado/5
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult EliminarConfirmado(int id)
        {
            try
            {
                _repositorio.EliminarInquilino(id);
                TempData["Mensaje"] = "¡Inquilino eliminado correctamente!";
                TempData["Tipo"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al eliminar: " + ex.Message;
                TempData["Tipo"] = "error";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Inquilino/ContratosActivos/5
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult ContratosActivos(int id)
        {
            var inquilino = _repositorio.ObtenerPorId(id);
            if (inquilino == null)
            {
                TempData["Mensaje"] = "Inquilino no encontrado.";
                TempData["Tipo"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            var contratos = new RepositorioContrato().ObtenerContratosActivosPorInquilino(id);

            ViewBag.InquilinoNombre = $"{inquilino.nombre} {inquilino.apellido}";
            return View(contratos);
        }

    }
}
