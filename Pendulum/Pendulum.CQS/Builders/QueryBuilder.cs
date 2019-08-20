using System.Threading.Tasks;
using Pendulum.CQS.Contexts;
using Pendulum.CQS.Interfaces;

namespace Pendulum.CQS.Builders
{
    public class QueryBuilder : IQueryBuilder
    {
        private static IPendulumServiceProvider serviceProvider;

        public QueryBuilder(IPendulumServiceProvider serviceProvider)
        {
            QueryBuilder.serviceProvider = serviceProvider;
        }
        
        public async Task<TReturnable> QueryAsync<TContext, TReturnable>(TContext context) where TContext : QueryContext
        {
            IQuery<TContext, TReturnable> service = serviceProvider.GetService<IQuery<TContext, TReturnable>>();
            return await service.QueryAsync(context);
        }
    }
}