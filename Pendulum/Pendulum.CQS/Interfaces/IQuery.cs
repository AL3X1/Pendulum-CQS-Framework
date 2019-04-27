using System.Threading.Tasks;

namespace Pendulum.CQS.Interfaces
{
    public interface IQuery<in TContext, TReturnable>
    {
        Task<TReturnable> QueryAsync(TContext context);
    }
}