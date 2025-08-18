using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Project.Application.Configuration.Commands;
using Project.Application.Configuration.Commands.Behaviors;
using Project.Application.Configuration.Queries.Behaviors;
using Project.Application.Configuration.Validation;
using Project.Domain.SeedWork;
using Project.Infrastructure.Logging;

namespace Project.Infrastructure.Processing
{
    /// <summary>
    /// Provides methods to configure the mediator module, including command/query handlers and pipeline behaviors.
    /// </summary>
    public static class MediatorModule
    {
        /// <summary>
        /// Registers MediatR-like services and command/query processing pipeline.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="args">Assemblies or assembly name prefixes to scan for handlers.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddMediatorModule(this IServiceCollection services, params object[] args)
        {
            services.AddScoped<IMediator, Mediator>();
            services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();

            var assemblies = ResolveAssemblies(args);
            RegisterHandlers(services, assemblies, typeof(INotificationHandler<>));
            RegisterHandlers(services, assemblies, typeof(IRequestHandler<,>));

            services.AddScoped<CommandsExecutor>();
            services.AddScoped<QueriesExecutor>();

            services.AddScoped(typeof(IRequestPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped(typeof(ICommandPipelineBehaviour<,>), typeof(UserInjectionCommandBehavior<,>));
            services.AddScoped(typeof(IRequestPipelineBehavior<,>), typeof(UserInjectionQueryBehavior<,>));
            services.AddScoped(typeof(IRequestPipelineBehavior<,>), typeof(UnitOfWorkPipelineBehavior<,>));
            services.AddScoped(typeof(IRequestPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

            return services;
        }

        /// <summary>
        /// Resolves assemblies from input parameters to be scanned for handler types.
        /// </summary>
        /// <param name="args">Can be an array of assemblies or string prefixes.</param>
        /// <returns>An array of resolved assemblies.</returns>
        public static Assembly[] ResolveAssemblies(object[] args)
        {
            if (args == null || args.Length == 0)
                return AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.FullName))
                    .ToArray();

            if (args.All(a => a is Assembly))
                return args.Cast<Assembly>().ToArray();

            if (args.All(a => a is string))
            {
                var prefixes = args.Cast<string>().ToArray();

                return AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(a =>
                        !a.IsDynamic &&
                        !string.IsNullOrWhiteSpace(a.FullName) &&
                        prefixes.Any(p => a.FullName!.StartsWith(p))
                    ).ToArray();
            }

            throw new ArgumentException("Invalid Parameters for AddMediatorModule.");
        }

        /// <summary>
        /// Registers all types implementing a specific handler interface (e.g., IRequestHandler, INotificationHandler).
        /// </summary>
        public static void RegisterHandlers(IServiceCollection services, Assembly[] assemblies, Type handlerInterface)
        {
            var types = assemblies.SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface);

                foreach (var intrfc in interfaces)
                {
                    services.AddTransient(intrfc, type);
                }
            }
        }
    }
}
