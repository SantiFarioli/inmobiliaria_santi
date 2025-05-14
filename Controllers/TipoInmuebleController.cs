using Microsoft.AspNetCore.Mvc;
using inmobiliaria_santi.Models;

namespace inmobiliaria_santi.Controllers
{
    public class TipoInmuebleController : Controller
    {
        private readonly RepositorioTipoInmueble _repositorio;

        public TipoInmuebleController(RepositorioTipoInmueble repositorio)
        {
            _repositorio = repositorio;
        }

        // GET: TipoInmueble
        public IActionResult Index()
        {
            var tipos = _repositorio.ObtenerTodos();
            return View(tipos);
        }

        // GET: TipoInmueble/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: TipoInmueble/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(TipoInmueble tipo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_repositorio.ExistePorNombre(tipo.nombre))
                    {
                        TempData["Mensaje"] = "Ya existe un tipo de inmueble con ese nombre.";
                        TempData["Tipo"] = "warning";
                        return View(tipo);
                    }

                    _repositorio.CrearTipoInmueble(tipo);
                    TempData["Mensaje"] = "¡Tipo de inmueble creado correctamente!";
                    TempData["Tipo"] = "success";
                    return RedirectToAction("Crear", "Inmueble");
                }

                TempData["Mensaje"] = "Error en la validación del formulario.";
                TempData["Tipo"] = "warning";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error inesperado: " + ex.Message;
                TempData["Tipo"] = "error";
            }
            return View(tipo);
        }

        // GET: TipoInmueble/Editar/5
        public IActionResult Editar(int id)
        {
            var tipo = _repositorio.ObtenerPorId(id);
            if (tipo == null)
            {
                return NotFound();
            }
            return View(tipo);
        }

        // POST: TipoInmueble/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(TipoInmueble tipo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repositorio.ActualizarTipoInmueble(tipo);
                    TempData["Mensaje"] = "¡Tipo de inmueble actualizado correctamente!";
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
            return View(tipo);
        }

        // GET: TipoInmueble/EliminarConfirmado/5
        [HttpGet]
        public IActionResult EliminarConfirmado(int id)
        {
            try
            {
                _repositorio.EliminarTipoInmueble(id);
                TempData["Mensaje"] = "¡Tipo de inmueble eliminado correctamente!";
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
