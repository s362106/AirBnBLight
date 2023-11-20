using System;
using Group_Project_2.Models;
namespace Group_Project_2.ViewModels;

public class ReservationListViewModel
{
	public IEnumerable<Reservation> Reservations;
    public string? CurrentViewName;

    public ReservationListViewModel(IEnumerable<Reservation> reservations, string? currentViewName)
	{
		Reservations = reservations;
		CurrentViewName = currentViewName;
	}
}
