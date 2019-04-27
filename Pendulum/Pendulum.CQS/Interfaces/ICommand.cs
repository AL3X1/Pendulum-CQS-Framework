using System.Threading.Tasks;
using Pendulum.CQS.Contexts;
using Pendulum.CQS.Models;

namespace Pendulum.CQS.Interfaces
{
    public interface ICommand<in TContext> where TContext : CommandContext
    {
        Task ExecuteAsync(TContext context);
    }

    public interface ICommand<in TContext, TReturnable> where TContext : CommandContext
    {
        Task<TReturnable> ExecuteAsync(TContext context);
    }
}