using Api_Intregacion.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Api_Intregacion.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {

        private readonly string con;

        public UsuarioController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("conexion");
        }

        [HttpGet("UsuarioxId/{IdUsuario}")]
        public IEnumerable<UsuarioModel> GetUsuariosById(int IdUsuario)
        {
            var list_order = new List<UsuarioModel>();

            using (SqlConnection connection = new(con))
            {
                connection.Open();

                string query = @"SELECT * FROM Usuario WHERE IdUsuario = @IdUsuario";

                using (SqlCommand command = new(query, connection))
                {
                    command.Parameters.AddWithValue("@IdUsuario", IdUsuario);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var order = new UsuarioModel
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                                Correo = reader.GetString(reader.GetOrdinal("Correo")),
                                Contraseña = reader.GetString(reader.GetOrdinal("Contraseña")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                            };

                            list_order.Add(order);
                        }
                    }
                }
            }
            return list_order;
        }

        // GET: api/ListaUsuarios
        [HttpGet("ListaUsuarios")]
        public IEnumerable<UsuarioModel> GetUsuarios()
        {
            var list_Orders = new List<UsuarioModel>();

            using (SqlConnection connection = new(con))
            {
                connection.Open();

                string query = @"Select * from Usuario";

                using (SqlCommand command = new(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var order = new UsuarioModel
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                                Correo = reader.GetString(reader.GetOrdinal("Correo")),
                                Contraseña = reader.GetString(reader.GetOrdinal("Contraseña")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                            };

                            list_Orders.Add(order);
                        }
                    }
                }


                return list_Orders;
            }
        }

        // POST api/AgregarUsuario>
        [HttpPost("AgregarUsuario")]
        public IActionResult AgregarUsuario([FromBody] UsuarioModel newUser)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                string query = @"
                INSERT INTO Usuario (Nombre, Apellido, Correo, Contraseña, Activo)
                VALUES (@Nombre, @Apellido, @Correo, @Contraseña, @Activo);
                ";
                using (SqlCommand command = new(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", newUser.Nombre);
                    command.Parameters.AddWithValue("@Apellido", newUser.Apellido);
                    command.Parameters.AddWithValue("@Correo", newUser.Correo);
                    command.Parameters.AddWithValue("@Contraseña", newUser.Contraseña);
                    command.Parameters.AddWithValue("@Activo", newUser.Activo);

                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificar si la inserción fue exitosa
                    if (rowsAffected > 0)
                    {
                        return Ok("Usuario insertado exitosamente.");
                    }
                    else
                    {
                        return BadRequest("Error al insertar el Usuario.");
                    }
                }
            }
        }

        // PUT api/<UsuarioController>/55
        [HttpPut("{IdUsuario}")]
        public IActionResult PutOrder(int IdUsuario, [FromBody] UsuarioModel user)
        {
                // Cadena de conexión obtenida del constructor a través de IConfiguration
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();

                string query = @"UPDATE Usuario
                            SET Nombre = @Nombre, 
                                Apellido = @Apellido, 
                                Correo = @Correo, 
                                Contraseña = @Contraseña, 
                                Activo = @Activo
                                WHERE IdUsuario = @IdUsuario";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Asignar valores a los parámetros.
                    command.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                    command.Parameters.AddWithValue("@Nombre", user.Nombre);
                    command.Parameters.AddWithValue("@Apellido", user.Apellido ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Correo", user.Correo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Contraseña", user.Contraseña ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Activo", user.Activo);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Se actualizo exitosamente." });
                    }
                    else
                    {
                        return NotFound(new { message = "No funciono." });
                    }
                }
            }


        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("{IdUsuario}")]
        public IActionResult DeleteOrder(int IdUsuario)
        {
                // Cadena de conexión obtenida del constructor a través de IConfiguration
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();

                string query = "DELETE FROM Usuario WHERE IdUsuario = @IdUsuario";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdUsuario", IdUsuario);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Usuario eliminado exitosamente." });
                    }
                    else
                    {
                        return NotFound(new { message = "No funciono." });
                    }
                }
            }
        }
    }
}
