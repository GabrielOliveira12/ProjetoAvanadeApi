using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjetoAvanadeApi.Api.Domain.Services;
using ProjetoAvanadeApi.Domain.DTOS;
using ProjetoAvanadeApi.Domain.Entities;
using ProjetoAvanadeApi.Domain.ModelViews;
using ProjetoAvanadeApi.Domain.Services;
using ProjetoAvanadeApi.Infrasctructure.Db;
using ProjetoAvanadeApi.Infrasctructure.Interfaces;

public class Program
{
    public static void Main(string[] args)
    {

#region Builder
var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Jwt").ToString();
if(string.IsNullOrEmpty(key)) key = "123456";

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdminServices, AdminService>();
builder.Services.AddScoped<IVehicleService, VehiclesService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {

        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddDbContext<Db>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("mysql"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql")));
});

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
#endregion

#region Administradores
string gerarTokenJwt(Admin admin)
{
    if (string.IsNullOrEmpty(key)) return string.Empty;

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>()
    {
        new Claim(ClaimTypes.Email, admin.Email),
        new Claim("perfil", admin.Perfil.ToString()),
        new Claim(ClaimTypes.Role, admin.Perfil.ToString())
    };

    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials

    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

LoginDTO loginDTO = new LoginDTO();

app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdminServices adminServices) =>
{
    var admin = adminServices.Login(loginDTO);

    if (admin != null)
    {
        string token = gerarTokenJwt(admin);
        return Results.Ok(new AdmLogado
        {
            Email = admin.Email,
            Perfil = admin.Perfil.ToString(),
            Token = token
        });
    }
    return Results.Unauthorized();
}).AllowAnonymous().WithTags("Administradores");

app.MapPost("/administradores", ([FromBody] AdminDTO adminDTO, IAdminServices adminServices) =>
{
    adminServices.Criar(adminDTO);

    return Results.Created($"/administradores/{adminDTO.Email}", adminDTO);

})
.RequireAuthorization(new AuthorizeAttribute{ Roles = "adm"})
.RequireAuthorization()
.WithTags("Administradores");

app.MapGet("/administradores", ([FromQuery] int? pagina, IAdminServices adminServices) =>
{
    var admins = adminServices.Todos(pagina ?? 1);
    return Results.Ok(admins);

})
.RequireAuthorization(new AuthorizeAttribute{ Roles = "adm"})
.RequireAuthorization()
.WithTags("Administradores");

app.MapGet("/administradores/{id}", ([FromRoute] int id, IAdminServices adminServices) =>
{
    var admin = adminServices.BuscarPorId(id);
    if (admin == null)
    {
        return Results.NotFound("Administrador não encontrado.");
    }
    return Results.Ok(admin);
})
.RequireAuthorization(new AuthorizeAttribute{ Roles = "adm"})
.RequireAuthorization()
.WithTags("Administradores");

app.MapPut("/administradores/{Id}", ([FromRoute] int Id, AdminDTO adminDTO, IAdminServices adminServices) =>
{
    var admin = adminServices.BuscarPorId(Id);
    if (admin == null)
        return Results.NotFound("Administrador não encontrado.");

    admin.Email = adminDTO.Email;
    admin.Perfil = adminDTO.Perfil;
    admin.Senha = adminDTO.Senha;

    adminServices.Atualizar(admin);
    return Results.Ok(admin);

})
.RequireAuthorization(new AuthorizeAttribute{ Roles = "adm"})
.RequireAuthorization()
.WithTags("Administradores");

app.MapDelete("/administradores/{Id}", ([FromRoute] int Id, IAdminServices adminServices) =>
{
    var admin = adminServices.BuscarPorId(Id);
    if (admin == null)
        return Results.NotFound("Administrador não encontrado.");

    adminServices.Deletar(admin);
    return Results.Ok(admin);   
    
}).RequireAuthorization().WithTags("Administradores");
#endregion

#region Veiculos
ErrorsValidation validaDTO(VehicleDTO vehicleDTO)
{
    var errors = new ErrorsValidation();

    if (string.IsNullOrEmpty(vehicleDTO.Nome))
        errors.Messages.Add("O nome do veículo é obrigatório.");
    if (string.IsNullOrEmpty(vehicleDTO.Marca))
        errors.Messages.Add("A marca do veículo é obrigatória.");
    if (vehicleDTO.Ano <= 0)
        errors.Messages.Add("O ano do veículo é inválido.");
    if (vehicleDTO.Ano <= 1950)
        errors.Messages.Add("O ano do veículo é muito antigo.");

    return errors;
}

app.MapPost("/veiculos", ([FromBody] VehicleDTO vehicleDTO, IVehicleService veiculoServices) =>
{
    var errors = validaDTO(vehicleDTO);
    if(errors.Messages.Count > 0) return Results.BadRequest(errors);

    var vehicle = new Vehicle
    {
        Nome = vehicleDTO.Nome,
        Marca = vehicleDTO.Marca,
        Ano = vehicleDTO.Ano
    };

    veiculoServices.Criar(vehicle);
    return Results.Created($"/veiculos/{vehicle.Id}", vehicle);
})
.RequireAuthorization(new AuthorizeAttribute{ Roles = "adm, user"})
.RequireAuthorization()
.WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int? pagina, IVehicleService veiculoServices) =>
{
    var vehicles = veiculoServices.Todos(pagina ?? 1);
    return Results.Ok(vehicles);
})
.RequireAuthorization(new AuthorizeAttribute{ Roles = "adm, user"})
.RequireAuthorization()
.WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVehicleService veiculoServices) =>
{
    var vehicle = veiculoServices.BuscarPorId(id);
    if (vehicle == null)
    {
        return Results.NotFound("Veículo não encontrado.");
    }
    return Results.Ok(vehicle);
})
.RequireAuthorization(new AuthorizeAttribute{ Roles = "adm, user"})
.RequireAuthorization()
.WithTags("Veiculos");

app.MapPut("/veiculos/{id}", ([FromRoute] int id, VehicleDTO vehicleDTO, IVehicleService veiculoServices) =>
{
    var vehicle = veiculoServices.BuscarPorId(id);
    var errors = validaDTO(vehicleDTO);

    if (vehicle == null)
        return Results.NotFound("Veículo não encontrado.");

    if (errors.Messages.Count > 0)
        return Results.BadRequest(errors);

    vehicle.Nome = vehicleDTO.Nome;
    vehicle.Marca = vehicleDTO.Marca;
    vehicle.Ano = vehicleDTO.Ano;

    veiculoServices.Atualizar(vehicle);
    return Results.Ok(vehicle);
})
.RequireAuthorization(new AuthorizeAttribute{ Roles = "adm"})
.RequireAuthorization()
.WithTags("Veiculos");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVehicleService veiculoServices) =>
{
    var vehicle = veiculoServices.BuscarPorId(id);
    if (vehicle == null)
    {
        return Results.NotFound("Veículo não encontrado.");
    }

    veiculoServices.Deletar(vehicle);
    return Results.Ok("Veículo deletado com sucesso.");
})
.RequireAuthorization(new AuthorizeAttribute{ Roles = "adm"})
.RequireAuthorization()
.WithTags("Veiculos");


#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion
    }
}