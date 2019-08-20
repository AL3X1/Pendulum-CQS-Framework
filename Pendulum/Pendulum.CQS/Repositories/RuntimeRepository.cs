using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pendulum.CQS.Interfaces;

namespace Pendulum.CQS.Repositories
{
    public class RuntimeRepository : IRuntimeRepository
    {
        private readonly DbContext _dbContext;

        public RuntimeRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public DbSet<TEntity> Query<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}