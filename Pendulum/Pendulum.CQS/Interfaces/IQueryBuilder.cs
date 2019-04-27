using System.Threading.Tasks;
using Pendulum.CQS.Contexts;

namespace Pendulum.CQS.Interfaces
{
    public interface IQueryBuilder
    {
        Task<TReturnable> QueryAsync<TContext, TReturnable>(TContext context) where TContext : QueryContext;
    }
}