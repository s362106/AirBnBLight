using Group_Project_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Group_Project_2.ViewModels;

namespace Group_Project_2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<User> userManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Table()
        {
            var users = await _userManager.Users.ToListAsync();
            if(users == null)
            {
                _logger.LogError("[UserController] User list not found while executing _userManager.Users.ToListAsync()");
                return NotFound("User list not found");
            }

            var usersWithRoles = new List<UserWithRolesViewModel>();

            foreach(var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add(new UserWithRolesViewModel
                {
                    User = user,
                    Roles = roles
                });
            }
            return View(usersWithRoles);
        }
    }
}
