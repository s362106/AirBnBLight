using System.Collections.Generic;
using System.Threading.Tasks;
using Group_Project_2.Models;
using Group_Project_2.ViewModels;
namespace Group_Project_2.DAL;

public interface IUserRepository
{
    Task<IEnumerable<UserViewModel>?> GetAllUsers();
    Task<bool> CreateUser(User user, string password);
    Task<User?> GetUserByEmail(string email);
}

