using Microsoft.AspNetCore.Mvc;
using INMOBILIARIA_SANTI.Models;

namespace INMOBILIARIA_SANTI.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly RepositorioPropietario repositorio = new RepositorioPropietario();

        // GET: Propietario
        public IActionResult Index()
        {
            var propietarios = repositorio.ObtenerTodos();
            return View(propietarios);
        }

        // GET: Propietario/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Propietario/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult Crear(Propietario propietario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.CrearPropietario(propietario);
                    TempData["Mensaje"] = "Propietario creado correctamente.";
                    TempData["Tipo"] = "success"; // success | error | warning | info
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
        public IActionResult Detalle(int id)
        {
            var propietario = repositorio.ObtenerPorId(id);
            if (propietario == null)
            {
                return NotFound();
            }
            return View(propietario);
        }

        // GET: Propietario/Editar/5
        public IActionResult Editar(int id)
        {
            var propietario = repositorio.ObtenerPorId(id);
            if (propietario == null)
            {
                return NotFound();
            }
            return View(propietario);
        }

        // POST: Propietario/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                repositorio.ActualizarPropietario(propietario);
                return RedirectToAction(nameof(Index));
            }
            return View(propietario);
        }

        // GET: Propietario/Eliminar/5
        public IActionResult Eliminar(int id)
        {
            var propietario = repositorio.ObtenerPorId(id);
            if (propietario == null)
            {
                return NotFound();
            }
            return View(propietario);
        }

        // POST: Propietario/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarConfirmado(int id)
        {
            repositorio.EliminarPropietario(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
