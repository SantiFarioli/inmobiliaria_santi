using Microsoft.AspNetCore.Mvc;
using inmobiliaria_santi.Models;

namespace inmobiliaria_santi.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly RepositorioInquilino _repositorio;

        public InquilinoController(RepositorioInquilino repositorio)
        {
            _repositorio = repositorio;
        }

        // GET: Inquilino
        public IActionResult Index()
        {
            var inquilinos = _repositorio.ObtenerTodos();
            return View(inquilinos);
        }

        // GET: Inquilino/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Inquilino/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Inquilino inquilino)
        {
            try
            {
                if (ModelState.IsValid)
                {
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
    }
}
