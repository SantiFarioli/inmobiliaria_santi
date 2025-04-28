using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace inmobiliaria_santi.Models
{
    public class RepositorioInquilino
    {
        private readonly string connectionString = "Server=localhost;Database=inmobiliaria_santi;Uid=root;Pwd=admin;";

        public List<Inquilino> ObtenerTodos()
        {
            List<Inquilino> inquilinos = new List<Inquilino>();
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"SELECT {nameof(Inquilino.idInquilino)}, {nameof(Inquilino.nombre)}, {nameof(Inquilino.apellido)},
                                    {nameof(Inquilino.dni)}, {nameof(Inquilino.telefono)}, {nameof(Inquilino.email)}, {nameof(Inquilino.estado)}
                             FROM inquilino
                             WHERE {nameof(Inquilino.estado)} = 1";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    conexion.Open();
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        inquilinos.Add(new Inquilino
                        {
                            idInquilino = reader.GetInt32(nameof(Inquilino.idInquilino)),
                            nombre = reader.GetString(nameof(Inquilino.nombre)),
                            apellido = reader.GetString(nameof(Inquilino.apellido)),
                            dni = reader.GetString(nameof(Inquilino.dni)),
                            telefono = reader.GetString(nameof(Inquilino.telefono)),
                            email = reader.GetString(nameof(Inquilino.email)),
                            estado = reader.GetBoolean(nameof(Inquilino.estado))
                        });
                    }
                }
            }
            return inquilinos;
        }

        public Inquilino? ObtenerPorId(int idInquilino)
        {
            Inquilino? inquilino = null;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"SELECT {nameof(Inquilino.idInquilino)}, {nameof(Inquilino.nombre)}, {nameof(Inquilino.apellido)},
                                    {nameof(Inquilino.dni)}, {nameof(Inquilino.telefono)}, {nameof(Inquilino.email)}, {nameof(Inquilino.estado)}
                             FROM inquilino
                             WHERE {nameof(Inquilino.idInquilino)} = @{nameof(idInquilino)} 
                             AND {nameof(Inquilino.estado)} = 1";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(idInquilino)}", idInquilino);
                    conexion.Open();
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        inquilino = new Inquilino
                        {
                            idInquilino = reader.GetInt32(nameof(Inquilino.idInquilino)),
                            nombre = reader.GetString(nameof(Inquilino.nombre)),
                            apellido = reader.GetString(nameof(Inquilino.apellido)),
                            dni = reader.GetString(nameof(Inquilino.dni)),
                            telefono = reader.GetString(nameof(Inquilino.telefono)),
                            email = reader.GetString(nameof(Inquilino.email)),
                            estado = reader.GetBoolean(nameof(Inquilino.estado))
                        };
                    }
                }
            }
            return inquilino;
        }

        public int CrearInquilino(Inquilino inquilino)
        {
            int id = 0;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"INSERT INTO inquilino
                             ({nameof(Inquilino.nombre)}, {nameof(Inquilino.apellido)}, {nameof(Inquilino.dni)}, 
                              {nameof(Inquilino.telefono)}, {nameof(Inquilino.email)}, {nameof(Inquilino.estado)})
                             VALUES
                             (@{nameof(Inquilino.nombre)}, @{nameof(Inquilino.apellido)}, @{nameof(Inquilino.dni)}, 
                              @{nameof(Inquilino.telefono)}, @{nameof(Inquilino.email)}, 1);
                             SELECT LAST_INSERT_ID();";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(Inquilino.nombre)}", inquilino.nombre);
                    comando.Parameters.AddWithValue($"@{nameof(Inquilino.apellido)}", inquilino.apellido);
                    comando.Parameters.AddWithValue($"@{nameof(Inquilino.dni)}", inquilino.dni);
                    comando.Parameters.AddWithValue($"@{nameof(Inquilino.telefono)}", inquilino.telefono);
                    comando.Parameters.AddWithValue($"@{nameof(Inquilino.email)}", inquilino.email);
                    conexion.Open();
                    id = Convert.ToInt32(comando.ExecuteScalar());
                    inquilino.idInquilino = id;
                }
            }
            return id;
        }

        public int ActualizarInquilino(Inquilino inquilino)
        {
            int filasAfectadas = 0;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"UPDATE inquilino
                             SET {nameof(Inquilino.nombre)} = @{nameof(Inquilino.nombre)},
                                 {nameof(Inquilino.apellido)} = @{nameof(Inquilino.apellido)},
                                 {nameof(Inquilino.dni)} = @{nameof(Inquilino.dni)},
                                 {nameof(Inquilino.telefono)} = @{nameof(Inquilino.telefono)},
                                 {nameof(Inquilino.email)} = @{nameof(Inquilino.email)}
                             WHERE {nameof(Inquilino.idInquilino)} = @{nameof(Inquilino.idInquilino)}";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(Inquilino.nombre)}", inquilino.nombre);
                    comando.Parameters.AddWithValue($"@{nameof(Inquilino.apellido)}", inquilino.apellido);
                    comando.Parameters.AddWithValue($"@{nameof(Inquilino.dni)}", inquilino.dni);
                    comando.Parameters.AddWithValue($"@{nameof(Inquilino.telefono)}", inquilino.telefono);
                    comando.Parameters.AddWithValue($"@{nameof(Inquilino.email)}", inquilino.email);
                    comando.Parameters.AddWithValue($"@{nameof(Inquilino.idInquilino)}", inquilino.idInquilino);
                    conexion.Open();
                    filasAfectadas = comando.ExecuteNonQuery();
                }
            }
            return filasAfectadas;
        }

        public int EliminarInquilino(int idInquilino)
        {
            int filasAfectadas = 0;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"UPDATE inquilino
                             SET {nameof(Inquilino.estado)} = 0
                             WHERE {nameof(Inquilino.idInquilino)} = @{nameof(idInquilino)}";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(idInquilino)}", idInquilino);
                    conexion.Open();
                    filasAfectadas = comando.ExecuteNonQuery();
                }
            }
            return filasAfectadas;
        }
    }
}
