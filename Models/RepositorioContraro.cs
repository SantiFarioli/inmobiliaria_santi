using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace inmobiliaria_santi.Models
{
    public class RepositorioContrato
    {
        // Asegúrate de tener la cadena de conexión correcta
        private readonly string connectionString = "Server=localhost;Database=inmobiliaria_santi;Uid=root;Pwd=admin;";

        // Obtener todos los contratos activos
        public List<Contrato> ObtenerTodos()
        {
            List<Contrato> contratos = new List<Contrato>();
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = "SELECT * FROM contrato WHERE estado = 1";  // Solo contratos activos
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    conexion.Open();
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        contratos.Add(new Contrato
                        {
                            idContrato = reader.GetInt32(reader.GetOrdinal("idContrato")),
                            idInmueble = reader.GetInt32(reader.GetOrdinal("idInmueble")),
                            idInquilino = reader.GetInt32(reader.GetOrdinal("idInquilino")),
                            fechaInicio = reader.GetDateTime(reader.GetOrdinal("fechaInicio")),
                            fechaFin = reader.GetDateTime(reader.GetOrdinal("fechaFin")),
                            montoRenta = reader.GetDecimal(reader.GetOrdinal("montoRenta")),
                            deposito = reader.GetString(reader.GetOrdinal("deposito")),
                            comision = reader.GetString(reader.GetOrdinal("comision")),
                            condiciones = reader.GetString(reader.GetOrdinal("condiciones")),
                            multaTerminacionTemprana = reader.IsDBNull(reader.GetOrdinal("multaTerminacionTemprana")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("multaTerminacionTemprana")),
                            fechaTerminacionTemprana = reader.IsDBNull(reader.GetOrdinal("fechaTerminacionTemprana")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fechaTerminacionTemprana")),
                            usuarioCreacion = reader.IsDBNull(reader.GetOrdinal("usuarioCreacion")) ? null : reader.GetString(reader.GetOrdinal("usuarioCreacion")),
                            usuarioTerminacion = reader.IsDBNull(reader.GetOrdinal("usuarioTerminacion")) ? null : reader.GetString(reader.GetOrdinal("usuarioTerminacion"))
                        });
                    }
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
                var sql = "SELECT * FROM contrato WHERE idContrato = @idContrato";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@idContrato", id);  // El ID es un entero, no string
                    conexion.Open();
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        contrato = new Contrato
                        {
                            idContrato = reader.GetInt32(reader.GetOrdinal("idContrato")),
                            idInmueble = reader.GetInt32(reader.GetOrdinal("idInmueble")),
                            idInquilino = reader.GetInt32(reader.GetOrdinal("idInquilino")),
                            fechaInicio = reader.GetDateTime(reader.GetOrdinal("fechaInicio")),
                            fechaFin = reader.GetDateTime(reader.GetOrdinal("fechaFin")),
                            montoRenta = reader.GetDecimal(reader.GetOrdinal("montoRenta")),
                            deposito = reader.GetString(reader.GetOrdinal("deposito")),
                            comision = reader.GetString(reader.GetOrdinal("comision")),
                            condiciones = reader.GetString(reader.GetOrdinal("condiciones")),
                            multaTerminacionTemprana = reader.IsDBNull(reader.GetOrdinal("multaTerminacionTemprana")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("multaTerminacionTemprana")),
                            fechaTerminacionTemprana = reader.IsDBNull(reader.GetOrdinal("fechaTerminacionTemprana")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fechaTerminacionTemprana")),
                            usuarioCreacion = reader.IsDBNull(reader.GetOrdinal("usuarioCreacion")) ? null : reader.GetString(reader.GetOrdinal("usuarioCreacion")),
                            usuarioTerminacion = reader.IsDBNull(reader.GetOrdinal("usuarioTerminacion")) ? null : reader.GetString(reader.GetOrdinal("usuarioTerminacion"))
                        };
                    }
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
                             condiciones, multaTerminacionTemprana, fechaTerminacionTemprana, usuarioCreacion, usuarioTerminacion)
                            VALUES 
                            (@idInquilino, @idInmueble, @fechaInicio, @fechaFin, @montoRenta, @deposito, @comision, 
                             @condiciones, @multaTerminacionTemprana, @fechaTerminacionTemprana, @usuarioCreacion, @usuarioTerminacion)";
                
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
                    comando.Parameters.AddWithValue("@usuarioTerminacion", string.IsNullOrEmpty(contrato.usuarioTerminacion) ? (object)DBNull.Value : contrato.usuarioTerminacion);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        // Actualizar un contrato existente
        public void ActualizarContrato(Contrato contrato)
        {
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = @"UPDATE contrato 
                            SET idInquilino = @idInquilino, idInmueble = @idInmueble, fechaInicio = @fechaInicio, 
                                fechaFin = @fechaFin, montoRenta = @montoRenta, deposito = @deposito, 
                                comision = @comision, condiciones = @condiciones, multaTerminacionTemprana = @multaTerminacionTemprana, 
                                fechaTerminacionTemprana = @fechaTerminacionTemprana, usuarioCreacion = @usuarioCreacion, 
                                usuarioTerminacion = @usuarioTerminacion
                            WHERE idContrato = @idContrato";
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
                    comando.Parameters.AddWithValue("@condiciones", contrato.condiciones);
                    comando.Parameters.AddWithValue("@multaTerminacionTemprana", contrato.multaTerminacionTemprana ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@fechaTerminacionTemprana", contrato.fechaTerminacionTemprana ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@usuarioCreacion", string.IsNullOrEmpty(contrato.usuarioCreacion) ? (object)DBNull.Value : contrato.usuarioCreacion);
                    comando.Parameters.AddWithValue("@usuarioTerminacion", string.IsNullOrEmpty(contrato.usuarioTerminacion) ? (object)DBNull.Value : contrato.usuarioTerminacion);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        // Eliminar contrato (lógicamente)
        public void EliminarContrato(int id)
        {
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = "UPDATE contrato SET estado = 0 WHERE idContrato = @idContrato";  // Eliminar lógicamente
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@idContrato", id);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}
