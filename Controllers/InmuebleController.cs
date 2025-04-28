using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using inmobiliaria_santi.Models;

namespace inmobiliaria_santi.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly RepositorioInmueble repositorio = new RepositorioInmueble();
        private readonly RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
        private readonly RepositorioTipoInmueble repositorioTipoInmueble = new RepositorioTipoInmueble();

        // GET: Inmueble
        public IActionResult Index()
        {
            var inmuebles = repositorio.ObtenerTodos();
            return View(inmuebles);
        }

        // GET: Inmueble/Crear
        public IActionResult Crear()
        {
            ViewBag.Propietarios = new SelectList(repositorioPropietario.ObtenerTodos(), "idPropietario", "apellido");
            ViewBag.Tipos = new SelectList(repositorioTipoInmueble.ObtenerTodos(), "idTipoInmueble", "nombre");
            return View();
        }

        // POST: Inmueble/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Inmueble inmueble)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.CrearInmueble(inmueble);
                    TempData["Mensaje"] = "¡Inmueble creado correctamente!";
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

            ViewBag.Propietarios = new SelectList(repositorioPropietario.ObtenerTodos(), "idPropietario", "apellido", inmueble.idPropietario);
            ViewBag.Tipos = new SelectList(repositorioTipoInmueble.ObtenerTodos(), "idTipoInmueble", "nombre", inmueble.idTipoInmueble);
            return View(inmueble);
        }

        // GET: Inmueble/Detalle/5
        public IActionResult Detalle(int id)
        {
            var inmueble = repositorio.ObtenerPorId(id);
            if (inmueble == null)
            {
                return NotFound();
            }
            return View(inmueble);
        }

        // GET: Inmueble/Editar/5
        public IActionResult Editar(int id)
        {
            var inmueble = repositorio.ObtenerPorId(id);
            if (inmueble == null)
            {
                return NotFound();
            }

            ViewBag.Propietarios = new SelectList(repositorioPropietario.ObtenerTodos(), "idPropietario", "apellido", inmueble.idPropietario);
            ViewBag.Tipos = new SelectList(repositorioTipoInmueble.ObtenerTodos(), "idTipoInmueble", "nombre", inmueble.idTipoInmueble);
            return View(inmueble);
        }

        // POST: Inmueble/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Inmueble inmueble)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.ActualizarInmueble(inmueble);
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

            ViewBag.Propietarios = new SelectList(repositorioPropietario.ObtenerTodos(), "idPropietario", "apellido", inmueble.idPropietario);
            ViewBag.Tipos = new SelectList(repositorioTipoInmueble.ObtenerTodos(), "idTipoInmueble", "nombre", inmueble.idTipoInmueble);
            return View(inmueble);
        }

        // GET: Inmueble/EliminarConfirmado/5
        [HttpGet]
        public IActionResult EliminarConfirmado(int id)
        {
            try
            {
                repositorio.EliminarInmueble(id);
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
