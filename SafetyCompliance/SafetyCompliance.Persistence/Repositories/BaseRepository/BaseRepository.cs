using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SafetyCompliance.Application.Abstraction.IRepository;
using SafetyCompliance.Domain.Entity;
using SafetyCompliance.Persistence.Data;
using System.Data;
using System.Linq.Expressions;

namespace SafetyCompliance.Persistence.Repositories.BaseRepository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        private readonly SafetyComplianceDbContext context;

        public BaseRepository(SafetyComplianceDbContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(T Modal)
        {
            context.Set<T>().Add(Modal);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteByIdAsync(Guid id)
        {
            T model = new T { Id = id};
            context.Set<T>().Remove(model);
            return context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindbyAsync(Expression<Func<T, bool>> expression)
        {
           return await context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetbyIdAsync(Guid guid)
        {
            return await context.Set<T>().FindAsync(guid);
        }

        public async Task<bool> IsExist(Expression<Func<T, bool>> expression)
        {
            return await context.Set<T>().AnyAsync(expression);
        }

        public async Task<int> UpdateAsync(T Modal)
        {
            context.Set<T>().Update(Modal);
            return await context.SaveChangesAsync();
        }

        #region Dapper Methods
        public async Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string query, object? param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            SqlConnection connection = new(context.Database.GetConnectionString());
            return await connection.QueryAsync<TEntity>(query, param, transaction, commandType: commandType);
        }
        public Task<TEntity> FirstOrDefaultAsync<TEntity>(string query, object? param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            SqlConnection connection = new(context.Database.GetConnectionString());
            return connection.QueryFirstOrDefaultAsync<TEntity>(query, param, transaction, commandType: commandType);
        }
        public Task<int> ExecuteAsync(string query, object? param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            SqlConnection connection = new(context.Database.GetConnectionString());
            return connection.ExecuteAsync(query, param, transaction, commandType: commandType);
        }
        #endregion
    }
}
