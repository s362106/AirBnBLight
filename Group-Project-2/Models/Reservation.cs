using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Group_Project_2.Models;

	public class Reservation
	{
    [JsonPropertyName("ReservationId")]
    public int ReservationId { get; set; }
    [JsonPropertyName("CheckInDate")]
    public DateTime CheckInDate { get; set; }
    [JsonPropertyName("CheckOutDate")]
    public DateTime CheckOutDate { get; set; }
    [JsonPropertyName("TotalPrice")]
    public decimal TotalPrice { get; set; }
    [JsonPropertyName("BookingDuration")]
    public int BookingDuration { get; set; }
    [JsonPropertyName("DateCreated")]
    public DateTime DateCreated { get; set; }

    // Navigation Property
    [JsonPropertyName("HouseId")]
    public int HouseId { get; set; }
    public virtual House? House { get; set; } = default!;

    /*
    // Navigation Property
    public string UserId { get; set; } = string.Empty;
    public virtual User? User { get; set; } = default!;
    */
}
