using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pendulum.CQS.Interfaces
{
    public interface IRuntimeRepository
    {
        DbSet<TEntity> Query<TEntity>() where TEntity : class;

        Task SaveAsync();

        void Save();
    }
}