using Microsoft.AspNetCore.Mvc;
using WebApplication2.Database;
using WebApplication2.Models;

namespace WebApplication2.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly UsersTable _usersTable;

    public UserController(UsersTable usersTable)
    {
        _usersTable = usersTable;
    }

    [HttpGet]
    [Route("emails")]
    public async Task<IEnumerable<UserEmail>> GetAllEmailsAsync()
    {
        await Task.Delay(2000);

        return new List<UserEmail>
        {
            new UserEmail
            {
                Email = "test@mail.ru"
            }
        };
    }

    [HttpGet]
    [Route("get-by-id")]
    public User GetUserById(int userId)
    {
        User? findedUser = _usersTable.GetUser(userId);

        return findedUser;
    }

    [HttpGet]
    [Route("check-username")]
    public bool CheckUserName(string username)
    {
        var status = _usersTable.CheckUserName(username);

        return status;
    }

    [HttpPost]
    [Route("add-user")]
    public bool AddUser(string username, string email, string password)
    {
        try
        {
            _usersTable.AddUser(username, email, password);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
