using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Data;

public interface IDataContext
{
    // Data bases:
    public DbSet<User> Users { get; set; }

    public DbSet<Log> Logs { get; set; }

    /// <summary>
    /// Get a list of items
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class;

    /// <summary>
    /// Create a new item
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    //TEntity Create<TEntity>(TEntity entity) where TEntity : class;
    Task<TEntity> CreateAsync<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Update an existing item matching the ID
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    //TEntity Update<TEntity>(TEntity entity) where TEntity : class;
    Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class;

    //void Delete<TEntity>(TEntity entity) where TEntity : class;
    Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class;
}
