using SafetyCompliance.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SafetyCompliance.Application.Abstraction.IRepository
{
    public interface IBaseRepository<T> where T : BaseEntity,new()
    {
        Task<IEnumerable<T>> GetAllByAsync();


        Task<int> AddAsync(T Modal);


        Task<int> UpdateAsync(T Modal);


        Task<T> GetbyIdAsync(Guid guid);


        Task<int> DeleteByIdAsync(Guid id);


        Task<bool> IsExist(Expression<Func<T, bool>> expression);


        Task<IEnumerable<T>> FindbyAsync(Expression<Func<T, bool>> expression);

        #region Dapper Methods
        Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string query, object? param = default, CommandType commandType = CommandType.Text, IDbTransaction transaction = null);

        Task<int> ExecuteAsync(string query, object? param = default, CommandType commandType = CommandType.Text, IDbTransaction transaction = null);

        Task<TEntity> FirstOrDefaultAsync<TEntity>(string query, object? param = default, CommandType commandType = CommandType.Text, IDbTransaction transaction = null);

        #endregion
    }
}
