using System.ComponentModel.DataAnnotations;

namespace Group_Project_2.Models;

public class House
{
	public int HouseId { get; set; }

    [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,50}", ErrorMessage = "The Title must be numbers or letters and between 2 to 50 characters.")]
    public string Title { get; set; } = string.Empty;

    [MinLength(50, ErrorMessage = "The description must be at least 50 characters long.")]
    public string Description { get; set; } = string.Empty;

    [RegularExpression(@"[https?://\S+|www\.\S+]{10,300}", ErrorMessage = "The Image-URL must be a link and be between 10 to 300 characters.")]
    [Display(Name = "House Image-URL")]
    public string HouseImageUrl { get; set; } = string.Empty;

    [Display(Name = "Bedroom Image-URL")]
    public string? BedroomImageUrl { get; set; }

    [Display(Name = "Bathroom Image-URL")]
    public string? BathroomImageUrl { get; set; }

    [RegularExpression(@"^[A-ZÆØÅ][a-zAæøå]{1,25},\s[A-ZÆØÅ][a-zA-Zæøå]{1,25}$", ErrorMessage = "E.g. 'Oslo, Norway'")]
    public string Location { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "The price must be greater than 0.")]
    [Display(Name = "Price Per Night")]
    public decimal PricePerNight { get; set; }

    [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "The value must be a whole number and at least 1.")]
    public int Bedrooms { get; set; }

    [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "The value must be a whole number and at least 1.")]
    public int Bathrooms { get; set; }

	public string UserId { get; set; } = string.Empty;
    public virtual User? User { get; set; } = default!;

	public virtual List<Reservation>? Reservations { get; set; }
}
