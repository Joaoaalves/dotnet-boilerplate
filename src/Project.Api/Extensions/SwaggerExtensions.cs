using Microsoft.OpenApi.Models;

namespace Project.Api.Extensions
{
    /// <summary>
    /// Provides extension methods to configure Swagger for the API.
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Adds Swagger and OpenAPI configuration to the service collection.
        /// </summary>
        /// <param name="services">The service collection to which Swagger services will be added.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance, allowing method chaining.</returns>
        /// <remarks>
        /// This method registers Swagger services including:
        /// <list type="bullet">
        /// <item>
        /// <term>Swagger generation</term>
        /// <description>Registers Swagger generators and UI endpoints.</description>
        /// </item>
        /// <item>
        /// <term>JWT Bearer authentication</term>
        /// <description>Adds security definitions and requirements for using JWT Bearer tokens in API requests.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Insert a valid JWT token",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Type = SecuritySchemeType.Http
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "BearerAuth"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }
    }
}
