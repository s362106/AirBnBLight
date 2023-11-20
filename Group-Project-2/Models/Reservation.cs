using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Group_Project_2.Models;

	public class Reservation
	{
    public int ReservationId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }
    public int BookingDuration { get; set; }
    public DateTime DateCreated { get; set; }

    // Navigation Property
    public int HouseId { get; set; }
    public virtual House? House { get; set; } = default!;

    // Navigation Property
    public string UserId { get; set; } = string.Empty;
    public virtual User? User { get; set; } = default!;
}
