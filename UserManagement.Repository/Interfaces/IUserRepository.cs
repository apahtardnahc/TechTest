using System.Collections.Generic;
using UserManagement.Models;

namespace UserManagement.Repository.Interfaces;


public interface IUserRepository
{
    // Should we make these queryable?
    IEnumerable<User> GetAll();

    IEnumerable<User> GetByActiveStatus(bool isActive);

    // For task 3
    User? GetById(long id);

    // These are to be done for task 3 
    //void Create(User user);
    User Create(User user);
    User Update(User user);
    User? Delete(long id);
}

