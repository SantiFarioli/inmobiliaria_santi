using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using inmobiliaria_santi.Models;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria_santi.Controllers
{
    [Authorize]
    public class InmuebleController : Controller
    {
        private readonly RepositorioInmueble _repositorio;
        private readonly RepositorioPropietario _repositorioPropietario;
        private readonly RepositorioTipoInmueble _repositorioTipoInmueble;

        public InmuebleController(RepositorioInmueble repositorio, 
                          RepositorioPropietario repositorioPropietario, 
                          RepositorioTipoInmueble repositorioTipoInmueble)
        {
            _repositorio = repositorio;
            _repositorioPropietario = repositorioPropietario;
            _repositorioTipoInmueble = repositorioTipoInmueble;
        }

        // GET: Inmueble
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Index(string q)
        {
            var inmuebles = _repositorio.ObtenerTodos();

            if (!string.IsNullOrEmpty(q))
            {
                q = q.ToLower();
                inmuebles = inmuebles.Where(i =>
                    (!string.IsNullOrEmpty(i.direccion) && i.direccion.ToLower().Contains(q)) ||
                    (!string.IsNullOrEmpty(i.uso) && i.uso.ToLower().Contains(q)) ||
                    (!string.IsNullOrEmpty(i.PropietarioNombre) && i.PropietarioNombre.ToLower().Contains(q)) ||
                    (!string.IsNullOrEmpty(i.PropietarioApellido) && i.PropietarioApellido.ToLower().Contains(q))
                ).ToList();
            }

            return View(inmuebles);
        }


        // GET: Inmueble/Crear
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear()
        {
            ViewBag.Propietarios = new SelectList(
                _repositorioPropietario.ObtenerTodos()
                .Select(p => new { 
                        p.idPropietario, 
                        NombreCompleto = p.nombre + " " + p.apellido 
                }), 
                "idPropietario", "NombreCompleto");

            ViewBag.Tipos = new SelectList(_repositorioTipoInmueble.ObtenerTodos(), "idTipoInmueble", "nombre");
            return View();
        }

        // POST: Inmueble/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear(Inmueble inmueble)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_repositorio.ExistePorDireccion(inmueble.direccion))
                    {
                        TempData["Mensaje"] = "Ya existe un inmueble registrado con esa dirección.";
                        TempData["Tipo"] = "warning";
                    }
                    else
                    {
                        _repositorio.CrearInmueble(inmueble);
                        TempData["Mensaje"] = "¡Inmueble creado correctamente!";
                        TempData["Tipo"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    TempData["Mensaje"] = "Error en la validación del formulario.";
                    TempData["Tipo"] = "warning";
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error inesperado: " + ex.Message;
                TempData["Tipo"] = "error";
            }

            ViewBag.Propietarios = new SelectList(_repositorioPropietario.ObtenerTodos(), "idPropietario", "apellido", inmueble.idPropietario);
            ViewBag.Tipos = new SelectList(_repositorioTipoInmueble.ObtenerTodos(), "idTipoInmueble", "nombre", inmueble.idTipoInmueble);
            return View(inmueble);
        }

        // GET: Inmueble/Detalle/5
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Detalle(int id)
        {
            var inmueble = _repositorio.ObtenerPorId(id);
            if (inmueble == null)
            {
                return NotFound();
            }
            return View(inmueble);
        }

        // GET: Inmueble/Editar/5
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            var inmueble = _repositorio.ObtenerPorId(id);
            if (inmueble == null)
            {
                return NotFound();
            }

            ViewBag.Propietarios = new SelectList(
                _repositorioPropietario.ObtenerTodos()
                .Select(p => new { 
                    p.idPropietario, 
                    NombreCompleto = p.nombre + " " + p.apellido 
                }), 
                "idPropietario", "NombreCompleto", inmueble.idPropietario);

            ViewBag.Tipos = new SelectList(_repositorioTipoInmueble.ObtenerTodos(), "idTipoInmueble", "nombre", inmueble.idTipoInmueble);
            return View(inmueble);
        }

        // POST: Inmueble/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(Inmueble inmueble)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repositorio.ActualizarInmueble(inmueble);
                    TempData["Mensaje"] = "¡Inmueble actualizado correctamente!";
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

            ViewBag.Propietarios = new SelectList(_repositorioPropietario.ObtenerTodos(), "idPropietario", "apellido", inmueble.idPropietario);
            ViewBag.Tipos = new SelectList(_repositorioTipoInmueble.ObtenerTodos(), "idTipoInmueble", "nombre", inmueble.idTipoInmueble);
            return View(inmueble);
        }

        // GET: Inmueble/EliminarConfirmado/5
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult EliminarConfirmado(int id)
        {
            try
            {
                _repositorio.EliminarInmueble(id);
                TempData["Mensaje"] = "¡Inmueble eliminado correctamente!";
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
