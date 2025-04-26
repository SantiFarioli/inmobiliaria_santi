

namespace inmobiliaria_santi.Models
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public string? email { get; set; }
        public string? contrasena { get; set; }
        public string? avatar { get; set; }
        public int rol { get; set; }
        public bool estado { get; set; }
    }
}