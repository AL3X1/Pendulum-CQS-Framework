using System.Threading.Tasks;
using Pendulum.CQS.Contexts;
using Pendulum.CQS.Models;

namespace Pendulum.CQS.Interfaces
{
    public interface ICommandBuilder
    {
        Task ExecuteAsync<TContext>(TContext context) where TContext : CommandContext;

        Task<TReturnable> ExecuteAsync<TContext, TReturnable>(TContext context) where TContext : CommandContext;
    }
}