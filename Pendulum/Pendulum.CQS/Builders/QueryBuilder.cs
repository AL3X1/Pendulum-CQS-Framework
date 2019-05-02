using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Pendulum.CQS.Contexts;
using Pendulum.CQS.Extensions.ServiceProvider;
using Pendulum.CQS.Interfaces;

namespace Pendulum.CQS.Builders
{
    public class QueryBuilder : IQueryBuilder
    {
        private static IPendulumServiceProvider _serviceProvider;

        public QueryBuilder(IPendulumServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public async Task<TReturnable> QueryAsync<TContext, TReturnable>(TContext context) where TContext : QueryContext
        {
            IQuery<TContext, TReturnable> service = _serviceProvider.GetService<IQuery<TContext, TReturnable>>();
            return await service.QueryAsync(context);
        }
    }
}