using System.ComponentModel.DataAnnotations;
namespace Group_Project_2.Models;

public class RegisterModel
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "The first name is required")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
