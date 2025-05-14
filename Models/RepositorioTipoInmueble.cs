using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace inmobiliaria_santi.Models
{
    public class RepositorioTipoInmueble
    {
        private readonly string connectionString = "Server=localhost;Database=inmobiliaria_santi;Uid=root;Pwd=admin;";

        public List<TipoInmueble> ObtenerTodos()
        {
            List<TipoInmueble> tipos = new List<TipoInmueble>();
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"SELECT {nameof(TipoInmueble.idTipoInmueble)}, {nameof(TipoInmueble.nombre)}, {nameof(TipoInmueble.activo)}
                             FROM tipoinmueble
                             WHERE {nameof(TipoInmueble.activo)} = 1";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    conexion.Open();
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        tipos.Add(new TipoInmueble
                        {
                            idTipoInmueble = reader.GetInt32(nameof(TipoInmueble.idTipoInmueble)),
                            nombre = reader.GetString(nameof(TipoInmueble.nombre)),
                            activo = reader.GetBoolean(nameof(TipoInmueble.activo))
                        });
                    }
                }
            }
            return tipos;
        }

        public TipoInmueble? ObtenerPorId(int idTipoInmueble)
        {
            TipoInmueble? tipo = null;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"SELECT {nameof(TipoInmueble.idTipoInmueble)}, {nameof(TipoInmueble.nombre)}, {nameof(TipoInmueble.activo)}
                             FROM tipoinmueble
                             WHERE {nameof(TipoInmueble.idTipoInmueble)} = @{nameof(idTipoInmueble)}
                             AND {nameof(TipoInmueble.activo)} = 1";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(idTipoInmueble)}", idTipoInmueble);
                    conexion.Open();
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        tipo = new TipoInmueble
                        {
                            idTipoInmueble = reader.GetInt32(nameof(TipoInmueble.idTipoInmueble)),
                            nombre = reader.GetString(nameof(TipoInmueble.nombre)),
                            activo = reader.GetBoolean(nameof(TipoInmueble.activo))
                        };
                    }
                }
            }
            return tipo;
        }

        public int CrearTipoInmueble(TipoInmueble tipo)
        {
            int id = 0;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"INSERT INTO tipoinmueble
                             ({nameof(TipoInmueble.nombre)}, {nameof(TipoInmueble.activo)})
                             VALUES
                             (@{nameof(TipoInmueble.nombre)}, 1);
                             SELECT LAST_INSERT_ID();";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(TipoInmueble.nombre)}", tipo.nombre);
                    conexion.Open();
                    id = Convert.ToInt32(comando.ExecuteScalar());
                    tipo.idTipoInmueble = id;
                }
            }
            return id;
        }

        public bool ExistePorNombre(string nombre)
        {
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"SELECT COUNT(*) FROM tipoinmueble 
                            WHERE {nameof(TipoInmueble.nombre)} = @{nameof(nombre)} 
                            AND {nameof(TipoInmueble.activo)} = 1";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(nombre)}", nombre);
                    conexion.Open();
                    return Convert.ToInt32(comando.ExecuteScalar()) > 0;
                }
            }
        }


        public int ActualizarTipoInmueble(TipoInmueble tipo)
        {
            int filasAfectadas = 0;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"UPDATE tipoinmueble
                             SET {nameof(TipoInmueble.nombre)} = @{nameof(TipoInmueble.nombre)}
                             WHERE {nameof(TipoInmueble.idTipoInmueble)} = @{nameof(TipoInmueble.idTipoInmueble)}";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(TipoInmueble.nombre)}", tipo.nombre);
                    comando.Parameters.AddWithValue($"@{nameof(TipoInmueble.idTipoInmueble)}", tipo.idTipoInmueble);
                    conexion.Open();
                    filasAfectadas = comando.ExecuteNonQuery();
                }
            }
            return filasAfectadas;
        }

        public int EliminarTipoInmueble(int idTipoInmueble)
        {
            int filasAfectadas = 0;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"UPDATE tipoinmueble
                             SET {nameof(TipoInmueble.activo)} = 0
                             WHERE {nameof(TipoInmueble.idTipoInmueble)} = @{nameof(idTipoInmueble)}";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(idTipoInmueble)}", idTipoInmueble);
                    conexion.Open();
                    filasAfectadas = comando.ExecuteNonQuery();
                }
            }
            return filasAfectadas;
        }
    }
}
