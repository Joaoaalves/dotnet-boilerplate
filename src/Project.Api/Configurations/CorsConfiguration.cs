namespace Project.API.Configurations
{
    /// <summary>
    /// Provides CORS configuration methods for the application.
    /// </summary>
    public static class CorsConfiguration
    {
        /// <summary>
        /// Adds CORS policies to the service collection.
        /// </summary>
        /// <param name="services">The service collection to which the CORS policies will be added.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance, enabling method chaining.</returns>
        /// <remarks>
        /// This method configures two CORS policies:
        /// <list type="bullet">
        /// <item>
        /// <term>"CorsPolicy"</term>
        /// <description>
        /// Dynamically allows origins based on environment and request origin.
        /// In development mode, it allows "localhost" and local IPs (192.168.*).
        /// In production, it allows only the origin specified in the "CLIENT_URL" environment variable.
        /// </description>
        /// </item>
        /// <item>
        /// <term>"PublicPolicy"</term>
        /// <description>Allows any origin, header, and method — intended for public access.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            var isProduction = Environment.GetEnvironmentVariable("PRODUCTION") == "true";

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .SetIsOriginAllowed(origin =>
                        {
                            if (string.IsNullOrEmpty(origin))
                                return false;

                            try
                            {
                                var uri = new Uri(origin);
                                var host = uri.Host;

                                if (!isProduction)
                                {
                                    if (host == "localhost")
                                        return true;

                                    if (System.Net.IPAddress.TryParse(host, out var ip))
                                    {
                                        if (ip.ToString().StartsWith("192.168."))
                                            return true;
                                    }
                                }

                                var clientUrl = Environment.GetEnvironmentVariable("CLIENT_URL");
                                if (!string.IsNullOrEmpty(clientUrl))
                                {
                                    var allowedHost = new Uri(clientUrl).Host;
                                    return host == allowedHost;
                                }

                                return false;
                            }
                            catch
                            {
                                return false;
                            }
                        })
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });

                options.AddPolicy("PublicPolicy", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
