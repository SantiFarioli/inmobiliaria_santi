using inmobiliaria_santi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace inmobiliaria_santi.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private readonly RepositorioPago _repositorio;

        public PagoController(RepositorioPago repositorio)
        {
            _repositorio = repositorio;
        }

        // GET: Pago
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Index(string q)
        {
            var pagos = _repositorio.ObtenerTodos();

            if (!string.IsNullOrEmpty(q))
            {
                q = q.ToLower();
                pagos = pagos.Where(p =>
                    (p.ContratoResumen != null && p.ContratoResumen.ToLower().Contains(q)) ||
                    p.detalle?.ToLower().Contains(q) == true ||
                    p.importe.ToString().Contains(q) ||
                    p.fechaPago.ToString("dd/MM/yyyy").Contains(q)
                ).ToList();
            }

            return View(pagos);
        }

        // GET: Pago/Crear
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear()
        {
            var repoContrato = new RepositorioContrato();
            var contratos = repoContrato.ObtenerContratosConResumen();

            if (contratos == null || !contratos.Any())
            {
                TempData["Mensaje"] = "⚠️ No hay contratos disponibles para registrar pagos.";
                TempData["Tipo"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Contratos = new SelectList(contratos, nameof(Contrato.idContrato), nameof(Contrato.ContratoResumen));
            return View(new Pago());
        }

        // POST: Pago/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear(Pago pago)
        {
            var repoContrato = new RepositorioContrato();
            ViewBag.Contratos = new SelectList(repoContrato.ObtenerContratosConResumen(), nameof(Contrato.idContrato), nameof(Contrato.ContratoResumen)); 

            try
            {
                // 💡 Setear nroPago ANTES de validar
                pago.nroPago = _repositorio.ObtenerUltimoNumeroPago(pago.idContrato) + 1;

                if (_repositorio.ExistePago(0, pago.idContrato, pago.nroPago))
                {
                    TempData["Mensaje"] = "Ya existe un pago con ese número para este contrato.";
                    TempData["Tipo"] = "warning";
                    return View(pago); 
                }

                if (ModelState.IsValid)
                {
                    pago.usuarioCreacion = User.Identity?.Name ?? "sistema";
                    _repositorio.CrearPago(pago);
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
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            var pago = _repositorio.ObtenerPorId(id);
            if (pago == null)
                return NotFound();
            return View(pago);
        }

        // POST: Pago/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id, Pago pago)
        {
            try
            {
                if (_repositorio.ExistePago(pago.idPago, pago.idContrato, pago.nroPago))
                {
                    TempData["Mensaje"] = "Ya existe un pago con ese número para este contrato.";
                    TempData["Tipo"] = "warning";
                    return View(pago);
                }

                if (ModelState.IsValid)
                {
                    _repositorio.ActualizarPago(pago);
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
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Detalle(int id)
        {
            var pago = _repositorio.ObtenerPorId(id);
            if (pago == null)
                return NotFound();
            return View(pago);
        }

        // GET: Pago/EliminarConfirmado/5
        [Authorize(Roles = "Administrador")]
        public IActionResult EliminarConfirmado(int id)
        {
            try
            {
                var usuario = User.Identity?.Name ?? "sistema";
                _repositorio.EliminarPago(id, usuario);
                TempData["Mensaje"] = "Pago Anulado correctamente.";
                TempData["Tipo"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al Anular: " + ex.Message;
                TempData["Tipo"] = "error";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
