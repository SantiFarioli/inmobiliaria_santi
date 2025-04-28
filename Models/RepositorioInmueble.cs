using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace inmobiliaria_santi.Models
{
    public class RepositorioInmueble
    {
        private readonly string connectionString = "Server=localhost;Database=inmobiliaria_santi;Uid=root;Pwd=admin;";

        public List<Inmueble> ObtenerTodos()
        {
            List<Inmueble> inmuebles = new List<Inmueble>();
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"SELECT {nameof(Inmueble.idInmueble)}, {nameof(Inmueble.idPropietario)}, {nameof(Inmueble.idTipoInmueble)},
                                    {nameof(Inmueble.direccion)}, {nameof(Inmueble.uso)}, {nameof(Inmueble.cantAmbiente)}, 
                                    {nameof(Inmueble.valor)}, {nameof(Inmueble.disponible)}, {nameof(Inmueble.estado)}
                             FROM inmueble
                             WHERE {nameof(Inmueble.estado)} = 1";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    conexion.Open();
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        inmuebles.Add(new Inmueble
                        {
                            idInmueble = reader.GetInt32(nameof(Inmueble.idInmueble)),
                            idPropietario = reader.GetInt32(nameof(Inmueble.idPropietario)),
                            idTipoInmueble = reader.GetInt32(nameof(Inmueble.idTipoInmueble)),
                            direccion = reader.GetString(nameof(Inmueble.direccion)),
                            uso = reader.GetString(nameof(Inmueble.uso)),
                            cantAmbiente = reader.GetInt32(nameof(Inmueble.cantAmbiente)),
                            valor = reader.GetDecimal(nameof(Inmueble.valor)),
                            disponible = reader.GetBoolean(nameof(Inmueble.disponible)),
                            estado = reader.GetBoolean(nameof(Inmueble.estado))
                        });
                    }
                }
            }
            return inmuebles;
        }

        public Inmueble? ObtenerPorId(int idInmueble)
        {
            Inmueble? inmueble = null;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"SELECT {nameof(Inmueble.idInmueble)}, {nameof(Inmueble.idPropietario)}, {nameof(Inmueble.idTipoInmueble)},
                                    {nameof(Inmueble.direccion)}, {nameof(Inmueble.uso)}, {nameof(Inmueble.cantAmbiente)}, 
                                    {nameof(Inmueble.valor)}, {nameof(Inmueble.disponible)}, {nameof(Inmueble.estado)}
                             FROM inmueble
                             WHERE {nameof(Inmueble.idInmueble)} = @{nameof(idInmueble)} 
                             AND {nameof(Inmueble.estado)} = 1";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(idInmueble)}", idInmueble);
                    conexion.Open();
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        inmueble = new Inmueble
                        {
                            idInmueble = reader.GetInt32(nameof(Inmueble.idInmueble)),
                            idPropietario = reader.GetInt32(nameof(Inmueble.idPropietario)),
                            idTipoInmueble = reader.GetInt32(nameof(Inmueble.idTipoInmueble)),
                            direccion = reader.GetString(nameof(Inmueble.direccion)),
                            uso = reader.GetString(nameof(Inmueble.uso)),
                            cantAmbiente = reader.GetInt32(nameof(Inmueble.cantAmbiente)),
                            valor = reader.GetDecimal(nameof(Inmueble.valor)),
                            disponible = reader.GetBoolean(nameof(Inmueble.disponible)),
                            estado = reader.GetBoolean(nameof(Inmueble.estado))
                        };
                    }
                }
            }
            return inmueble;
        }

        public int CrearInmueble(Inmueble inmueble)
        {
            int id = 0;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"INSERT INTO inmueble
                             ({nameof(Inmueble.idPropietario)}, {nameof(Inmueble.idTipoInmueble)}, {nameof(Inmueble.direccion)},
                              {nameof(Inmueble.uso)}, {nameof(Inmueble.cantAmbiente)}, {nameof(Inmueble.valor)},
                              {nameof(Inmueble.disponible)}, {nameof(Inmueble.estado)})
                             VALUES
                             (@{nameof(Inmueble.idPropietario)}, @{nameof(Inmueble.idTipoInmueble)}, @{nameof(Inmueble.direccion)},
                              @{nameof(Inmueble.uso)}, @{nameof(Inmueble.cantAmbiente)}, @{nameof(Inmueble.valor)},
                              @{nameof(Inmueble.disponible)}, 1);
                             SELECT LAST_INSERT_ID();";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(Inmueble.idPropietario)}", inmueble.idPropietario);
                    comando.Parameters.AddWithValue($"@{nameof(Inmueble.idTipoInmueble)}", inmueble.idTipoInmueble);
                    comando.Parameters.AddWithValue($"@{nameof(Inmueble.direccion)}", inmueble.direccion);
                    comando.Parameters.AddWithValue($"@{nameof(Inmueble.uso)}", inmueble.uso);
                    comando.Parameters.AddWithValue($"@{nameof(Inmueble.cantAmbiente)}", inmueble.cantAmbiente);
                    comando.Parameters.AddWithValue($"@{nameof(Inmueble.valor)}", inmueble.valor);
                    comando.Parameters.AddWithValue($"@{nameof(Inmueble.disponible)}", inmueble.disponible);
                    conexion.Open();
                    id = Convert.ToInt32(comando.ExecuteScalar());
                    inmueble.idInmueble = id;
                }
            }
            return id;
        }

        public int ActualizarInmueble(Inmueble inmueble)
        {
            int filasAfectadas = 0;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"UPDATE inmueble
                             SET {nameof(Inmueble.idPropietario)} = @{nameof(Inmueble.idPropietario)},
                                 {nameof(Inmueble.idTipoInmueble)} = @{nameof(Inmueble.idTipoInmueble)},
                                 {nameof(Inmueble.direccion)} = @{nameof(Inmueble.direccion)},
                                 {nameof(Inmueble.uso)} = @{nameof(Inmueble.uso)},
                                 {nameof(Inmueble.cantAmbiente)} = @{nameof(Inmueble.cantAmbiente)},
                                 {nameof(Inmueble.valor)} = @{nameof(Inmueble.valor)},
                                 {nameof(Inmueble.disponible)} = @{nameof(Inmueble.disponible)}
                             WHERE {nameof(Inmueble.idInmueble)} = @{nameof(Inmueble.idInmueble)}";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(Inmueble.idPropietario)}", inmueble.idPropietario);
                    comando.Parameters.AddWithValue($"@{nameof(Inmueble.idTipoInmueble)}", inmueble.idTipoInmueble);
                    comando.Parameters.AddWithValue($"@{nameof(Inmueble.direccion)}", inmueble.direccion);
                    comando.Parameters.AddWithValue($"@{nameof(Inmueble.uso)}", inmueble.uso);
                    comando.Parameters.AddWithValue($"@{nameof(Inmueble.cantAmbiente)}", inmueble.cantAmbiente);
                    comando.Parameters.AddWithValue($"@{nameof(Inmueble.valor)}", inmueble.valor);
                    comando.Parameters.AddWithValue($"@{nameof(Inmueble.disponible)}", inmueble.disponible);
                    comando.Parameters.AddWithValue($"@{nameof(Inmueble.idInmueble)}", inmueble.idInmueble);
                    conexion.Open();
                    filasAfectadas = comando.ExecuteNonQuery();
                }
            }
            return filasAfectadas;
        }

        public int EliminarInmueble(int idInmueble)
        {
            int filasAfectadas = 0;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"UPDATE inmueble
                             SET {nameof(Inmueble.estado)} = 0
                             WHERE {nameof(Inmueble.idInmueble)} = @{nameof(idInmueble)}";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(idInmueble)}", idInmueble);
                    conexion.Open();
                    filasAfectadas = comando.ExecuteNonQuery();
                }
            }
            return filasAfectadas;
        }
    }
}
