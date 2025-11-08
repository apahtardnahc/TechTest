using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Repository.Interfaces;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository) => _userRepository = userRepository;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public async Task<IEnumerable<User>> FilterByActiveAsync(bool isActive)
    {
        var users = await _userRepository.GetAllAsync();
        return users.Where(user => user.IsActive == isActive);
    }

    /// <summary>
    /// Return all users
    /// </summary>
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    /// <summary>
    /// Returns a user by their specific id
    /// </summary>
    public async Task<User?> GetByIdAsync(long id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    public async Task<User> CreateAsync(User user)
    {
        return await _userRepository.CreateAsync(user);
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    public async Task<User> UpdateAsync(User user)
    {
        return await _userRepository.UpdateAsync(user);
    }

    /// <summary>
    /// Delete a user by id
    /// </summary>
    public async Task<User?> DeleteAsync(long id)
    {
        return await _userRepository.DeleteAsync(id);
    }
}
