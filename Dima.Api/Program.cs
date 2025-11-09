using Dima.Api.Data;
using Dima.Core.Models;
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
builder.Services.AddTransient<Handler>();

var app = builder.Build();

app.UseSwagger(); //Habilita o swagger
app.UseSwaggerUI(); //Habilita a interface visual do swagger

app.MapPost(
        "/v1/categories", 
        (Request request, Handler handler) //Dependência injetada
            => handler.Handle(request))
    .WithName("Categories: Create")
    .WithSummary("Cria uma nova categoria")
    .Produces<Response>();

app.Run();

//Request
public class Request
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

//Response
public class Response
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
}

//Handler
public class Handler(AppDbContext context)
{
    public Response Handle(Request request)
    {
        var category = new Category
        {
            Title = request.Title,
            Description = request.Description
        };

        context.Categories.Add(category);
        context.SaveChanges();
        
        //Faz todo o processo de criação
        return new Response
        {
            Id = category.Id,
            Title = category.Title
        };
    }
}   
