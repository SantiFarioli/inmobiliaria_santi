using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliaria_santi.Models
{
    public class Inquilino
    {
        public int idInquilino { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public string? dni { get; set; }
        public string? telefono { get; set; }
        public string? email { get; set; }
        public bool estado { get; set; }
    }
}