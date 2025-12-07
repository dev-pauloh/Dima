using System.Security.Claims;
using Dima.Api.Data;
using Dima.Api.Endpoints;
using Dima.Api.Handler;
using Dima.Api.Models;
using Dima.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); //Habilita o uso do swagger / Mapeia os endpoints da api
builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(n => n.FullName);
}); //Cria uma interface visual para a api / Pega o nome completo das classes para evitar conflitos / Full Qualified Name

builder.Services
    .AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies(); 
builder.Services.AddAuthorization();

var cnnStr = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(cnnStr);
});

// Configura o Identity para usar a classe User e IdentityRole<long>
builder.Services
    .AddIdentityCore<User>()
    .AddRoles<IdentityRole<long>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

// registra o handler como serviço
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();

var app = builder.Build();

app.UseAuthentication(); //Habilita a autenticação
app.UseAuthorization(); //Habilita a autorização

app.UseSwagger(); //Habilita o swagger
app.UseSwaggerUI(); //Habilita a interface visual do swagger

app.MapGet("/", () => new { Message = "OK" }); //Endpoint de teste para verificar se a api está rodando
app.MapEndpoints();
app.MapGroup("v1/identity")
    .WithTags("Identity")
    .MapIdentityApi<User>();

app.MapGroup("v1/identity")
    .WithTags("Identity")
    .MapPost("/logout", async(
        SignInManager<User> signInManager) =>
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    })
    .RequireAuthorization();

app.MapGroup("v1/identity")
    .WithTags("Identity")
    .MapGet("/roles", (ClaimsPrincipal user) =>
    {
        if(user.Identity is null || !user.Identity.IsAuthenticated)
            return Results.Unauthorized();
        
        var identity = (ClaimsIdentity)user.Identity;
        var roles = identity
            .FindAll(identity.RoleClaimType)
            .Select(c => new
            {
                c.Issuer,
                c.OriginalIssuer,
                c.Type,
                c.Value,
                c.ValueType
            });
        
        return TypedResults.Json(roles);
    })
    .RequireAuthorization();

app.Run();

