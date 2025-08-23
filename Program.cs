using ProjetoAvanadeApi.Domain.DTOS;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Bem vindo!");

LoginDTO loginDTO = new LoginDTO();

app.MapPost("/login", (LoginDTO loginDTO) =>
{
    if (loginDTO.email == "useradm@teste.com" && loginDTO.senha == "123456")
    {
        return Results.Ok("Login realizado com sucesso!");
    }
    return Results.Unauthorized();
});

app.Run();
