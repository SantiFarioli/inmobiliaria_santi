

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public IFormFile? AvatarFile { get; set; } // ← solo para formularios
    }

    public enum RolUsuario
    {
        Administrador = 1,
        Empleado = 2,
    }
}
