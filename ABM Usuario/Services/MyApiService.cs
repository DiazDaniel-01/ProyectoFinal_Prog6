using ABM_Usuario.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ABM_Usuario.Services
{
    public class MyApiService
    {
        private readonly HttpClient _httpClient;

        public MyApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApiClient");
        }



        // Obtener lista de usuarios desde la API
        public async Task<List<UsuarioModel>> GetUsuariosAsync()
        {
            var response = await _httpClient.GetAsync("/Usuario/ListaUsuarios");  // Ruta a tu API
            response.EnsureSuccessStatusCode();  // Verifica si la respuesta es exitosa

            var usuarios = await response.Content.ReadFromJsonAsync<List<UsuarioModel>>();
            return usuarios ?? new List<UsuarioModel>();
        }

        public async Task<UsuarioModel> GetUsuarioByIdAsync(int IdUsuario)
        {
            var response = await _httpClient.GetAsync($"/Usuario/UsuarioxId/{IdUsuario}");

            if (response.IsSuccessStatusCode)
            {
                var usuario = await response.Content.ReadFromJsonAsync<UsuarioModel>();
                return usuario;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al obtener usuario: {errorContent}");
            }
        }


        // Agregar usuario mediante la API
        public async Task<string> AgregarUsuarioAsync(UsuarioModel newUser)
        {
            var response = await _httpClient.PostAsJsonAsync("/Usuario/AgregarUsuario", newUser);
            if (response.IsSuccessStatusCode)
            {
                return "Usuario agregado exitosamente.";
            }
            else
            {
                return "Error al agregar el usuario.";
            }
        }

        // Actualizar usuario mediante la API
        public async Task<string> ActualizarUsuarioAsync(int IdUsuario, UsuarioModel user)
        {
            var response = await _httpClient.PutAsJsonAsync($"/Usuario/{IdUsuario}", user);
            if (response.IsSuccessStatusCode)
            {
                return "Usuario actualizado exitosamente.";
            }
            else
            {
                return "Error al actualizar el usuario.";
            }
        }


        // Eliminar usuario mediante la API
        public async Task<string> EliminarUsuarioAsync(int IdUsuario)
        {
            var response = await _httpClient.DeleteAsync($"/Usuario/{IdUsuario}");
            if (response.IsSuccessStatusCode)
            {
                return "Usuario eliminado exitosamente.";
            }
            else
            {
                return "Error al eliminar el usuario.";
            }
        }

    }
}
