

namespace inmobiliaria_santi.Models
{
    public class Pago
    {
        public int idPago { get; set; }
        public int idContrato { get; set; }
        public int nroPago { get; set; }
        public DateTime fechaPago { get; set; }
        public string? detalle { get; set; }
        public decimal importe { get; set; }
        public bool estado { get; set; }
        public string? usuarioCreacion { get; set; }
        public string? usuarioAnulacion { get; set; }
        public string? usuarioEliminacion { get; set; }
    }
}