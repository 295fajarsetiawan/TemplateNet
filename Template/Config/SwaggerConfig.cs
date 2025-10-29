using Microsoft.OpenApi.Models;

namespace Template.Config;

public static class SwaggerConfig
{
    public static void RegisterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(opciones =>
        {
            opciones.SwaggerDoc("V1", new OpenApiInfo
            {
                Version = "Swager Open Api",
                Title = "Swager",
                Description = "Dokumentasi APi Swager",
            });
            opciones.CustomSchemaIds(tipe => tipe.ToString());

            opciones.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "Header Otorisasi JWT menggunakan skema Bearer. \r\n\r\n Masukkan 'Bearer' [spasi] dan token Anda di kolom input teks di bawah.\r\n\r\nContoh: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            opciones.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });
        services.AddSwaggerGenNewtonsoftSupport();
    }
}