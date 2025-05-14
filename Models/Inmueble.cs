using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_santi.Models
{
    public class Inmueble
    {
        [Key]
        public int idInmueble { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un propietario.")]
        public int idPropietario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de inmueble.")]
        public int idTipoInmueble { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        public string direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El uso es obligatorio (Ej: comercial, personal).")]
        [StringLength(50)]
        public string uso { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe indicar la cantidad de ambientes.")]
        [Range(1, 20, ErrorMessage = "Debe ingresar entre 1 y 20 ambientes.")]
        public int cantAmbiente { get; set; }

        [Required(ErrorMessage = "El valor del inmueble es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Debe ingresar un valor mayor a 0.")]
        public decimal valor { get; set; }

        public bool disponible { get; set; }

        public bool estado { get; set; } = true;

        // Propiedades extendidas para la vista
        public string? PropietarioNombre { get; set; }
        public string? PropietarioApellido { get; set; }
        public string? PropietarioDni { get; set; }
        public string? TipoNombre { get; set; }
    }
}
