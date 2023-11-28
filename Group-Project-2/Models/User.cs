using System;
using Microsoft.AspNetCore.Identity;
namespace Group_Project_2.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public virtual List<House>? Houses { get; set; }
    }
}
