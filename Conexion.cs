using MySql.Data.MySqlClient;
using System;

namespace INMOBILIARIA_SANTI.Models
{
    public class Conexion
    {
        private string cadenaConexion = "Server=localhost;Database=inmobiliaria_santi;Uid=root;Pwd=admin;";
        
        public MySqlConnection ObtenerConexion()
        {
            MySqlConnection conexion = new MySqlConnection(cadenaConexion);
            try
            {
                conexion.Open();
                Console.WriteLine("Conexión abierta.");
                return conexion;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al abrir la conexión: " + ex.Message);
                throw;
            }
        }
    }
}
