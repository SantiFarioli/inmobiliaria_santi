using inmobiliaria_santi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_santi.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private readonly RepositorioPago _repo;

        public PagoController()
        {
            _repo = new RepositorioPago();
        }

        // GET: Pago
        public IActionResult Index()
        {
            var pagos = _repo.ObtenerTodos();
            return View(pagos);
        }

        // GET: Pago/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Pago/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Pago pago)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    pago.usuarioCreacion = User.Identity?.Name ?? "sistema";
                    _repo.CrearPago(pago);
                    TempData["Mensaje"] = "Pago registrado correctamente.";
                    TempData["Tipo"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Mensaje"] = "Error de validación.";
                TempData["Tipo"] = "warning";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error inesperado: " + ex.Message;
                TempData["Tipo"] = "error";
            }
            return View(pago);
        }

        // GET: Pago/Editar/5
        public IActionResult Editar(int id)
        {
            var pago = _repo.ObtenerPorId(id);
            if (pago == null)
                return NotFound();
            return View(pago);
        }

        // POST: Pago/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, Pago pago)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repo.ActualizarPago(pago);
                    TempData["Mensaje"] = "Pago modificado correctamente.";
                    TempData["Tipo"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Mensaje"] = "Error en validación.";
                TempData["Tipo"] = "warning";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error inesperado: " + ex.Message;
                TempData["Tipo"] = "error";
            }
            return View(pago);
        }

        // GET: Pago/Detalle/5
        public IActionResult Detalle(int id)
        {
            var pago = _repo.ObtenerPorId(id);
            if (pago == null)
                return NotFound();
            return View(pago);
        }

        // GET: Pago/EliminarConfirmado/5
        public IActionResult EliminarConfirmado(int id)
        {
            try
            {
                var usuario = User.Identity?.Name ?? "sistema";
                _repo.EliminarPago(id, usuario);
                TempData["Mensaje"] = "Pago eliminado correctamente.";
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
