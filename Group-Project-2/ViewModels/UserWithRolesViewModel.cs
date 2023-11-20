using Group_Project_2.Models;
namespace Group_Project_2.ViewModels;

public class UserWithRolesViewModel
{
	public User User { get; set; } = default!;
	public IList<string> Roles { get; set; } = new List<string>();
}

