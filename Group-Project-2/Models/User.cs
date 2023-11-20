using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Group_Project_2.Models;

public class User : IdentityUser
{
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;

	public virtual List<Reservation>? Reservations { get; set; }
	public virtual List<House>? Houses { get; set; }

    internal object Select(Func<object, SelectListItem> value)
    {
        throw new NotImplementedException();
    }
}