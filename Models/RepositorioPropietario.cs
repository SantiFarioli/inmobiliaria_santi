
using inmobiliaria_santi.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;


    public class RepositorioPropietario
    {
        private readonly string connectionString = "Server=localhost;Database=inmobiliaria_santi;Uid=root;Pwd=admin;";

        public List<Propietario> ObtenerTodos()
        {
            List<Propietario> propietarios = new List<Propietario>();
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"SELECT {nameof(Propietario.idPropietario)}, {nameof(Propietario.nombre)}, 
                                    {nameof(Propietario.apellido)}, {nameof(Propietario.dni)}, 
                                    {nameof(Propietario.telefono)}, {nameof(Propietario.email)}, {nameof(Propietario.estado)} 
                             FROM propietario
                             WHERE {nameof(Propietario.estado)} = 1";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    conexion.Open();
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        propietarios.Add(new Propietario
                        {
                            idPropietario = reader.GetInt32(nameof(Propietario.idPropietario)),
                            nombre = reader.GetString(nameof(Propietario.nombre)),
                            apellido = reader.GetString(nameof(Propietario.apellido)),
                            dni = reader.GetInt32(nameof(Propietario.dni)),
                            telefono = reader.GetString(nameof(Propietario.telefono)),
                            email = reader.GetString(nameof(Propietario.email)),
                            estado = reader.GetBoolean(nameof(Propietario.estado))
                        });
                    }
                }
            }
            return propietarios;
        }

        public Propietario? ObtenerPorId(int idPropietario)
        {
            Propietario? propietario = null;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"SELECT {nameof(Propietario.idPropietario)}, {nameof(Propietario.nombre)}, 
                                    {nameof(Propietario.apellido)}, {nameof(Propietario.dni)}, 
                                    {nameof(Propietario.telefono)}, {nameof(Propietario.email)}, {nameof(Propietario.estado)} 
                             FROM propietario
                             WHERE {nameof(Propietario.idPropietario)} = @{nameof(idPropietario)} 
                             AND {nameof(Propietario.estado)} = 1";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(idPropietario)}", idPropietario);
                    conexion.Open();
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        propietario = new Propietario
                        {
                            idPropietario = reader.GetInt32(nameof(Propietario.idPropietario)),
                            nombre = reader.GetString(nameof(Propietario.nombre)),
                            apellido = reader.GetString(nameof(Propietario.apellido)),
                            dni = reader.GetInt32(nameof(Propietario.dni)),
                            telefono = reader.GetString(nameof(Propietario.telefono)),
                            email = reader.GetString(nameof(Propietario.email)),
                            estado = reader.GetBoolean(nameof(Propietario.estado))
                        };
                    }
                }
            }
            return propietario;
        }

        public int CrearPropietario(Propietario propietario)
        {
            int id = 0;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"INSERT INTO propietario 
                             ({nameof(Propietario.nombre)}, {nameof(Propietario.apellido)}, {nameof(Propietario.dni)}, 
                              {nameof(Propietario.telefono)}, {nameof(Propietario.email)}, {nameof(Propietario.estado)})
                             VALUES
                             (@{nameof(Propietario.nombre)}, @{nameof(Propietario.apellido)}, @{nameof(Propietario.dni)}, 
                              @{nameof(Propietario.telefono)}, @{nameof(Propietario.email)}, 1);
                             SELECT LAST_INSERT_ID();";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(Propietario.nombre)}", propietario.nombre);
                    comando.Parameters.AddWithValue($"@{nameof(Propietario.apellido)}", propietario.apellido);
                    comando.Parameters.AddWithValue($"@{nameof(Propietario.dni)}", propietario.dni);
                    comando.Parameters.AddWithValue($"@{nameof(Propietario.telefono)}", propietario.telefono);
                    comando.Parameters.AddWithValue($"@{nameof(Propietario.email)}", propietario.email);
                    conexion.Open();
                    id = Convert.ToInt32(comando.ExecuteScalar());
                    propietario.idPropietario = id;
                }
            }
            return id;
        }

        public int ActualizarPropietario(Propietario propietario)
        {
            int filasAfectadas = 0;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"UPDATE propietario
                             SET {nameof(Propietario.nombre)} = @{nameof(Propietario.nombre)},
                                 {nameof(Propietario.apellido)} = @{nameof(Propietario.apellido)},
                                 {nameof(Propietario.dni)} = @{nameof(Propietario.dni)},
                                 {nameof(Propietario.telefono)} = @{nameof(Propietario.telefono)},
                                 {nameof(Propietario.email)} = @{nameof(Propietario.email)}
                             WHERE {nameof(Propietario.idPropietario)} = @{nameof(Propietario.idPropietario)}";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(Propietario.nombre)}", propietario.nombre);
                    comando.Parameters.AddWithValue($"@{nameof(Propietario.apellido)}", propietario.apellido);
                    comando.Parameters.AddWithValue($"@{nameof(Propietario.dni)}", propietario.dni);
                    comando.Parameters.AddWithValue($"@{nameof(Propietario.telefono)}", propietario.telefono);
                    comando.Parameters.AddWithValue($"@{nameof(Propietario.email)}", propietario.email);
                    comando.Parameters.AddWithValue($"@{nameof(Propietario.idPropietario)}", propietario.idPropietario);
                    conexion.Open();
                    filasAfectadas = comando.ExecuteNonQuery();
                }
            }
            return filasAfectadas;
        }

        public int EliminarPropietario(int idPropietario)
        {
            int filasAfectadas = 0;
            using (var conexion = new MySqlConnection(connectionString))
            {
                var sql = $@"UPDATE propietario
                             SET {nameof(Propietario.estado)} = 0
                             WHERE {nameof(Propietario.idPropietario)} = @{nameof(idPropietario)}";
                using (var comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue($"@{nameof(idPropietario)}", idPropietario);
                    conexion.Open();
                    filasAfectadas = comando.ExecuteNonQuery();
                }
            }
            return filasAfectadas;
        }
    }

