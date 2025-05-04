namespace inmobiliaria_santi.Models;

using System.ComponentModel.DataAnnotations;

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
    public decimal montoRenta { get; set; }

    [DataType(DataType.Currency)]
    public decimal deposito { get; set; }

    [DataType(DataType.Currency)]
    public decimal comision { get; set; }
    public string? condiciones { get; set; }

    public string? usuarioCreacion { get; set; }
    public string? usuarioTerminacion { get; set; }

    public decimal? multaTerminacionTemprana { get; set; }
    public DateTime? fechaTerminacionTemprana { get; set; }
    public bool estado { get; set; }
    public decimal? porcentajeMulta { get; set; }

    // propiedades para mostrar datos
    public string? PropietarioNombre { get; set; }
    public string? PropietarioApellido { get; set; }
    public string? InmuebleDireccion { get; set; }
    public string? InquilinoNombre { get; set; }
    public string? InquilinoApellido { get; set; }
}