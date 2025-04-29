using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;

namespace inmobiliaria_santi.Models
{
    public class Conexion(IConfiguration configuration)
    {
        private readonly string? _cadenaConexion = configuration.GetConnectionString("DefaultConnection");

        public void AbrirConexion()
        {
            using (var conexion = new MySqlConnection(_cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    Console.WriteLine("Conexión abierta.");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error al abrir la conexión: " + ex.Message);
                }
            }
        }

        public void CerrarConexion()
        {
            using (var conexion = new MySqlConnection(_cadenaConexion))
            {
                try
                {
                    conexion.Close();
                    Console.WriteLine("Conexión cerrada.");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error al cerrar la conexión: " + ex.Message);
                }
            }
        }
    }
}
