using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

using Group_Project_2.Models;
using Group_Project_2.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using Group_Project_2.DAL;
namespace Group_Project_2.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<UserController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserController(UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor,
        ILogger<UserController> logger,
        SignInManager<User> signInManager,
        IUserRepository userRepository)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _signInManager = signInManager;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            var user = await _userRepository.GetUserByEmail(model.Email);
            if (user == null)
            {
                _logger.LogError("[UserController] User not found in list");
                return Unauthorized();
            }

            //var response = new { success = false, message = "Invalid email or Password" };


            var result = await _signInManager.PasswordSignInAsync(model.Email, model.ProvidedPassword, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var role = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email)
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(claimsIdentity);

                if (_httpContextAccessor.HttpContext != null)
                {
                    await _httpContextAccessor.HttpContext
                    .SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    var response = new { success = true, message = "Logged in successfully", userEmail = user.Email };
                    return Ok(response);
                }
                return BadRequest(new { success = false, message = "Login failure" });
            }
            return Unauthorized(new { success = false, message = "Email or Password incorrect" });
        }

        catch (Exception e)
        {
            _logger.LogError("[UserController] Sign in failed for {UserEmail} {Password}. Error: {Error}", model.Email, model.ProvidedPassword, e.Message);
            return BadRequest("Invalid username or password");
        }
    }

    [HttpGet("isloggedin")]
    [Authorize]
    public IActionResult IsCheckedIn()
    {
        if (User.Identity.IsAuthenticated)
        {
            return Ok("User is authenticated");
        }
        else
        {
            return Unauthorized("User is not authenticated");
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        if (User?.Identity?.IsAuthenticated == true && _httpContextAccessor.HttpContext != null)
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("[UserController] User logged out at {Time}", DateTime.UtcNow);

            var appCookies = Request.Cookies.Keys;

            foreach (var cookie in appCookies)
            {
                Response.Cookies.Delete(cookie);
            }
            return Ok(new { success = true, message = "Logout successfull" });
        }
        return BadRequest(new { success = false, message = "User not logged out" });
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            if (model == null)
            {
                _logger.LogError("[UserController] Invalid registration request. Model is null.");
                return BadRequest(new { Success = false, Message = "Invalid registration request." });
            }

            var userExists = await _userManager.FindByEmailAsync(model.Email);

            if (userExists != null)
            {
                _logger.LogError("[UserController] User with email {Email} already exists.", model.Email);

                var response = new { success = false, message = "Email already in use" };
                return BadRequest(response);
            }

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                NormalizedUserName = model.Email.ToUpper()
            };

            var result = await _userRepository.CreateUser(user, model.Password);

            if (!result)
            {
                _logger.LogError("[UserController] User registration failed");
                return BadRequest(new { success = false, message = "An unexpected error occurred during registration. Please try again later." });
            }

            _logger.LogInformation("[UserController] User registered successfully. Email: {Email}", user.Email);

            return Ok(new { success = true, message = "User successfully registrered" });
        }

        catch (Exception e)
        {
            _logger.LogError("[UserController] An unexpected error occurred during registration. Error: {Error}", e.Message);
            return StatusCode(500, new { success = false, message = "An unexpected error occurred during registration. Please try again later." });
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<string>> GetMyDetails()
    {
        try
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

            var user = await _userRepository.GetUserByEmail(userEmail);

            if (user == null)
            {
                _logger.LogError("[UserController] User not found in database");
                return NotFound("User not found");
            }

            var userDetails = new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Id = user.Id
            };


            return Ok(userDetails);
        }
        catch (Exception e)
        {
            _logger.LogError("[ErrorController] Error retreving user details. Error: {Error}", e.Message);

            return StatusCode(500, new { success = false, message = "An unexpected error occurred while fetching user details." });
        }
    }
}
