using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pendulum.CQS.Extensions.Repository
{
    public interface IRuntimeRepository
    {
        DbSet<TEntity> Query<TEntity>() where TEntity : class;

        Task SaveAsync();

        void Save();
    }
}