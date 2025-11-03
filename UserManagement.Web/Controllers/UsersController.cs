using System;
using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public ViewResult List()
    {
        var items = _userService.GetAll().Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }

    [HttpGet("active")]
    public ViewResult Active()
    {
        //try
        //{
        // automate this section with the select
        var items = _userService.FilterByActive(true).Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        }); ;

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };


        return View("List", model);
    }
        //}
        //// Add Expecption ex to the logger 
        // catch (Exception)
        //{
        //    return View();
        //    //return StatusCode(500, "An error has occured.");
        //}

        [HttpGet("inactive")]
        public ViewResult Inactive()
        {
            //try
            //{
            // automate this section with the select
            var items = _userService.FilterByActive(false).Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                IsActive = p.IsActive,
                DateOfBirth = p.DateOfBirth
            }); ;

            var model = new UserListViewModel
            {
                Items = items.ToList()
            };


            return View("List", model);
        }
}
