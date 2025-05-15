using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_santi.Models;

public class Pago
{
    [Key]
    public int idPago { get; set; }

    [Required(ErrorMessage = "El contrato es obligatorio.")]
    public int idContrato { get; set; }

    public int nroPago { get; set; }

    [Required(ErrorMessage = "La fecha de pago es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime fechaPago { get; set; }

    public string? detalle { get; set; }

    [Required(ErrorMessage = "El importe es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El importe debe ser mayor que cero.")]
    [DataType(DataType.Currency)]
    public decimal importe { get; set; }

    public bool estado { get; set; } = true;

    public string? usuarioCreacion { get; set; }
    public string? usuarioAnulacion { get; set; }
    public string? usuarioEliminacion { get; set; }

    // Vista
    public string? ContratoResumen { get; set; }
}
