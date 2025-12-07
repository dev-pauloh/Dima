namespace Dima.Api.Common.Api;

public static class AppExtension
{
    public static void ConfigureDevEnvironment(this WebApplication app)
    {
        app.UseSwagger(); //Habilita o swagger
        app.UseSwaggerUI(); //Habilita a interface visual do swagger
        app.MapSwagger().RequireAuthorization();
    }

    public static void UseSecurity(this WebApplication app)
    {
        app.UseAuthentication(); //Habilita a autenticação
        app.UseAuthorization(); //Habilita a autorização
    }
}