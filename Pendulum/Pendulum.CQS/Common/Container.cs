using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pendulum.CQS.Builders;
using Pendulum.CQS.Interfaces;
using Pendulum.CQS.Repositories;
using Pendulum.CQS.ServiceProviders;

namespace Pendulum.CQS.Common
{                    
    public class Container : IPendulumContainer
    {        
        private readonly IServiceCollection _services;

        public Container(IServiceCollection serviceCollection)
        {
            _services = serviceCollection;
        }

        public void Populate(Assembly assembly)
        {
            List<Type> commandTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ICommand<>)
                                                  || i.GetGenericTypeDefinition() == typeof(ICommand<,>))))
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

            PendulumServiceProvider pendulumServiceProvider =
                new PendulumServiceProvider(_services.BuildServiceProvider());

            _services.AddSingleton<IPendulumServiceProvider>(pendulumServiceProvider);
            _services.AddSingleton<ICommandBuilder>(new CommandBuilder(pendulumServiceProvider));
            _services.AddSingleton<IQueryBuilder>(new QueryBuilder(pendulumServiceProvider));
        }

        public Container IncludeDbContext<TContext>(Action<DbContextOptionsBuilder> optionsAction)
            where TContext : DbContext
        {
            _services?.AddDbContext<TContext>(optionsAction, ServiceLifetime.Transient);
            _services?.AddScoped<IRuntimeRepository, RuntimeRepository>();

            return this;
        }
    }
}