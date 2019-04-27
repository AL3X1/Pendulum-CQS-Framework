using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pendulum.CQS.Builders;
using Pendulum.CQS.Extensions.Repository;
using Pendulum.CQS.Interfaces;

namespace Pendulum.CQS.Common
{                    
    public class Container
    {
        private IServiceProvider _serviceProvider;

        private IServiceCollection _services;

        public Container(IServiceCollection serviceCollection)
        {
            _services = serviceCollection;
        }
        
        public void Populate(Assembly assembly)
        {
            List<Type> commandTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>)))
                .ToList();

            List<Type> queryTypes = assembly.GetTypes()
                .Where(t =>
                    t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<,>)))
                .ToList();

            foreach (Type commandType in commandTypes)
            {
                foreach (Type implementedInterface in commandType.GetInterfaces())
                {
                    _services.AddTransient(implementedInterface, commandType);
                }
            }

            foreach (Type queryType in queryTypes)
            {
                foreach (Type implementedInterface in queryType.GetInterfaces())
                {
                    _services.AddTransient(implementedInterface, queryType);
                }
            }
            
            _serviceProvider = _services.BuildServiceProvider();
            
            _services.AddSingleton(_serviceProvider);
            _services.AddSingleton<ICommandBuilder>(new CommandBuilder(_serviceProvider));
            _services.AddSingleton<IQueryBuilder>(new QueryBuilder(_serviceProvider));
        }

        public void Populate<TDbContext>(Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            IncludeDbContext(typeof(TDbContext), lifetime);
            Populate(assembly);
        }

        private void IncludeDbContext(Type contextType, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            Type dbContextType = typeof(DbContext);
            
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    _services?.AddScoped(dbContextType, contextType);                    
                    _services?.AddScoped<IRuntimeRepository, RuntimeRepository>();
                    break;

                case ServiceLifetime.Singleton:
                    _services?.AddSingleton(dbContextType, contextType);
                    _services?.AddSingleton<IRuntimeRepository, RuntimeRepository>();
                    break;
                case ServiceLifetime.Transient:
                    _services?.AddTransient(dbContextType, contextType);
                    _services?.AddTransient<IRuntimeRepository, RuntimeRepository>();
                    break;
            }
        }
    }
}