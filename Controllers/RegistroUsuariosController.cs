using System.Text.Json;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Mvc;
using frontendnet.Extensions;

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

        if (!ModelState.IsValid) return View("Index", itemToCreate);

        var response = await usuario.PostAsync(itemToCreate);
        
        if(response.IsSuccessStatusCode)
        {
            ViewBag.MensajeModal = "Registro de cuenta exitoso";
            return RedirectToAction("Index", "Auth");
        }

        await ModelState.ProcesarErroresDeApi(response);
        return View("Index", itemToCreate);
    }
}