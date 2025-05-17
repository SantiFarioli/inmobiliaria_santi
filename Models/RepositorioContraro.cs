using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace inmobiliaria_santi.Models
{
    public class RepositorioContrato
    {
        // Cadena de conexión a la base de datos
        private readonly string connectionString = "Server=localhost;Database=inmobiliaria_santi;Uid=root;Pwd=admin;";

        // Obtener todos los contratos activos
        public List<Contrato> ObtenerTodos()
        {
            List<Contrato> contratos = new List<Contrato>();
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"
                    SELECT 
                        c.{nameof(Contrato.idContrato)},
                        c.{nameof(Contrato.idInmueble)},
                        c.{nameof(Contrato.idInquilino)},
                        c.{nameof(Contrato.fechaInicio)},
                        c.{nameof(Contrato.fechaFin)},
                        c.{nameof(Contrato.montoRenta)},
                        c.{nameof(Contrato.deposito)},
                        c.{nameof(Contrato.comision)},
                        c.{nameof(Contrato.condiciones)},
                        c.{nameof(Contrato.multaTerminacionTemprana)},
                        c.{nameof(Contrato.fechaTerminacionTemprana)},
                        c.{nameof(Contrato.usuarioCreacion)},
                        c.{nameof(Contrato.usuarioRescision)},
                        c.{nameof(Contrato.estado)},
                        c.{nameof(Contrato.porcentajeMulta)},
                        i.{nameof(Inmueble.direccion)} AS {nameof(Contrato.InmuebleDireccion)},
                        inq.{nameof(Inquilino.nombre)} AS {nameof(Contrato.InquilinoNombre)},
                        inq.{nameof(Inquilino.apellido)} AS {nameof(Contrato.InquilinoApellido)},
                        p.{nameof(Propietario.nombre)} AS {nameof(Contrato.PropietarioNombre)},
                        p.{nameof(Propietario.apellido)} AS {nameof(Contrato.PropietarioApellido)}
                    FROM contrato c
                    JOIN inmueble i ON c.{nameof(Contrato.idInmueble)} = i.{nameof(Inmueble.idInmueble)}
                    JOIN inquilino inq ON c.{nameof(Contrato.idInquilino)} = inq.{nameof(Inquilino.idInquilino)}
                    JOIN propietario p ON i.{nameof(Inmueble.idPropietario)} = p.{nameof(Propietario.idPropietario)}";


                try
                {
                    using (var comando = new MySqlCommand(sql, conexion))
                    {
                        conexion.Open();
                        var reader = comando.ExecuteReader();
                        while (reader.Read())
                        {
                            contratos.Add(new Contrato
                            {
                                idContrato = reader.GetInt32(reader.GetOrdinal(nameof(Contrato.idContrato))),
                                idInmueble = reader.GetInt32(reader.GetOrdinal(nameof(Contrato.idInmueble))),
                                idInquilino = reader.GetInt32(reader.GetOrdinal(nameof(Contrato.idInquilino))),
                                fechaInicio = reader.GetDateTime(reader.GetOrdinal(nameof(Contrato.fechaInicio))),
                                fechaFin = reader.GetDateTime(reader.GetOrdinal(nameof(Contrato.fechaFin))),
                                montoRenta = reader.GetDecimal(reader.GetOrdinal(nameof(Contrato.montoRenta))),
                                deposito = reader.GetDecimal(reader.GetOrdinal(nameof(Contrato.deposito))),
                                comision = reader.GetDecimal(reader.GetOrdinal(nameof(Contrato.comision))),
                                condiciones = reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.condiciones))) ? null : reader.GetString(nameof(Contrato.condiciones)),
                                multaTerminacionTemprana = reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.multaTerminacionTemprana))) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal(nameof(Contrato.multaTerminacionTemprana))),
                                fechaTerminacionTemprana = reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.fechaTerminacionTemprana))) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal(nameof(Contrato.fechaTerminacionTemprana))),
                                usuarioCreacion = reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.usuarioCreacion))) ? null : reader.GetString(nameof(Contrato.usuarioCreacion)),
                                usuarioRescision = reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.usuarioRescision))) ? null : reader.GetString(nameof(Contrato.usuarioRescision)),
                                estado = reader.GetBoolean(reader.GetOrdinal(nameof(Contrato.estado))), // Asegúrate de usar GetBoolean para tipo tinyint(1)
                                porcentajeMulta = reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.porcentajeMulta))) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal(nameof(Contrato.porcentajeMulta))),
                                InmuebleDireccion = reader.GetString(nameof(Contrato.InmuebleDireccion)),
                                InquilinoNombre = reader.GetString(nameof(Contrato.InquilinoNombre)),
                                InquilinoApellido = reader.GetString(nameof(Contrato.InquilinoApellido)),
                                PropietarioNombre = reader.GetString(nameof(Contrato.PropietarioNombre)),
                                PropietarioApellido = reader.GetString(nameof(Contrato.PropietarioApellido))
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones para saber si algo falla en la consulta o conexión.
                    Console.WriteLine($"Error al obtener contratos: {ex.Message}");
                    // Puedes optar por registrar el error en un log en vez de solo imprimirlo
                }
            }
            return contratos;
        }

        // Obtener un contrato por ID
        public Contrato? ObtenerPorId(int id)
        {
            Contrato? contrato = null;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"
                    SELECT 
                        c.{nameof(Contrato.idContrato)},
                        c.{nameof(Contrato.idInmueble)},
                        c.{nameof(Contrato.idInquilino)},
                        c.{nameof(Contrato.fechaInicio)},
                        c.{nameof(Contrato.fechaFin)},
                        c.{nameof(Contrato.montoRenta)},
                        c.{nameof(Contrato.deposito)},
                        c.{nameof(Contrato.comision)},
                        c.{nameof(Contrato.condiciones)},
                        c.{nameof(Contrato.multaTerminacionTemprana)},
                        c.{nameof(Contrato.fechaTerminacionTemprana)},
                        c.{nameof(Contrato.usuarioCreacion)},
                        c.{nameof(Contrato.usuarioRescision)},
                        c.{nameof(Contrato.estado)},
                        c.{nameof(Contrato.porcentajeMulta)},
                        i.{nameof(Inmueble.direccion)} AS {nameof(Contrato.InmuebleDireccion)},
                        inq.{nameof(Inquilino.nombre)} AS {nameof(Contrato.InquilinoNombre)},
                        inq.{nameof(Inquilino.apellido)} AS {nameof(Contrato.InquilinoApellido)},
                        p.{nameof(Propietario.nombre)} AS {nameof(Contrato.PropietarioNombre)},
                        p.{nameof(Propietario.apellido)} AS {nameof(Contrato.PropietarioApellido)}
                    FROM contrato c
                    JOIN inmueble i ON c.{nameof(Contrato.idInmueble)} = i.{nameof(Inmueble.idInmueble)}
                    JOIN inquilino inq ON c.{nameof(Contrato.idInquilino)} = inq.{nameof(Inquilino.idInquilino)}
                    JOIN propietario p ON i.{nameof(Inmueble.idPropietario)} = p.{nameof(Propietario.idPropietario)}
                    WHERE c.{nameof(Contrato.idContrato)} = @idContrato";

                try
                {
                    using (var comando = new MySqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@idContrato", id);
                        conexion.Open();
                        var reader = comando.ExecuteReader();
                        if (reader.Read())
                        {
                            contrato = new Contrato
                            {
                                idContrato = reader.GetInt32(reader.GetOrdinal(nameof(Contrato.idContrato))),
                                idInmueble = reader.GetInt32(reader.GetOrdinal(nameof(Contrato.idInmueble))),
                                idInquilino = reader.GetInt32(reader.GetOrdinal(nameof(Contrato.idInquilino))),
                                fechaInicio = reader.GetDateTime(reader.GetOrdinal(nameof(Contrato.fechaInicio))),
                                fechaFin = reader.GetDateTime(reader.GetOrdinal(nameof(Contrato.fechaFin))),
                                montoRenta = reader.GetDecimal(reader.GetOrdinal(nameof(Contrato.montoRenta))),
                                deposito = reader.GetDecimal(reader.GetOrdinal(nameof(Contrato.deposito))),
                                comision = reader.GetDecimal(reader.GetOrdinal(nameof(Contrato.comision))),
                                condiciones = reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.condiciones))) ? null : reader.GetString(nameof(Contrato.condiciones)),
                                multaTerminacionTemprana = reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.multaTerminacionTemprana))) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal(nameof(Contrato.multaTerminacionTemprana))),
                                fechaTerminacionTemprana = reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.fechaTerminacionTemprana))) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal(nameof(Contrato.fechaTerminacionTemprana))),
                                usuarioCreacion = reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.usuarioCreacion))) ? null : reader.GetString(nameof(Contrato.usuarioCreacion)),
                                usuarioRescision = reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.usuarioRescision))) ? null : reader.GetString(nameof(Contrato.usuarioRescision)),
                                estado = reader.GetBoolean(reader.GetOrdinal(nameof(Contrato.estado))),
                                porcentajeMulta = reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.porcentajeMulta))) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal(nameof(Contrato.porcentajeMulta))),
                                InmuebleDireccion = reader.GetString(nameof(Contrato.InmuebleDireccion)),
                                InquilinoNombre = reader.GetString(nameof(Contrato.InquilinoNombre)),
                                InquilinoApellido = reader.GetString(nameof(Contrato.InquilinoApellido)),
                                PropietarioNombre = reader.GetString(nameof(Contrato.PropietarioNombre)),
                                PropietarioApellido = reader.GetString(nameof(Contrato.PropietarioApellido))
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener contrato por ID: {ex.Message}");
                }
            }
            return contrato;
        }

        // Crear un contrato en la base de datos
        public void CrearContrato(Contrato contrato)
        {
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = @"INSERT INTO contrato 
                            (idInquilino, idInmueble, fechaInicio, fechaFin, montoRenta, deposito, comision, 
                             condiciones, multaTerminacionTemprana, fechaTerminacionTemprana, usuarioCreacion, usuarioRescision, estado, porcentajeMulta)
                            VALUES 
                            (@idInquilino, @idInmueble, @fechaInicio, @fechaFin, @montoRenta, @deposito, @comision, 
                             @condiciones, @multaTerminacionTemprana, @fechaTerminacionTemprana, @usuarioCreacion, @usuarioRescision, @estado, @porcentajeMulta)";

                try
                {
                    using (var comando = new MySqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@idInquilino", contrato.idInquilino);
                        comando.Parameters.AddWithValue("@idInmueble", contrato.idInmueble);
                        comando.Parameters.AddWithValue("@fechaInicio", contrato.fechaInicio);
                        comando.Parameters.AddWithValue("@fechaFin", contrato.fechaFin);
                        comando.Parameters.AddWithValue("@montoRenta", contrato.montoRenta);
                        comando.Parameters.AddWithValue("@deposito", contrato.deposito);
                        comando.Parameters.AddWithValue("@comision", contrato.comision);
                        comando.Parameters.AddWithValue("@condiciones", contrato.condiciones);
                        comando.Parameters.AddWithValue("@multaTerminacionTemprana", contrato.multaTerminacionTemprana ?? (object)DBNull.Value);
                        comando.Parameters.AddWithValue("@fechaTerminacionTemprana", contrato.fechaTerminacionTemprana ?? (object)DBNull.Value);
                        comando.Parameters.AddWithValue("@usuarioCreacion", string.IsNullOrEmpty(contrato.usuarioCreacion) ? (object)DBNull.Value : contrato.usuarioCreacion);
                        comando.Parameters.AddWithValue("@usuarioRescision", string.IsNullOrEmpty(contrato.usuarioRescision) ? (object)DBNull.Value : contrato.usuarioRescision);
                        comando.Parameters.AddWithValue("@estado", contrato.estado ? 1 : 0); // Asumiendo que estado es bool, 1 para true y 0 para false
                        comando.Parameters.AddWithValue("@porcentajeMulta", contrato.porcentajeMulta ?? (object)DBNull.Value);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear contrato: {ex.Message}");
                }
            }
        }

        // Actualizar un contrato existente
        public void ActualizarContrato(Contrato contrato)
        {
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = @"UPDATE contrato 
                            SET idInquilino = @idInquilino, 
                                idInmueble = @idInmueble, 
                                fechaInicio = @fechaInicio, 
                                fechaFin = @fechaFin, 
                                montoRenta = @montoRenta, 
                                deposito = @deposito, 
                                comision = @comision, 
                                condiciones = @condiciones, 
                                multaTerminacionTemprana = @multaTerminacionTemprana, 
                                fechaTerminacionTemprana = @fechaTerminacionTemprana, 
                                usuarioCreacion = @usuarioCreacion, 
                                usuarioRescision = @usuarioRescision, 
                                estado = @estado, 
                                porcentajeMulta = @porcentajeMulta
                            WHERE idContrato = @idContrato";

                try
                {
                    using (var comando = new MySqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@idContrato", contrato.idContrato);
                        comando.Parameters.AddWithValue("@idInquilino", contrato.idInquilino);
                        comando.Parameters.AddWithValue("@idInmueble", contrato.idInmueble);
                        comando.Parameters.AddWithValue("@fechaInicio", contrato.fechaInicio);
                        comando.Parameters.AddWithValue("@fechaFin", contrato.fechaFin);
                        comando.Parameters.AddWithValue("@montoRenta", contrato.montoRenta);
                        comando.Parameters.AddWithValue("@deposito", contrato.deposito);
                        comando.Parameters.AddWithValue("@comision", contrato.comision);
                        comando.Parameters.AddWithValue("@condiciones", contrato.condiciones ?? (object)DBNull.Value);
                        comando.Parameters.AddWithValue("@multaTerminacionTemprana", contrato.multaTerminacionTemprana ?? (object)DBNull.Value);
                        comando.Parameters.AddWithValue("@fechaTerminacionTemprana", contrato.fechaTerminacionTemprana ?? (object)DBNull.Value);
                        comando.Parameters.AddWithValue("@usuarioCreacion", contrato.usuarioCreacion ?? (object)DBNull.Value);
                        comando.Parameters.AddWithValue("@usuarioRescision", contrato.usuarioRescision ?? (object)DBNull.Value); // 🔧 Este es el nombre correcto
                        comando.Parameters.AddWithValue("@estado", contrato.estado ? 1 : 0);
                        comando.Parameters.AddWithValue("@porcentajeMulta", contrato.porcentajeMulta ?? (object)DBNull.Value);

                        conexion.Open();
                        int filas = comando.ExecuteNonQuery();
                        Console.WriteLine($"Filas afectadas: {filas}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al actualizar contrato: {ex.Message}");
                }
            }
        }


        // Eliminar contrato (lógicamente)
        public void EliminarContrato(int id)
        {
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = "UPDATE contrato SET estado = 0 WHERE idContrato = @idContrato";  // Eliminar lógicamente
                try
                {
                    using (var comando = new MySqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@idContrato", id);
                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al eliminar contrato: {ex.Message}");
                }
            }
        }

        public List<Contrato> ObtenerContratosConResumen()
        {
            var contratos = new List<Contrato>();
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = @"SELECT c.idContrato, CONCAT(i.direccion, ' - ', inq.nombre, ' ', inq.apellido) AS ContratoResumen
                            FROM contrato c
                            INNER JOIN inmueble i ON c.idInmueble = i.idInmueble
                            INNER JOIN inquilino inq ON c.idInquilino = inq.idInquilino
                            WHERE c.estado = 1";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    conexion.Open();
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        contratos.Add(new Contrato
                        {
                            idContrato = reader.GetInt32("idContrato"),
                            ContratoResumen = reader["ContratoResumen"].ToString()
                        });
                    }
                }
            }
            return contratos;
        }

        public bool ExisteContratoActivoEnFechas(int idInmueble, DateTime fechaInicio, DateTime fechaFin)
        {
            using var conexion = new MySqlConnection(connectionString);
            var sql = $@"
                SELECT COUNT(*) FROM contrato
                WHERE {nameof(Contrato.idInmueble)} = @{nameof(idInmueble)}
                AND {nameof(Contrato.estado)} = 1
                AND (@{nameof(fechaInicio)} <= {nameof(Contrato.fechaFin)}
                AND @{nameof(fechaFin)} >= {nameof(Contrato.fechaInicio)})";

            using var comando = new MySqlCommand(sql, conexion);
            comando.Parameters.AddWithValue($"@{nameof(idInmueble)}", idInmueble);
            comando.Parameters.AddWithValue($"@{nameof(fechaInicio)}", fechaInicio);
            comando.Parameters.AddWithValue($"@{nameof(fechaFin)}", fechaFin);

            conexion.Open();
            int cantidad = Convert.ToInt32(comando.ExecuteScalar());
            return cantidad > 0;
        }

        public List<Contrato> ObtenerContratosPorVencer(int dias = 30)
        {
            var lista = new List<Contrato>();
            using var conexion = new MySqlConnection(connectionString);
            var sql = $@"
                SELECT 
                    c.{nameof(Contrato.idContrato)}, c.{nameof(Contrato.idInmueble)}, c.{nameof(Contrato.idInquilino)},
                    c.{nameof(Contrato.fechaInicio)}, c.{nameof(Contrato.fechaFin)}, c.{nameof(Contrato.montoRenta)},
                    i.{nameof(Inmueble.direccion)} AS {nameof(Contrato.InmuebleDireccion)},
                    inq.{nameof(Inquilino.nombre)} AS {nameof(Contrato.InquilinoNombre)},
                    inq.{nameof(Inquilino.apellido)} AS {nameof(Contrato.InquilinoApellido)},
                    p.{nameof(Propietario.nombre)} AS {nameof(Contrato.PropietarioNombre)},
                    p.{nameof(Propietario.apellido)} AS {nameof(Contrato.PropietarioApellido)}
                FROM contrato c
                JOIN inmueble i ON c.{nameof(Contrato.idInmueble)} = i.{nameof(Inmueble.idInmueble)}
                JOIN propietario p ON i.{nameof(Inmueble.idPropietario)} = p.{nameof(Propietario.idPropietario)}
                JOIN inquilino inq ON c.{nameof(Contrato.idInquilino)} = inq.{nameof(Inquilino.idInquilino)}
                WHERE c.{nameof(Contrato.estado)} = 1
                AND c.{nameof(Contrato.fechaFin)} BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL @dias DAY)
                ORDER BY c.{nameof(Contrato.fechaFin)} ASC";

            using var comando = new MySqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@dias", dias);
            conexion.Open();
            using var reader = comando.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Contrato
                {
                    idContrato = reader.GetInt32(nameof(Contrato.idContrato)),
                    idInmueble = reader.GetInt32(nameof(Contrato.idInmueble)),
                    idInquilino = reader.GetInt32(nameof(Contrato.idInquilino)),
                    fechaInicio = reader.GetDateTime(nameof(Contrato.fechaInicio)),
                    fechaFin = reader.GetDateTime(nameof(Contrato.fechaFin)),
                    montoRenta = reader.GetDecimal(nameof(Contrato.montoRenta)),
                    InmuebleDireccion = reader[nameof(Contrato.InmuebleDireccion)].ToString(),
                    InquilinoNombre = reader[nameof(Contrato.InquilinoNombre)].ToString(),
                    InquilinoApellido = reader[nameof(Contrato.InquilinoApellido)].ToString(),
                    PropietarioNombre = reader[nameof(Contrato.PropietarioNombre)].ToString(),
                    PropietarioApellido = reader[nameof(Contrato.PropietarioApellido)].ToString()
                });
            }
            return lista;
        }

        public List<Contrato> ObtenerRescindidos()
        {
            var lista = new List<Contrato>();
            using var conexion = new MySqlConnection(connectionString);
            var sql = $@"
                SELECT 
                    c.{nameof(Contrato.idContrato)}, c.{nameof(Contrato.idInmueble)}, c.{nameof(Contrato.idInquilino)},
                    c.{nameof(Contrato.fechaInicio)}, c.{nameof(Contrato.fechaFin)}, c.{nameof(Contrato.montoRenta)},
                    c.{nameof(Contrato.multaTerminacionTemprana)}, c.{nameof(Contrato.fechaTerminacionTemprana)}, c.{nameof(Contrato.porcentajeMulta)},
                    i.{nameof(Inmueble.direccion)} AS {nameof(Contrato.InmuebleDireccion)},
                    inq.{nameof(Inquilino.nombre)} AS {nameof(Contrato.InquilinoNombre)},
                    inq.{nameof(Inquilino.apellido)} AS {nameof(Contrato.InquilinoApellido)},
                    p.{nameof(Propietario.nombre)} AS {nameof(Contrato.PropietarioNombre)},
                    p.{nameof(Propietario.apellido)} AS {nameof(Contrato.PropietarioApellido)}
                FROM contrato c
                JOIN inmueble i ON c.idInmueble = i.idInmueble
                JOIN inquilino inq ON c.idInquilino = inq.idInquilino
                JOIN propietario p ON i.idPropietario = p.idPropietario
                WHERE c.estado = 0
                ORDER BY c.fechaTerminacionTemprana DESC";

            using var comando = new MySqlCommand(sql, conexion);
            conexion.Open();
            using var reader = comando.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Contrato
                {
                    idContrato = reader.GetInt32(nameof(Contrato.idContrato)),
                    idInmueble = reader.GetInt32(nameof(Contrato.idInmueble)),
                    idInquilino = reader.GetInt32(nameof(Contrato.idInquilino)),
                    fechaInicio = reader.GetDateTime(nameof(Contrato.fechaInicio)),
                    fechaFin = reader.GetDateTime(nameof(Contrato.fechaFin)),
                    montoRenta = reader.GetDecimal(nameof(Contrato.montoRenta)),
                    multaTerminacionTemprana = reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.multaTerminacionTemprana))) ? null : reader.GetDecimal(nameof(Contrato.multaTerminacionTemprana)),
                    fechaTerminacionTemprana = reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.fechaTerminacionTemprana))) ? null : reader.GetDateTime(nameof(Contrato.fechaTerminacionTemprana)),
                    porcentajeMulta = reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.porcentajeMulta))) ? null : reader.GetDecimal(nameof(Contrato.porcentajeMulta)),
                    InmuebleDireccion = reader[nameof(Contrato.InmuebleDireccion)].ToString(),
                    InquilinoNombre = reader[nameof(Contrato.InquilinoNombre)].ToString(),
                    InquilinoApellido = reader[nameof(Contrato.InquilinoApellido)].ToString(),
                    PropietarioNombre = reader[nameof(Contrato.PropietarioNombre)].ToString(),
                    PropietarioApellido = reader[nameof(Contrato.PropietarioApellido)].ToString()
                });
            }
            return lista;
        }

        public List<Contrato> ObtenerContratosActivosPorPropietario(int idPropietario)
        {
            var lista = new List<Contrato>();
            using var conexion = new MySqlConnection(connectionString);
            var sql = $@"
                SELECT c.{nameof(Contrato.idContrato)}, c.{nameof(Contrato.idInmueble)}, c.{nameof(Contrato.idInquilino)},
                    c.{nameof(Contrato.fechaInicio)}, c.{nameof(Contrato.fechaFin)}, c.{nameof(Contrato.montoRenta)},
                    i.{nameof(Inmueble.direccion)} AS {nameof(Contrato.InmuebleDireccion)},
                    inq.{nameof(Inquilino.nombre)} AS {nameof(Contrato.InquilinoNombre)},
                    inq.{nameof(Inquilino.apellido)} AS {nameof(Contrato.InquilinoApellido)}
                FROM contrato c
                INNER JOIN inmueble i ON c.{nameof(Contrato.idInmueble)} = i.{nameof(Inmueble.idInmueble)}
                INNER JOIN inquilino inq ON c.{nameof(Contrato.idInquilino)} = inq.{nameof(Inquilino.idInquilino)}
                WHERE c.{nameof(Contrato.estado)} = 1 AND i.{nameof(Inmueble.idPropietario)} = @{nameof(idPropietario)}";

            using var comando = new MySqlCommand(sql, conexion);
            comando.Parameters.AddWithValue($"@{nameof(idPropietario)}", idPropietario);
            conexion.Open();
            using var reader = comando.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Contrato
                {
                    idContrato = reader.GetInt32(nameof(Contrato.idContrato)),
                    idInmueble = reader.GetInt32(nameof(Contrato.idInmueble)),
                    idInquilino = reader.GetInt32(nameof(Contrato.idInquilino)),
                    fechaInicio = reader.GetDateTime(nameof(Contrato.fechaInicio)),
                    fechaFin = reader.GetDateTime(nameof(Contrato.fechaFin)),
                    montoRenta = reader.GetDecimal(nameof(Contrato.montoRenta)),
                    InmuebleDireccion = reader[nameof(Contrato.InmuebleDireccion)]?.ToString(),
                    InquilinoNombre = reader[nameof(Contrato.InquilinoNombre)]?.ToString(),
                    InquilinoApellido = reader[nameof(Contrato.InquilinoApellido)]?.ToString()
                });
            }
            return lista;
        }
        
        public List<Contrato> ObtenerContratosActivosPorInquilino(int idInquilino)
        {
            var lista = new List<Contrato>();
            using var conexion = new MySqlConnection(connectionString);
            var sql = $@"
                SELECT c.{nameof(Contrato.idContrato)}, c.{nameof(Contrato.idInmueble)}, c.{nameof(Contrato.idInquilino)},
                    c.{nameof(Contrato.fechaInicio)}, c.{nameof(Contrato.fechaFin)}, c.{nameof(Contrato.montoRenta)},
                    c.{nameof(Contrato.deposito)}, c.{nameof(Contrato.comision)}, c.{nameof(Contrato.condiciones)},
                    c.{nameof(Contrato.estado)}, i.{nameof(Inquilino.nombre)} AS InquilinoNombre,
                    i.{nameof(Inquilino.apellido)} AS InquilinoApellido,
                    inm.{nameof(Inmueble.direccion)} AS InmuebleDireccion
                FROM contrato c
                INNER JOIN inquilino i ON c.{nameof(Contrato.idInquilino)} = i.{nameof(Inquilino.idInquilino)}
                INNER JOIN inmueble inm ON c.{nameof(Contrato.idInmueble)} = inm.{nameof(Inmueble.idInmueble)}
                WHERE c.{nameof(Contrato.estado)} = 1 AND c.{nameof(Contrato.idInquilino)} = @{nameof(idInquilino)};";

            using var comando = new MySqlCommand(sql, conexion);
            comando.Parameters.AddWithValue($"@{nameof(idInquilino)}", idInquilino);
            conexion.Open();
            using var reader = comando.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Contrato
                {
                    idContrato = reader.GetInt32(nameof(Contrato.idContrato)),
                    idInmueble = reader.GetInt32(nameof(Contrato.idInmueble)),
                    idInquilino = reader.GetInt32(nameof(Contrato.idInquilino)),
                    fechaInicio = reader.GetDateTime(nameof(Contrato.fechaInicio)),
                    fechaFin = reader.GetDateTime(nameof(Contrato.fechaFin)),
                    montoRenta = reader.GetDecimal(nameof(Contrato.montoRenta)),
                    deposito = reader.GetDecimal(nameof(Contrato.deposito)),
                    comision = reader.GetDecimal(nameof(Contrato.comision)),
                    condiciones = reader[nameof(Contrato.condiciones)]?.ToString(),
                    estado = reader.GetBoolean(nameof(Contrato.estado)),
                    InquilinoNombre = reader[nameof(Contrato.InquilinoNombre)]?.ToString(),
                    InquilinoApellido = reader[nameof(Contrato.InquilinoApellido)]?.ToString(),
                    InmuebleDireccion = reader[nameof(Contrato.InmuebleDireccion)]?.ToString()
                });
            }
            return lista;
        }
    }
}
