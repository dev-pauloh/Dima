using Dima.Api.Data;
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

var app = builder.Build();

app.UseSwagger(); //Habilita o swagger
app.UseSwaggerUI(); //Habilita a interface visual do swagger

app.MapPost("/v1/categories", async (CreateCategoryRequest request, ICategoryHandler handler)  //Dependência injetada | Recebe uma instância do CategoryHandler
        => await handler.CreateAsync(request))
        .WithName("Categories: Create")
        .WithSummary("Cria uma nova categoria")
        .Produces<Response<Category>>();

app.MapPut("/v1/categories/{id}", 
        async (long id, UpdateCategoryRequest request, ICategoryHandler handler)  
        =>
        {
            request.Id = id;
            return await handler.UpdateAsync(request);
        })
        .WithName("Categories: Update")
        .WithSummary("Atualiza categoria")
        .Produces<Response<Category?>>();

app.MapDelete("/v1/categories/{id}", 
        async (long id, ICategoryHandler handler)  
        =>
        {
            var request = new DeleteCategoryRequest
            {
                Id = id,
                UserId = "test@balta.io"
            };
            return await handler.DeleteAsync(request);
        })
        .WithName("Categories: Delete")
        .WithSummary("Exclui uma categoria")
        .Produces<Response<Category?>>();

app.MapGet("/v1/categories", 
        async (ICategoryHandler handler)  
        =>
        {
            var request = new GetAllCategoriesRequest
            {
                UserId = "test@balta.io"
            };
            return await handler.GetAllAsync(request);
        })
        .WithName("Categories: Get All")
        .WithSummary("Retorna todas as categorias de um usuário")
        .Produces<PagedResponse<List<Category>?>>();

app.MapGet("/v1/categories/{id}", 
        async (long id, ICategoryHandler handler)  
        =>
        {
            var request = new GetCategoryByIdRequest
            {
                Id = id,
                UserId = "test@balta.io"
            };
            return await handler.GetByIdAsync(request);
        })
        .WithName("Categories: Get By Id")
        .WithSummary("Busca uma categoria por ID")
        .Produces<Response<Category?>>();

app.Run();

