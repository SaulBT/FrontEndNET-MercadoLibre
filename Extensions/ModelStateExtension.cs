using frontendnet.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Text.Json;

namespace frontendnet.Extensions;

public static class ModelStateExtensions
{
    public static async Task ProcesarErroresDeApi(this ModelStateDictionary modelState, HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;

        if(response.StatusCode == HttpStatusCode.BadRequest)
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
                            modelState.AddModelError(error.path, error.msg);
                        }
                    }
                    else
                    {
                        modelState.AddModelError("", $"Error en los datos enviados.");
                    }
                }
                catch (JsonException)
                {
                    modelState.AddModelError("", "Error procesando la respuesta del servidor.");
                }
            }
            else
            {
                modelState.AddModelError("", $"Error del servidor: {response.StatusCode}");
            }
    }
}