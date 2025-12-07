using Dima.Api.Data;
using Dima.Api.Handler;
using Dima.Api.Models;
using Dima.Core;
using Dima.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Common.Api;

public static class BuilderExtension
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.ConnectionString = builder
            .Configuration
            .GetConnectionString("DefaultConnection") ?? string.Empty;
        
        Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
        Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty;
    }

    public static void AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer(); //Habilita o uso do swagger / Mapeia os endpoints da api
        builder.Services.AddSwaggerGen(x =>
        {
            x.CustomSchemaIds(n => n.FullName);
        }); //Cria uma interface visual para a api / Pega o nome completo das classes para evitar conflitos / Full Qualified Name
    }
    
    public static void AddSecurity(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddIdentityCookies(); 
        builder.Services.AddAuthorization();
    }

    public static void AddDataContexts(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(x =>
        {
            x.UseSqlServer(Configuration.ConnectionString);
        });

        // Configura o Identity para usar a classe User e IdentityRole<long>
        builder.Services
            .AddIdentityCore<User>()
            .AddRoles<IdentityRole<long>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints(); 
    }

    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(
            options => options.AddPolicy(
                ApiConfiguration.CorsPolicyName,
                policy => policy
                    .WithOrigins([
                        Configuration.FrontendUrl,
                        Configuration.BackendUrl
                    ])
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            ));
               
    }
    
    public static void AddServices(this WebApplicationBuilder builder)
    {
        // registra o handler como serviço
        builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
        builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
    }
}