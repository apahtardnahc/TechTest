using System.Collections.Generic;
using UserManagement.Models;
using UserManagement.Repository.Interfaces;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    // Refactoring
    //private readonly IDataContext _dataAccess;
    private readonly IUserRepository _userRepository;
    //public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;
    public UserService(IUserRepository userRepository) => _userRepository = userRepository;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool isActive)
    {
        //throw new NotImplementedException();
        return _userRepository.GetByActiveStatus(isActive);
    }

    public IEnumerable<User> GetAll() => _userRepository.GetAll();

    // Task 3 methods
    // TODO add tests
    public User? GetById(long id) => _userRepository.GetById(id);

    public User Create(User user) => _userRepository.Create(user);

    public User Update(User user) => _userRepository.Update(user);

    public User? Delete(long id) => _userRepository.Delete(id);
}
