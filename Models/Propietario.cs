namespace inmobiliaria_santi.Models
{
    public class Propietario
    {
        public int idPropietario { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public int dni { get; set; }
        public string? telefono { get; set; }
        public string? email { get; set; }
        public bool estado { get; set; }
    }
}
