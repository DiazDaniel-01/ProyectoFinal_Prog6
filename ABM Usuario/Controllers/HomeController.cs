using ABM_Usuario.Models;
using ABM_Usuario.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ABM_Usuario.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyApiService _apiService;

        public HomeController(MyApiService apiService)
        {
            _apiService = apiService;
        }

        // Mostrar lista de usuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _apiService.GetUsuariosAsync();
            return View(usuarios);
        }

        // Agregar usuario
        [HttpPost]
        public async Task<IActionResult> AgregarUsuario(UsuarioModel newUser)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, regresa a la vista con los errores
                return View("Index", await _apiService.GetUsuariosAsync());
            }

            try
            {
                var mensaje = await _apiService.AgregarUsuarioAsync(newUser);
                TempData["Mensaje"] = mensaje; // Usar TempData para mensajes
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al agregar el usuario: {ex.Message}");
                return View("Index", await _apiService.GetUsuariosAsync());
            }
        }

        // Actualizar usuario
        [HttpPost]
        public async Task<IActionResult> ActualizarUsuario(int IdUsuario, UsuarioModel user)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", await _apiService.GetUsuariosAsync());
            }

            try
            {
                var mensaje = await _apiService.ActualizarUsuarioAsync(IdUsuario, user);
                TempData["Mensaje"] = mensaje;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al actualizar el usuario: {ex.Message}");
                return View("Index", await _apiService.GetUsuariosAsync());
            }
        }


        // Eliminar usuario
        [HttpPost]
        public async Task<IActionResult> EliminarUsuario(int IdUsuario)
        {
            try
            {
                var mensaje = await _apiService.EliminarUsuarioAsync(IdUsuario);
                TempData["Mensaje"] = mensaje;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al eliminar el usuario: {ex.Message}");
                return View("Index", await _apiService.GetUsuariosAsync());
            }
        }
    }
}