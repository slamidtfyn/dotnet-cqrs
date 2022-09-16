using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Crqs.Queries;

public static class AddQueriesExtension
{
    public static IServiceCollection AddQueries(this IServiceCollection serviceCollection, Assembly queryAssembly)
    {
        var queryHandlers = queryAssembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
            );

        foreach (var handler in queryHandlers)
        {
            serviceCollection.AddScoped(
                handler.GetInterfaces().First(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)), handler);
        }

                serviceCollection.AddScoped<IQueryDispatcher, QueryDispatcher>();
        return serviceCollection;
    }
}