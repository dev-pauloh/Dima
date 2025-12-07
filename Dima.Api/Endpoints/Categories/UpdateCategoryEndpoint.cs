using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Categories;

public class UpdateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id}", HandleAsync)
            .WithName("Categories: Update")
            .WithSummary("Atualiza uma categoria")
            .WithDescription("Atualiza uma categoria no sistema")
            .WithOrder(2)
            .Produces<Response<Category?>>();
    
    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ICategoryHandler handler, 
        [FromBody]UpdateCategoryRequest request, 
        [FromRoute]long id)
    {
        
        if (id <= 0)
            return TypedResults.BadRequest(new { Message = "Id inválido" });
        
        request.UserId = user.Identity?.Name ?? string.Empty;
        request.Id = id;
        var result = await handler.UpdateAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result.Data)
            : TypedResults.BadRequest(result.Data);
    }
}