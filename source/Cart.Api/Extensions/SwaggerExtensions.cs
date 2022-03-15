using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Cart.Api.Extensions;

public static  class SwaggerExtensions
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var apiAssembly = typeof(Program).Assembly;
            var xmlComments = Path.Combine(AppContext.BaseDirectory, $"{apiAssembly.GetName().Name}.xml");
            if (File.Exists(xmlComments)) options.IncludeXmlComments(xmlComments);

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Cart API",
                Description = "Simple cart service.",
                Contact = new OpenApiContact
                {
                    Name = "Mustafa Çiçek",
                    Url = new Uri("https://github.com/mstfcck")
                }
            });
        });
    }
}