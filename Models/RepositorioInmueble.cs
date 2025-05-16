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
                var sql = $@"
                    SELECT i.{nameof(Inmueble.idInmueble)}, i.{nameof(Inmueble.idPropietario)}, i.{nameof(Inmueble.idTipoInmueble)},
                        i.{nameof(Inmueble.direccion)}, i.{nameof(Inmueble.uso)}, i.{nameof(Inmueble.cantAmbiente)},
                        i.{nameof(Inmueble.valor)}, i.{nameof(Inmueble.disponible)}, i.{nameof(Inmueble.estado)},
                        p.nombre AS PropietarioNombre, p.apellido AS PropietarioApellido, p.dni AS PropietarioDni,
                        ti.nombre AS TipoNombre
                    FROM inmueble i
                    INNER JOIN propietario p ON i.{nameof(Inmueble.idPropietario)} = p.{nameof(Propietario.idPropietario)}
                    INNER JOIN tipoinmueble ti ON i.{nameof(Inmueble.idTipoInmueble)} = ti.{nameof(TipoInmueble.idTipoInmueble)}
                    WHERE i.{nameof(Inmueble.estado)} = 1";

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
                            direccion = reader[nameof(Inmueble.direccion)]?.ToString() ?? string.Empty,
                            uso = reader[nameof(Inmueble.uso)]?.ToString() ?? string.Empty,
                            cantAmbiente = reader.GetInt32(nameof(Inmueble.cantAmbiente)),
                            valor = reader.GetDecimal(nameof(Inmueble.valor)),
                            disponible = reader.GetBoolean(nameof(Inmueble.disponible)),
                            estado = reader.GetBoolean(nameof(Inmueble.estado)),
                            PropietarioNombre = reader["PropietarioNombre"].ToString(),
                            PropietarioApellido = reader["PropietarioApellido"].ToString(),
                            PropietarioDni = reader["PropietarioDni"].ToString(),
                            TipoNombre = reader["TipoNombre"].ToString()
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
                var sql = $@"
                    SELECT i.{nameof(Inmueble.idInmueble)}, i.{nameof(Inmueble.idPropietario)}, i.{nameof(Inmueble.idTipoInmueble)},
                        i.{nameof(Inmueble.direccion)}, i.{nameof(Inmueble.uso)}, i.{nameof(Inmueble.cantAmbiente)},
                        i.{nameof(Inmueble.valor)}, i.{nameof(Inmueble.disponible)}, i.{nameof(Inmueble.estado)},
                        p.nombre AS PropietarioNombre, p.apellido AS PropietarioApellido, p.dni AS PropietarioDni,
                        ti.nombre AS TipoNombre
                    FROM inmueble i
                    INNER JOIN propietario p ON i.idPropietario = p.idPropietario
                    INNER JOIN tipoinmueble ti ON i.idTipoInmueble = ti.idTipoInmueble
                    WHERE i.{nameof(Inmueble.idInmueble)} = @{nameof(idInmueble)} AND i.{nameof(Inmueble.estado)} = 1";

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
                            direccion = reader[nameof(Inmueble.direccion)]?.ToString() ?? string.Empty,
                            uso = reader[nameof(Inmueble.uso)]?.ToString() ?? string.Empty,
                            cantAmbiente = reader.GetInt32(nameof(Inmueble.cantAmbiente)),
                            valor = reader.GetDecimal(nameof(Inmueble.valor)),
                            disponible = reader.GetBoolean(nameof(Inmueble.disponible)),
                            estado = reader.GetBoolean(nameof(Inmueble.estado)),
                            PropietarioNombre = reader["PropietarioNombre"].ToString(),
                            PropietarioApellido = reader["PropietarioApellido"].ToString(),
                            PropietarioDni = reader["PropietarioDni"].ToString(),
                            TipoNombre = reader["TipoNombre"].ToString()
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

        public bool ExistePorDireccion(string direccion)
        {
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"SELECT COUNT(*) FROM inmueble 
                            WHERE {nameof(Inmueble.direccion)} = @{nameof(direccion)} 
                            AND {nameof(Inmueble.estado)} = 1";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(direccion)}", direccion);
                    conexion.Open();
                    return Convert.ToInt32(comando.ExecuteScalar()) > 0;
                }
            }
        }

        public List<Inmueble> ObtenerDisponibles()
        {
            var lista = new List<Inmueble>();
            using var conexion = new MySqlConnection(connectionString);
            var sql = $@"
                SELECT 
                    i.{nameof(Inmueble.idInmueble)},
                    i.{nameof(Inmueble.idPropietario)},
                    i.{nameof(Inmueble.idTipoInmueble)},
                    i.{nameof(Inmueble.direccion)},
                    i.{nameof(Inmueble.uso)},
                    i.{nameof(Inmueble.cantAmbiente)},
                    i.{nameof(Inmueble.valor)},
                    i.{nameof(Inmueble.disponible)},
                    i.{nameof(Inmueble.estado)},
                    p.{nameof(Propietario.nombre)} AS {nameof(Inmueble.PropietarioNombre)},
                    p.{nameof(Propietario.apellido)} AS {nameof(Inmueble.PropietarioApellido)},
                    p.{nameof(Propietario.dni)} AS {nameof(Inmueble.PropietarioDni)},
                    t.{nameof(TipoInmueble.nombre)} AS {nameof(Inmueble.TipoNombre)}
                FROM inmueble i
                JOIN propietario p ON i.{nameof(Inmueble.idPropietario)} = p.{nameof(Propietario.idPropietario)}
                JOIN tipoinmueble t ON i.{nameof(Inmueble.idTipoInmueble)} = t.{nameof(TipoInmueble.idTipoInmueble)}
                WHERE i.{nameof(Inmueble.estado)} = 1 AND i.{nameof(Inmueble.disponible)} = 1
                ORDER BY i.{nameof(Inmueble.direccion)} ASC;";

            using var comando = new MySqlCommand(sql, conexion);
            conexion.Open();
            using var reader = comando.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Inmueble
                {
                    idInmueble = reader.GetInt32(nameof(Inmueble.idInmueble)),
                    idPropietario = reader.GetInt32(nameof(Inmueble.idPropietario)),
                    idTipoInmueble = reader.GetInt32(nameof(Inmueble.idTipoInmueble)),
                    direccion = reader[nameof(Inmueble.direccion)].ToString() ?? "",
                    uso = reader[nameof(Inmueble.uso)].ToString() ?? "",
                    cantAmbiente = reader.GetInt32(nameof(Inmueble.cantAmbiente)),
                    valor = reader.GetDecimal(nameof(Inmueble.valor)),
                    disponible = reader.GetBoolean(nameof(Inmueble.disponible)),
                    estado = reader.GetBoolean(nameof(Inmueble.estado)),
                    PropietarioNombre = reader[nameof(Inmueble.PropietarioNombre)].ToString(),
                    PropietarioApellido = reader[nameof(Inmueble.PropietarioApellido)].ToString(),
                    PropietarioDni = reader[nameof(Inmueble.PropietarioDni)].ToString(),
                    TipoNombre = reader[nameof(Inmueble.TipoNombre)].ToString()
                });
            }
            return lista;
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
