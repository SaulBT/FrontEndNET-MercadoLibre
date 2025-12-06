using frontendnet.Models;

namespace frontendnet.Services;

public class RegistroClientService(HttpClient client)
{
    public async Task<HttpResponseMessage> PostAsync(UsuarioPwd usuario)
    {
        var response = await client.PostAsJsonAsync($"api/usuarios/registro", usuario);
        return response;
    }
}