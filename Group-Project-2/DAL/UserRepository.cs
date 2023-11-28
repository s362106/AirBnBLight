using Microsoft.EntityFrameworkCore;
using Group_Project_2.Models;
using Microsoft.AspNetCore.Identity;

using Group_Project_2.ViewModels;

namespace Group_Project_2.DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(UserManager<User> userManager, ILogger<UserRepository> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IEnumerable<UserViewModel>?> GetAllUsers()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var listToReturn = users.Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email
                });
                return listToReturn;
            }

            catch (Exception e)
            {
                _logger.LogError("[UserRepository] AllUsers() failed to retreive users. Error: {e}", e.Message);
                return null;
            }
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            try
            {
                return await _userManager.FindByEmailAsync(email);
            }
            catch (Exception e)
            {
                _logger.LogError("[UserRepository] GetUserByEmail() failed to retreive user. Error: {e}", e.Message);
                return null;
            }
        }

        public async Task<bool> CreateUser(User user, string password)
        {
            var createUserResult = await _userManager.CreateAsync(user, password);

            if (!createUserResult.Succeeded)
            {
                _logger.LogError("[UserRepository] User creation failed. Error: {Error}", createUserResult.Errors);
                return false;
            }
            return true;
        }
    }
}
