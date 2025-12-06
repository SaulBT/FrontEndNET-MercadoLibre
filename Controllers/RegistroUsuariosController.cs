using System.Text.Json;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

public class RegistroUsuariosController(RegistroClientService usuario) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(UsuarioPwd itemToCreate)
    {
        itemToCreate.Rol = "Usuario";
        ModelState.Remove(nameof(itemToCreate.Rol));
       
        if (ModelState.IsValid)
        {
            var response = await usuario.PostAsync(itemToCreate);

            if(response.IsSuccessStatusCode)
            {
                ViewBag.MensajeModal = "Registro de cuenta exitoso";
                return RedirectToAction("Index", "Auth");
            }

            if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var contenido = await response.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                try
                {
                    var errorResponse = JsonSerializer.Deserialize<BackendErrorResponse>(contenido, opciones);

                    if (errorResponse != null && errorResponse.errors.Count > 0)
                    {
                        foreach (var error in errorResponse.errors)
                        {
                            ModelState.AddModelError(error.path, error.msg);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", $"Error en los datos enviados.");
                    }
                }
                catch (JsonException)
                {
                    ModelState.AddModelError("", "Error procesando la respuesta del servidor.");
                }
            }
            else
            {
                ModelState.AddModelError("", $"Error del servidor: {response.StatusCode}");
            }
        }
        else
        {
            ModelState.AddModelError("", "El modelo no es válido.");
        }
        
        return View("Index", itemToCreate);
    }
}