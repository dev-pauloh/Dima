using Dima.Api.Data;
using Dima.Api.Endpoints;
using Dima.Api.Handler;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var cnnStr = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(cnnStr);
});

builder.Services.AddEndpointsApiExplorer(); //Habilita o uso do swagger / Mapeia os endpoints da api
builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(n => n.FullName);
}); //Cria uma interface visual para a api / Pega o nome completo das classes para evitar conflitos / Full Qualified Name

// registra o handler como serviço
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();

var app = builder.Build();

app.UseSwagger(); //Habilita o swagger
app.UseSwaggerUI(); //Habilita a interface visual do swagger

app.MapGet("/", () => new { Message = "OK" }); //Endpoint de teste para verificar se a api está rodando
app.MapEndpoints();

app.Run();

