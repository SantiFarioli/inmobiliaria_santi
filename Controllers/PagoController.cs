using inmobiliaria_santi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace inmobiliaria_santi.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private readonly RepositorioPago _repositorioPago;
        private readonly RepositorioContrato _repositorioContrato;

        public PagoController(RepositorioPago repositorioPago, RepositorioContrato repositorioContrato)
        {
            _repositorioContrato = repositorioContrato;
            _repositorioPago = repositorioPago;
        }

        // GET: Pago
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Index(string q)
        {
            var pagos = _repositorioPago.ObtenerTodos();

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
        [HttpGet]
        public IActionResult Crear(int? idContrato)
        {
            // Trae solo el contrato correspondiente
            var contratos = idContrato.HasValue
                ? _repositorioContrato.ObtenerContratosConResumen(idContrato.Value)
                : _repositorioContrato.ObtenerContratosConResumen();
        
            if (contratos == null || !contratos.Any())
            {
                TempData["Mensaje"] = "⚠️ No hay contratos disponibles para registrar pagos.";
                TempData["Tipo"] = "warning";
                return RedirectToAction(nameof(Index));
            }
        
            ViewBag.idContratoFijo = idContrato.HasValue;
            ViewBag.Contratos = new SelectList(contratos, nameof(Contrato.idContrato), nameof(Contrato.ContratoResumen), idContrato);
        
            return View(new Pago
            {
                idContrato = idContrato ?? 0,
                estado = true,
                usuarioCreacion = User.Identity?.Name ?? "sistema"
            });
        }


        // POST: Pago/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear(Pago pago)
        {
            var contratos = _repositorioContrato.ObtenerContratosConResumen(pago.idContrato);
            ViewBag.Contratos = new SelectList(contratos, nameof(Contrato.idContrato), nameof(Contrato.ContratoResumen), pago.idContrato);
            ViewBag.idContratoFijo = true;

            try
            {
                // 💡 Setear nroPago ANTES de validar
                pago.nroPago = _repositorioPago.ObtenerUltimoNumeroPago(pago.idContrato) + 1;

                if (_repositorioPago.ExistePago(0, pago.idContrato, pago.nroPago))
                {
                    TempData["Mensaje"] = "Ya existe un pago con ese número para este contrato.";
                    TempData["Tipo"] = "warning";
                    return View(pago);
                }

                if (ModelState.IsValid)
                {
                    pago.usuarioCreacion = User.Identity?.Name ?? "sistema";
                    _repositorioPago.CrearPago(pago);
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
            var pago = _repositorioPago.ObtenerPorId(id);
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
                if (_repositorioPago.ExistePago(pago.idPago, pago.idContrato, pago.nroPago))
                {
                    TempData["Mensaje"] = "Ya existe un pago con ese número para este contrato.";
                    TempData["Tipo"] = "warning";
                    return View(pago);
                }

                if (ModelState.IsValid)
                {
                    _repositorioPago.ActualizarPago(pago);
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
            var pago = _repositorioPago.ObtenerPorId(id);
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
                _repositorioPago.EliminarPago(id, usuario);
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
        
        // GET: Pago/PorContrato/5
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult PorContrato(int idContrato)
        {
            var pagos = _repositorioPago.ObtenerPagosPorContrato(idContrato);
            ViewBag.Contrato = _repositorioContrato.ObtenerPorId(idContrato);
            return View("PorContrato", pagos); // asegúrate de crear la vista PorContrato.cshtml
        }

    }
}
