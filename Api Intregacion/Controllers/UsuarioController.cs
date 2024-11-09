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
                                Contrase�a = reader.GetString(reader.GetOrdinal("Contrase�a")),
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
                                Contrase�a = reader.GetString(reader.GetOrdinal("Contrase�a")),
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
                INSERT INTO Usuario (Nombre, Apellido, Correo, Contrase�a, Activo)
                VALUES (@Nombre, @Apellido, @Correo, @Contrase�a, @Activo);
                ";
                using (SqlCommand command = new(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", newUser.Nombre);
                    command.Parameters.AddWithValue("@Apellido", newUser.Apellido);
                    command.Parameters.AddWithValue("@Correo", newUser.Correo);
                    command.Parameters.AddWithValue("@Contrase�a", newUser.Contrase�a);
                    command.Parameters.AddWithValue("@Activo", newUser.Activo);

                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificar si la inserci�n fue exitosa
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
                // Cadena de conexi�n obtenida del constructor a trav�s de IConfiguration
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();

                string query = @"UPDATE Usuario
                            SET Nombre = @Nombre, 
                                Apellido = @Apellido, 
                                Correo = @Correo, 
                                Contrase�a = @Contrase�a, 
                                Activo = @Activo
                                WHERE IdUsuario = @IdUsuario";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Asignar valores a los par�metros.
                    command.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                    command.Parameters.AddWithValue("@Nombre", user.Nombre);
                    command.Parameters.AddWithValue("@Apellido", user.Apellido ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Correo", user.Correo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Contrase�a", user.Contrase�a ?? (object)DBNull.Value);
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
                // Cadena de conexi�n obtenida del constructor a trav�s de IConfiguration
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
