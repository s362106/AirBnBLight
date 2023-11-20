using Microsoft.AspNetCore.Mvc.Rendering;
using Group_Project_2.Models;

namespace Group_Project_2.ViewModels
{
    public class CreateReservationViewModel
    {
        public Reservation Reservation { get; set; } = default!;
        public List<SelectListItem> HouseSelectList { get; set; } = default!;
        public List<SelectListItem> UserSelectList { get; set; } = default!;

    }
}
