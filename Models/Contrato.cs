

namespace inmobiliaria_santi.Models
{
    public class Contrato
    {
        public int idContrato { get; set; }
        public int idInmueble { get; set; }
        public int idInquilino { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public decimal montoRenta { get; set; }
        public string? deposito { get; set; }
        public string? comision { get; set; }
        public string? condiciones { get; set; }
        public decimal? multaTerminacionTemprana { get; set; }
        public DateTime? fechaTerminacionTemprana { get; set; }
        public string? usuarioCreacion { get; set; }
        public string? usuarioTerminacion { get; set; }
        public bool estado { get; set; } // Activo o eliminado lógicamente
        public decimal? porcentajeMulta { get; set; }
    }
}