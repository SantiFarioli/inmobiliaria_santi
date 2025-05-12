using MySql.Data.MySqlClient;
using System.Data;

namespace inmobiliaria_santi.Models
{
    public class RepositorioUsuario
    {
        private readonly string connectionString = "Server=localhost;Database=inmobiliaria_santi;Uid=root;Pwd=admin;";

        public List<Usuario> ObtenerTodos()
        {
            var lista = new List<Usuario>();
            using var connection = new MySqlConnection(connectionString);
            string sql = "SELECT * FROM usuario WHERE estado = 1;";
            using var command = new MySqlCommand(sql, connection);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var usuario = new Usuario
                {
                    idUsuario = reader.GetInt32("idUsuario"),
                    nombre = reader["nombre"].ToString(),
                    apellido = reader["apellido"].ToString(),
                    email = reader["email"].ToString(),
                    contrasena = reader["contrasena"].ToString(),
                    avatar = reader["avatar"].ToString(),
                    rol = reader.GetInt32("rol"),
                    estado = reader.GetBoolean("estado")
                };
                lista.Add(usuario);
            }
            return lista;
        }

        // Obtener un usuario por email (para login)
        public Usuario? ObtenerPorEmail(string email)
        {
            Usuario? usuario = null;
            using var connection = new MySqlConnection(connectionString);
            string sql = @"SELECT * FROM usuario WHERE email = @email AND estado = 1;";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                usuario = new Usuario
                {
                    idUsuario = reader.GetInt32("idUsuario"),
                    nombre = reader["nombre"].ToString(),
                    apellido = reader["apellido"].ToString(),
                    email = reader["email"].ToString(),
                    contrasena = reader["contrasena"].ToString(),
                    avatar = reader["avatar"].ToString(),
                    rol = reader.GetInt32("rol"),
                    estado = reader.GetBoolean("estado")
                };
            }
            return usuario;
        }

        // Validar login (email + contraseña)
        public Usuario? ValidarUsuario(string email, string clave)
        {
            var usuario = ObtenerPorEmail(email);
            if (usuario != null && usuario.contrasena == HashHelper.CalcularHash(clave))
                return usuario;
            return null;
        }

        // Obtener por ID
        public Usuario ObtenerPorId(int id)
        {
            Usuario? usuario = null;
            using var connection = new MySqlConnection(connectionString);
            string sql = @"SELECT * FROM usuario WHERE idUsuario = @id;";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                usuario = new Usuario
                {
                    idUsuario = reader.GetInt32("idUsuario"),
                    nombre = reader["nombre"].ToString(),
                    apellido = reader["apellido"].ToString(),
                    email = reader["email"].ToString(),
                    contrasena = reader["contrasena"].ToString(),
                    avatar = reader["avatar"].ToString(),
                    rol = reader.GetInt32("rol"),
                    estado = reader.GetBoolean("estado")
                };
            }
            return usuario!;
        }

        // Alta de usuario (solo para admins)
        public int Alta(Usuario u)
        {
            int id = 0;
            using var connection = new MySqlConnection(connectionString);
            string sql = @"INSERT INTO usuario (nombre, apellido, email, contrasena, avatar, rol, estado)
                           VALUES (@nombre, @apellido, @email, @contrasena, @avatar, @rol, 1);
                           SELECT LAST_INSERT_ID();";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@nombre", u.nombre);
            command.Parameters.AddWithValue("@apellido", u.apellido);
            command.Parameters.AddWithValue("@email", u.email);
            command.Parameters.AddWithValue("@contrasena", u.contrasena);
            command.Parameters.AddWithValue("@avatar", u.avatar ?? "");
            command.Parameters.AddWithValue("@rol", u.rol);
            connection.Open();
            id = Convert.ToInt32(command.ExecuteScalar());
            u.idUsuario = id;
            return id;
        }

        // Modificar usuario (solo para admins)

        public void ActualizarUsuarioAdmin(Usuario u)
        {
            using var connection = new MySqlConnection(connectionString);
            string sql = @"UPDATE usuario 
                        SET nombre = @nombre, apellido = @apellido, email = @email, 
                            avatar = @avatar, rol = @rol
                        WHERE idUsuario = @id;";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@nombre", u.nombre);
            command.Parameters.AddWithValue("@apellido", u.apellido);
            command.Parameters.AddWithValue("@email", u.email);
            command.Parameters.AddWithValue("@avatar", u.avatar ?? "");
            command.Parameters.AddWithValue("@rol", u.rol);
            command.Parameters.AddWithValue("@id", u.idUsuario);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void ActualizarUsuarioEmpleado(Usuario u)
        {
            using var connection = new MySqlConnection(connectionString);
            string sql = @"UPDATE usuario 
                        SET nombre = @nombre,
                            apellido = @apellido,
                            email = @email,
                            avatar = @avatar
                        WHERE idUsuario = @id;";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@nombre", u.nombre);
            command.Parameters.AddWithValue("@apellido", u.apellido);
            command.Parameters.AddWithValue("@email", u.email);
            command.Parameters.AddWithValue("@avatar", u.avatar ?? "");
            command.Parameters.AddWithValue("@id", u.idUsuario);
            connection.Open();
            command.ExecuteNonQuery();
        }


        // Eliminar usuario (solo para admins)
        public void EliminarUsuario(int id)
        {
            using var connection = new MySqlConnection(connectionString);
            string sql = @"UPDATE usuario SET estado = 0 WHERE idUsuario = @id;";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
        
    }
}
