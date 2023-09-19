using System.Text;

namespace PokeAPI.MiddleWare;

public class HashPassword
{
    private readonly RequestDelegate _next;

    public HashPassword(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Verifique se o corpo da solicitação contém uma senha a ser criptografada
        if (context.Request.Path == "/bcrypt" && context.Request.Method == "POST")
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                var requestBody = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(requestBody))
                {
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(requestBody);

                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync($"Hash da senha: {hashedPassword}");
                    return;
                }
            }

        await _next(context);
    }
}
