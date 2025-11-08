using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Repository.Interfaces;

namespace UserManagement.Repository.Implementations;
public class UserRepository : IUserRepository
{
    private readonly IDataContext _dataContext;

    public UserRepository(IDataContext dataContext) => _dataContext = dataContext;

    public async Task<List<User>> GetAllAsync()
    {
        return await _dataContext.GetAllAsync<User>();
    }

    public async Task<List<User>> GetByActiveStatusAsync(bool isActive)
    {
        var users = await _dataContext.GetAllAsync<User>();
        return users.Where(user => user.IsActive == isActive).ToList();
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        var users = await _dataContext.GetAllAsync<User>();
        return users.FirstOrDefault(user => user.Id == id);
    }

    public async Task<User> CreateAsync(User user)
    {
        return await _dataContext.CreateAsync(user);
    }

    public async Task<User> UpdateAsync(User user)
    {
        return await _dataContext.UpdateAsync(user);
    }

    public async Task<User?> DeleteAsync(long id)
    {
        var userToDelete = await GetByIdAsync(id);

        if (userToDelete == null)
        {
            return null;
        }

        await _dataContext.DeleteAsync(userToDelete);
        return userToDelete;
    }
}

