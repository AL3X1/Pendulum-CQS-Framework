using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Pendulum.CQS.Contexts;
using Pendulum.CQS.Interfaces;

namespace Pendulum.CQS.Builders
{
    public class CommandBuilder : ICommandBuilder
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public async Task ExecuteAsync<TContext>(TContext context) where TContext : CommandContext
        {
            ICommand<TContext> service = _serviceProvider.GetService<ICommand<TContext>>();
            await service.ExecuteAsync(context);            
        }

        public async Task<TReturnable> ExecuteAsync<TContext, TReturnable>(TContext context) where TContext : CommandContext
        {
            ICommand<TContext, TReturnable> service = _serviceProvider.GetService<ICommand<TContext, TReturnable>>();
            return await service.ExecuteAsync(context);
        }
    }
}