using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_santi.Models
{
    public class TipoInmueble
    {
        [Key]
        public int idTipoInmueble { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de inmueble es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        public string nombre { get; set; } = string.Empty;

        public bool activo { get; set; } = true;
    }
}
