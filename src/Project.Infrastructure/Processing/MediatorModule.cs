using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Project.Domain.SeedWork;

namespace Project.Infrastructure.Processing
{
    public static class MediatorModule
    {
        public static IServiceCollection AddMediatorModule(this IServiceCollection services, params object[] args)
        {
            services.AddScoped<IMediator, Mediator>();
            services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();

            var assemblies = ResolveAssemblies(args);
            services.AddScoped<IMediator, Mediator>();
            RegisterHandlers(services, assemblies, typeof(INotificationHandler<>));
            RegisterHandlers(services, assemblies, typeof(IRequestHandler<,>));

            return services;
        }

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

            throw new ArgumentException("Invalid Parameters for AddMediator().");
        }

        public static void RegisterHandlers(IServiceCollection services, Assembly[] assemblies, Type handlerInterface)
        {
            var types = assemblies.SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces()
                    .Where(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == handlerInterface
                    );

                foreach (var intrfc in interfaces)
                {
                    services.AddTransient(intrfc, type);
                }
            }
        }
    }
}