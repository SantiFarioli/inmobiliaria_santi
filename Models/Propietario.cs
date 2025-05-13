using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_santi.Models
{
    public class Propietario
    {
        [Key]
        public int idPropietario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        public string nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        public string apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [Range(1000000, 99999999, ErrorMessage = "Debe ser un DNI válido.")]
        public int dni { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Phone(ErrorMessage = "Número de teléfono inválido.")]
        public string telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Dirección de email inválida.")]
        public string email { get; set; } = string.Empty;

        public bool estado { get; set; } = true;
    }
}
