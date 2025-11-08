using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Repository.Interfaces;


public interface IUserRepository
{

    Task<List<User>> GetAllAsync();
    Task<List<User>> GetByActiveStatusAsync(bool isActive);
    Task<User?> GetByIdAsync(long id);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<User?> DeleteAsync(long id);

}

