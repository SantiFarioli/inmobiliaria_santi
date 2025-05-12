using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliaria_santi.Models
{
    public class Inmueble
    {
        public int idInmueble { get; set; }
        public int idPropietario { get; set; }
        public int idTipoInmueble { get; set; }
        public string? direccion { get; set; }
        public string? uso { get; set; }
        public int cantAmbiente { get; set; }
        public decimal valor { get; set; }
        public bool disponible { get; set; }
        public bool estado { get; set; }

        // propiedades extendidas para mostrar en la vista
        public string? PropietarioNombre { get; set; }
        public string? PropietarioApellido { get; set; }
        public string? PropietarioDni { get; set; }
        public string? TipoNombre { get; set; } // si querés mostrar el tipo (Departamento, etc.)

    }
}