using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    Task<IEnumerable<User>> FilterByActiveAsync(bool isActive);
    Task<IEnumerable<User>> GetAllAsync();

    // Task 3
    Task<User?> GetByIdAsync(long id);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<User?> DeleteAsync(long id);
}
