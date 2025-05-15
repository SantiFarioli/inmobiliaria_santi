using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_santi.Models
{
    public class Contrato
    {
        [Key]
        public int idContrato { get; set; }

        [Required]
        public int idInmueble { get; set; }

        [Required]
        public int idInquilino { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime fechaInicio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime fechaFin { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal? montoRenta { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "El depósito no puede ser negativo.")]
        public decimal? deposito { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "La comisión no puede ser negativa.")]
        public decimal? comision { get; set; }

        public string? condiciones { get; set; }

        public string? usuarioCreacion { get; set; }
        public string? usuarioRescision { get; set; }

        public decimal? multaTerminacionTemprana { get; set; }
        public DateTime? fechaTerminacionTemprana { get; set; }
        public bool estado { get; set; } = true;
        public decimal? porcentajeMulta { get; set; }

        // Propiedades extendidas para la vista
        public string? PropietarioNombre { get; set; }
        public string? PropietarioApellido { get; set; }
        public string? InmuebleDireccion { get; set; }
        public string? InquilinoNombre { get; set; }
        public string? InquilinoApellido { get; set; }
        public string? ContratoResumen { get; set; }
    }
}
