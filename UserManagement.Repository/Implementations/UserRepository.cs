using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Repository.Interfaces;

namespace UserManagement.Repository.Implementations;
public class UserRepository : IUserRepository
{
    private readonly IDataContext _dataContext;

    public UserRepository(IDataContext dataContext) => _dataContext = dataContext;

    public IEnumerable<User> GetAll() => _dataContext.GetAll<User>();

    public IEnumerable<User> GetByActiveStatus(bool isActive) =>
        _dataContext.GetAll<User>().Where(user => user.IsActive == isActive);

    public User? GetById(long id) => _dataContext.GetAll<User>().FirstOrDefault(user => user.Id == id);
    //public void Create(User user) => _dataContext.Create(user);

    public User Create(User user) {
        return _dataContext.Create(user);
    }

    //public void Update(User user) => _dataContext.Update(user);

    public User Update(User user)
    {
        return _dataContext.Update(user);
    }

    public User? Delete(long id) {
        var userToDelete = GetById(id);

        if (userToDelete == null)
        {
            return null;
        }

        _dataContext.Delete(userToDelete);
        return userToDelete;
    }
}

