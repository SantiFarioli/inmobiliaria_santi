using inmobiliaria_santi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace inmobiliaria_santi.Controllers
{
    [Authorize]
    public class ContratoController : Controller
    {
        private readonly RepositorioContrato _repositorioContrato;
        private readonly RepositorioInquilino _repositorioInquilino;
        private readonly RepositorioInmueble _repositorioInmueble;

        public ContratoController(RepositorioContrato repositorioContrato,
                                  RepositorioInquilino repositorioInquilino,
                                  RepositorioInmueble repositorioInmueble)
        {
            _repositorioContrato = repositorioContrato;
            _repositorioInquilino = repositorioInquilino;
            _repositorioInmueble = repositorioInmueble;
        }

        // GET: Contrato
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Index(string q)
        {
            var contratos = _repositorioContrato.ObtenerTodos();

            if (!string.IsNullOrEmpty(q))
            {
                q = q.ToLower();
                contratos = contratos.Where(c =>
                    (!string.IsNullOrEmpty(c.InquilinoNombre) && c.InquilinoNombre.ToLower().Contains(q)) ||
                    (!string.IsNullOrEmpty(c.InquilinoApellido) && c.InquilinoApellido.ToLower().Contains(q)) ||
                    (!string.IsNullOrEmpty(c.PropietarioNombre) && c.PropietarioNombre.ToLower().Contains(q)) ||
                    (!string.IsNullOrEmpty(c.PropietarioApellido) && c.PropietarioApellido.ToLower().Contains(q)) ||
                    (!string.IsNullOrEmpty(c.InmuebleDireccion) && c.InmuebleDireccion.ToLower().Contains(q))
                ).ToList();
            }

            ViewBag.HayDisponibles = _repositorioInmueble.ObtenerDisponibles().Any();
            return View(contratos);
        }

        // GET: Contrato/Crear
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear(int? idInmueble)
        {
            ViewBag.Inquilinos = new SelectList(
                _repositorioInquilino.ObtenerTodos().Select(i => new
                {
                    i.idInquilino,
                    NombreCompleto = i.nombre + " " + i.apellido + " - DNI: " + i.dni
                }),
                "idInquilino", "NombreCompleto");

            ViewBag.Inmuebles = new SelectList(
                _repositorioInmueble.ObtenerTodos().Select(i => new
                {
                    i.idInmueble,
                    Direccion = i.direccion + " - " + i.uso
                }),
                "idInmueble", "Direccion",
                idInmueble // ← Selecciona el inmueble si viene por parámetro
            );

            return View(new Contrato
            {
                idInmueble = idInmueble ?? 0,
                estado = true
            });
        }

        // POST: Contrato/Crear

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear(Contrato contrato)
        {
            try
            {
                if (_repositorioContrato.ExisteContratoActivoEnFechas(
                    contrato.idInmueble, contrato.fechaInicio, contrato.fechaFin))
                {
                    TempData["Mensaje"] = "⚠️ El inmueble ya tiene un contrato activo en ese período.";
                    TempData["Tipo"] = "warning";

                    // Recargar dropdowns
                    ViewBag.Inquilinos = new SelectList(
                        _repositorioInquilino.ObtenerTodos().Select(i => new
                        {
                            i.idInquilino,
                            NombreCompleto = i.nombre + " " + i.apellido + " - DNI: " + i.dni
                        }),
                        "idInquilino", "NombreCompleto", contrato.idInquilino);

                    ViewBag.Inmuebles = new SelectList(
                        _repositorioInmueble.ObtenerTodos().Select(i => new
                        {
                            i.idInmueble,
                            Direccion = i.direccion + " - " + i.uso
                        }),
                        "idInmueble", "Direccion", contrato.idInmueble);

                    return View(contrato);
                }

                if (ModelState.IsValid)
                {
                    contrato.estado = true;
                    contrato.usuarioCreacion = User.Identity?.Name ?? "sistema";
                    _repositorioContrato.CrearContrato(contrato);
                    TempData["Mensaje"] = "✅ Contrato creado correctamente.";
                    TempData["Tipo"] = "success";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Mensaje"] = "❌ Error en la validación del formulario.";
                TempData["Tipo"] = "warning";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "🚨 Error inesperado: " + ex.Message;
                TempData["Tipo"] = "error";
            }

            // Recargar dropdowns si hubo error
            ViewBag.Inquilinos = new SelectList(
                _repositorioInquilino.ObtenerTodos().Select(i => new
                {
                    i.idInquilino,
                    NombreCompleto = i.nombre + " " + i.apellido + " - DNI: " + i.dni
                }),
                "idInquilino", "NombreCompleto", contrato.idInquilino);

            ViewBag.Inmuebles = new SelectList(
                _repositorioInmueble.ObtenerTodos().Select(i => new
                {
                    i.idInmueble,
                    Direccion = i.direccion + " - " + i.uso
                }),
                "idInmueble", "Direccion", contrato.idInmueble);

            return View(contrato);
        }


        // GET: Contrato/Editar/5
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            var contrato = _repositorioContrato.ObtenerPorId(id);
            if (contrato == null)
            {
                return NotFound();
            }

            // Mostrar inquilinos con su nombre completo y DNI
            ViewBag.Inquilinos = new SelectList(
                _repositorioInquilino.ObtenerTodos()
                .Select(i => new
                {
                    i.idInquilino,
                    NombreCompleto = i.nombre + " " + i.apellido + " - DNI: " + i.dni
                }),
                "idInquilino", "NombreCompleto", contrato.idInquilino);

            // Mostrar inmuebles con su dirección
            ViewBag.Inmuebles = new SelectList(
                _repositorioInmueble.ObtenerTodos()
                .Select(i => new
                {
                    i.idInmueble,
                    Direccion = i.direccion + " - " + i.uso
                }),
                "idInmueble", "Direccion", contrato.idInmueble);

            return View(contrato);
        }

        // POST: Contrato/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(Contrato contrato)
        {
            try
            {
                if (_repositorioContrato.ExisteContratoActivoEnFechas(
                    contrato.idInmueble, contrato.fechaInicio, contrato.fechaFin))
                {
                    var contratoExistente = _repositorioContrato.ObtenerPorId(contrato.idContrato);

                    if (contratoExistente != null &&
                        (contratoExistente.idInmueble != contrato.idInmueble ||
                        contratoExistente.fechaInicio != contrato.fechaInicio ||
                        contratoExistente.fechaFin != contrato.fechaFin))
                    {
                        TempData["Mensaje"] = "⚠️ Ya existe otro contrato activo en ese período para este inmueble.";
                        TempData["Tipo"] = "warning";

                        // Recargar dropdowns
                        ViewBag.Inquilinos = new SelectList(
                            _repositorioInquilino.ObtenerTodos().Select(i => new
                            {
                                i.idInquilino,
                                NombreCompleto = i.nombre + " " + i.apellido + " - DNI: " + i.dni
                            }),
                            "idInquilino", "NombreCompleto", contrato.idInquilino);

                        ViewBag.Inmuebles = new SelectList(
                            _repositorioInmueble.ObtenerTodos().Select(i => new
                            {
                                i.idInmueble,
                                Direccion = i.direccion + " - " + i.uso
                            }),
                            "idInmueble", "Direccion", contrato.idInmueble);

                        return View(contrato);
                    }
                }

                if (ModelState.IsValid)
                {
                    _repositorioContrato.ActualizarContrato(contrato);
                    TempData["Mensaje"] = "✅ Contrato actualizado correctamente.";
                    TempData["Tipo"] = "success";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Mensaje"] = "❌ Error en la validación del formulario.";
                TempData["Tipo"] = "warning";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "🚨 Error inesperado: " + ex.Message;
                TempData["Tipo"] = "error";
            }

            // Recargar dropdowns en caso de error
            ViewBag.Inquilinos = new SelectList(
                _repositorioInquilino.ObtenerTodos().Select(i => new
                {
                    i.idInquilino,
                    NombreCompleto = i.nombre + " " + i.apellido + " - DNI: " + i.dni
                }),
                "idInquilino", "NombreCompleto", contrato.idInquilino);

            ViewBag.Inmuebles = new SelectList(
                _repositorioInmueble.ObtenerTodos().Select(i => new
                {
                    i.idInmueble,
                    Direccion = i.direccion + " - " + i.uso
                }),
                "idInmueble", "Direccion", contrato.idInmueble);

            return View(contrato);
        }


        // GET: Contrato/Detalle/5
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Detalle(int id)
        {
            var contrato = _repositorioContrato.ObtenerPorId(id);
            if (contrato == null)
            {
                return NotFound();
            }

            ViewBag.PuedeRescindir = contrato.estado == true;
            return View(contrato);  // Devuelve la vista con el contrato encontrado
        }


        // GET: Contrato/EliminarConfirmado/5
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult EliminarConfirmado(int id)
        {
            try
            {
                _repositorioContrato.EliminarContrato(id);
                TempData["Mensaje"] = "Contrato eliminado correctamente";
                TempData["Tipo"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al eliminar contrato: " + ex.Message;
                TempData["Tipo"] = "error";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Contrato/RescindirContrato/5
        [Authorize(Roles = "Administrador")]
        public IActionResult RescindirContrato(int id)
        {
            var contrato = _repositorioContrato.ObtenerPorId(id);
            if (contrato == null)
            {
                return NotFound();
            }

            // Calcular la multa según el tiempo restante en el contrato
            var fechaFin = contrato.fechaFin;
            var hoy = DateTime.Now;
            int mesesRestantes = ((fechaFin.Year - hoy.Year) * 12) + fechaFin.Month - hoy.Month;

            // Calcular el porcentaje de multa según el tiempo restante
            decimal porcentajeMulta = mesesRestantes < 6 ? 0.10m : 0.15m;
            decimal multa = (contrato.montoRenta ?? 0) * porcentajeMulta;

            // Preparar el contrato para ser actualizado
            contrato.multaTerminacionTemprana = multa;
            contrato.fechaTerminacionTemprana = hoy;  // Fecha de rescisión
            contrato.porcentajeMulta = porcentajeMulta;

            // Mostrar la vista de rescisión con la multa calculada
            return View(contrato);
        }

        // POST: Contrato/RescindirContrato/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult RescindirContrato(Contrato contrato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var contratoExistente = _repositorioContrato.ObtenerPorId(contrato.idContrato);
                    if (contratoExistente == null)
                    {
                        return NotFound();
                    }

                    // Marcar el contrato como rescindido en lugar de eliminarlo
                    contratoExistente.estado = false; // Suponiendo que el estado 'false' significa rescindido
                    contratoExistente.fechaTerminacionTemprana = DateTime.Now; // Fecha de rescisión
                    contratoExistente.multaTerminacionTemprana = contrato.multaTerminacionTemprana; // Multa calculada
                    contratoExistente.porcentajeMulta = contrato.porcentajeMulta; // Porcentaje de multa calculado
                    contratoExistente.usuarioRescision = User.Identity?.Name ?? "sistema";

                    _repositorioContrato.ActualizarContrato(contratoExistente);

                    TempData["Mensaje"] = "Contrato rescindido correctamente con la multa aplicada";
                    TempData["Tipo"] = "success";

                    return RedirectToAction(nameof(Index));
                }

                TempData["Mensaje"] = "Error al rescindir el contrato.";
                TempData["Tipo"] = "warning";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error inesperado: " + ex.Message;
                TempData["Tipo"] = "error";
            }

            return View(contrato);
        }

        // GET: Contrato/ActivosPorInquilino/5
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult ActivosPorInquilino(int idInquilino)
        {
            var contratos = _repositorioContrato.ObtenerContratosActivosPorInquilino(idInquilino);
            if (!contratos.Any())
            {
                TempData["Mensaje"] = "Este inquilino no tiene contratos activos.";
                TempData["Tipo"] = "info";
            }

            ViewBag.Inquilino = _repositorioInquilino.ObtenerPorId(idInquilino);
            return View("ContratosActivosPorInquilino", contratos); // nombre de la vista .cshtml
        }

        // GET: Contrato/PorVencer
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult PorVencer()
        {
            try
            {
                var contratos = _repositorioContrato.ObtenerContratosPorVencer();
                if (!contratos.Any())
                {
                    TempData["Mensaje"] = "No hay contratos próximos a vencer.";
                    TempData["Tipo"] = "info";
                }
                return View(contratos);
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al cargar los contratos por vencer: " + ex.Message;
                TempData["Tipo"] = "error";
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Rescindidos()
        {
            var rescindidos = _repositorioContrato.ObtenerRescindidos();
            return View(rescindidos);
        }

    }
}
