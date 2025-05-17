using inmobiliaria_santi.Models;
using MySql.Data.MySqlClient;

public class RepositorioPago
{
    private readonly string connectionString = "Server=localhost;Database=inmobiliaria_santi;Uid=root;Pwd=admin;";

    public List<Pago> ObtenerTodos()
    {
        List<Pago> pagos = new List<Pago>();
        using (var conexion = new MySqlConnection(connectionString))
        {
            var sql = $@"SELECT p.{nameof(Pago.idPago)}, p.{nameof(Pago.idContrato)}, p.{nameof(Pago.nroPago)},
                                p.{nameof(Pago.fechaPago)}, p.{nameof(Pago.detalle)}, p.{nameof(Pago.importe)},
                                p.{nameof(Pago.estado)}, p.{nameof(Pago.usuarioCreacion)},
                                p.{nameof(Pago.usuarioAnulacion)}, p.{nameof(Pago.usuarioEliminacion)},
                                CONCAT(i.direccion, ' - ', inq.nombre, ' ', inq.apellido) AS ContratoResumen
                        FROM pago p
                        INNER JOIN contrato c ON p.idContrato = c.idContrato
                        INNER JOIN inmueble i ON c.idInmueble = i.idInmueble
                        INNER JOIN inquilino inq ON c.idInquilino = inq.idInquilino
                        ORDER BY p.fechaPago DESC;";
            using (var comando = new MySqlCommand(sql, conexion))
            {
                conexion.Open();
                var reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    pagos.Add(new Pago
                    {
                        idPago = reader.GetInt32(nameof(Pago.idPago)),
                        idContrato = reader.GetInt32(nameof(Pago.idContrato)),
                        nroPago = reader.GetInt32(nameof(Pago.nroPago)),
                        fechaPago = reader.GetDateTime(nameof(Pago.fechaPago)),
                        detalle = reader[nameof(Pago.detalle)].ToString(),
                        importe = reader.GetDecimal(nameof(Pago.importe)),
                        estado = reader.GetBoolean(nameof(Pago.estado)),
                        usuarioCreacion = reader[nameof(Pago.usuarioCreacion)].ToString(),
                        usuarioAnulacion = reader[nameof(Pago.usuarioAnulacion)].ToString(),
                        usuarioEliminacion = reader[nameof(Pago.usuarioEliminacion)].ToString(),
                        ContratoResumen = reader["ContratoResumen"].ToString()
                    });
                }
            }
        }
        return pagos;
    }

    public Pago? ObtenerPorId(int idPago)
    {
        Pago? pago = null;
        using (var conexion = new MySqlConnection(connectionString))
        {
            var sql = $@"SELECT p.{nameof(Pago.idPago)}, p.{nameof(Pago.idContrato)}, p.{nameof(Pago.nroPago)},
                                p.{nameof(Pago.fechaPago)}, p.{nameof(Pago.detalle)}, p.{nameof(Pago.importe)},
                                p.{nameof(Pago.estado)}, p.{nameof(Pago.usuarioCreacion)},
                                p.{nameof(Pago.usuarioAnulacion)}, p.{nameof(Pago.usuarioEliminacion)},
                                CONCAT(i.direccion, ' - ', inq.nombre, ' ', inq.apellido) AS contratoResumen
                        FROM pago p
                        INNER JOIN contrato c ON p.idContrato = c.idContrato
                        INNER JOIN inmueble i ON c.idInmueble = i.idInmueble
                        INNER JOIN inquilino inq ON c.idInquilino = inq.idInquilino
                        WHERE p.{nameof(Pago.idPago)} = @{nameof(idPago)}";
            using (var comando = new MySqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue($"@{nameof(idPago)}", idPago);
                conexion.Open();
                var reader = comando.ExecuteReader();
                if (reader.Read())
                {
                    pago = new Pago
                    {
                        idPago = reader.GetInt32(nameof(Pago.idPago)),
                        idContrato = reader.GetInt32(nameof(Pago.idContrato)),
                        nroPago = reader.GetInt32(nameof(Pago.nroPago)),
                        fechaPago = reader.GetDateTime(nameof(Pago.fechaPago)),
                        detalle = reader[nameof(Pago.detalle)].ToString(),
                        importe = reader.GetDecimal(nameof(Pago.importe)),
                        estado = reader.GetBoolean(nameof(Pago.estado)),
                        usuarioCreacion = reader[nameof(Pago.usuarioCreacion)].ToString(),
                        usuarioAnulacion = reader[nameof(Pago.usuarioAnulacion)].ToString(),
                        usuarioEliminacion = reader[nameof(Pago.usuarioEliminacion)].ToString(),
                        ContratoResumen = reader["contratoResumen"].ToString()
                    };
                }
            }
        }
        return pago;
    }

    public int CrearPago(Pago pago)
    {
        int id = 0;
        using (var conexion = new MySqlConnection(connectionString))
        {
            var sql = $@"INSERT INTO pago ({nameof(Pago.idContrato)}, {nameof(Pago.nroPago)},
                                             {nameof(Pago.fechaPago)}, {nameof(Pago.detalle)},
                                             {nameof(Pago.importe)}, {nameof(Pago.estado)},
                                             {nameof(Pago.usuarioCreacion)})
                         VALUES (@{nameof(Pago.idContrato)}, @{nameof(Pago.nroPago)}, @{nameof(Pago.fechaPago)},
                                 @{nameof(Pago.detalle)}, @{nameof(Pago.importe)}, 1, @{nameof(Pago.usuarioCreacion)});
                         SELECT LAST_INSERT_ID();";
            using (var comando = new MySqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue($"@{nameof(Pago.idContrato)}", pago.idContrato);
                comando.Parameters.AddWithValue($"@{nameof(Pago.nroPago)}", pago.nroPago);
                comando.Parameters.AddWithValue($"@{nameof(Pago.fechaPago)}", pago.fechaPago);
                comando.Parameters.AddWithValue($"@{nameof(Pago.detalle)}", pago.detalle ?? "");
                comando.Parameters.AddWithValue($"@{nameof(Pago.importe)}", pago.importe);
                comando.Parameters.AddWithValue($"@{nameof(Pago.usuarioCreacion)}", pago.usuarioCreacion ?? "");
                conexion.Open();
                id = Convert.ToInt32(comando.ExecuteScalar());
                pago.idPago = id;
            }
        }
        return id;
    }

    public int ActualizarPago(Pago pago)
    {
        int filasAfectadas = 0;
        using (var conexion = new MySqlConnection(connectionString))
        {
            var sql = $@"UPDATE pago
                         SET {nameof(Pago.idContrato)} = @{nameof(Pago.idContrato)},
                             {nameof(Pago.nroPago)} = @{nameof(Pago.nroPago)},
                             {nameof(Pago.fechaPago)} = @{nameof(Pago.fechaPago)},
                             {nameof(Pago.detalle)} = @{nameof(Pago.detalle)},
                             {nameof(Pago.importe)} = @{nameof(Pago.importe)}
                         WHERE {nameof(Pago.idPago)} = @{nameof(Pago.idPago)}";
            using (var comando = new MySqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue($"@{nameof(Pago.idContrato)}", pago.idContrato);
                comando.Parameters.AddWithValue($"@{nameof(Pago.nroPago)}", pago.nroPago);
                comando.Parameters.AddWithValue($"@{nameof(Pago.fechaPago)}", pago.fechaPago);
                comando.Parameters.AddWithValue($"@{nameof(Pago.detalle)}", pago.detalle ?? "");
                comando.Parameters.AddWithValue($"@{nameof(Pago.importe)}", pago.importe);
                comando.Parameters.AddWithValue($"@{nameof(Pago.idPago)}", pago.idPago);
                conexion.Open();
                filasAfectadas = comando.ExecuteNonQuery();
            }
        }
        return filasAfectadas;
    }

    public int EliminarPago(int idPago, string usuario)
    {
        int filasAfectadas = 0;
        using (var conexion = new MySqlConnection(connectionString))
        {
            var sql = $@"UPDATE pago
                         SET {nameof(Pago.estado)} = 0,
                             {nameof(Pago.usuarioEliminacion)} = @{nameof(Pago.usuarioEliminacion)}
                         WHERE {nameof(Pago.idPago)} = @{nameof(idPago)}";
            using (var comando = new MySqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue($"@{nameof(idPago)}", idPago);
                comando.Parameters.AddWithValue($"@{nameof(Pago.usuarioEliminacion)}", usuario);
                conexion.Open();
                filasAfectadas = comando.ExecuteNonQuery();
            }
        }
        return filasAfectadas;
    }

    public bool ExistePago(int idContrato, int nroPago, int idPago)
    {
        using (var conexion = new MySqlConnection(connectionString))
        {
            var sql = $@"SELECT COUNT(*) FROM pago
                        WHERE {nameof(Pago.idContrato)} = @{nameof(idContrato)} 
                        AND {nameof(Pago.nroPago)} = @{nameof(nroPago)}
                        AND {nameof(Pago.estado)} = 1";
            using (var comando = new MySqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue($"@{nameof(idContrato)}", idContrato);
                comando.Parameters.AddWithValue($"@{nameof(nroPago)}", nroPago);
                conexion.Open();
                var resultado = Convert.ToInt32(comando.ExecuteScalar());
                return resultado > 0;
            }
        }
    }

    public int ObtenerUltimoNumeroPago(int idContrato)
    {
        int ultimo = 0;
        using (var conexion = new MySqlConnection(connectionString))
        {
            var sql = $@"SELECT IFNULL(MAX({nameof(Pago.nroPago)}), 0)
                        FROM pago
                        WHERE {nameof(Pago.idContrato)} = @{nameof(idContrato)}";

            using (var comando = new MySqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue($"@{nameof(idContrato)}", idContrato);
                conexion.Open();
                var resultado = comando.ExecuteScalar();
                if (resultado != null && resultado != DBNull.Value)
                    ultimo = Convert.ToInt32(resultado);
            }
        }
        return ultimo;
    }
    
    public List<Pago> ObtenerPagosPorContrato(int idContrato)
    {
        var lista = new List<Pago>();
        using var conexion = new MySqlConnection(connectionString);
        var sql = $@"
            SELECT 
                p.{nameof(Pago.idPago)},
                p.{nameof(Pago.idContrato)},
                p.{nameof(Pago.nroPago)},
                p.{nameof(Pago.fechaPago)},
                p.{nameof(Pago.detalle)},
                p.{nameof(Pago.importe)},
                p.{nameof(Pago.estado)},
                p.{nameof(Pago.usuarioCreacion)},
                p.{nameof(Pago.usuarioAnulacion)},
                p.{nameof(Pago.usuarioEliminacion)},
                CONCAT(i.direccion, ' - ', inq.nombre, ' ', inq.apellido) AS ContratoResumen
            FROM pago p
            INNER JOIN contrato c ON p.idContrato = c.idContrato
            INNER JOIN inmueble i ON c.idInmueble = i.idInmueble
            INNER JOIN inquilino inq ON c.idInquilino = inq.idInquilino
            WHERE p.idContrato = @idContrato
            ORDER BY p.fechaPago DESC";

        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@idContrato", idContrato);
        conexion.Open();

        using var reader = comando.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Pago
            {
                idPago = reader.GetInt32(nameof(Pago.idPago)),
                idContrato = reader.GetInt32(nameof(Pago.idContrato)),
                nroPago = reader.GetInt32(nameof(Pago.nroPago)),
                fechaPago = reader.GetDateTime(nameof(Pago.fechaPago)),
                detalle = reader[nameof(Pago.detalle)]?.ToString(),
                importe = reader.GetDecimal(nameof(Pago.importe)),
                estado = reader.GetBoolean(nameof(Pago.estado)),
                usuarioCreacion = reader[nameof(Pago.usuarioCreacion)]?.ToString(),
                usuarioAnulacion = reader[nameof(Pago.usuarioAnulacion)]?.ToString(),
                usuarioEliminacion = reader[nameof(Pago.usuarioEliminacion)]?.ToString(),
                ContratoResumen = reader["ContratoResumen"]?.ToString()
            });
        }
        return lista;
    }

}
