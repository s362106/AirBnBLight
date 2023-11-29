using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Group_Project_2.Models;

public class House
{
    [JsonPropertyName("HouseId")]
    public int HouseId { get; set; }

    [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,50}", ErrorMessage = "The Title must be numbers or letters and between 2 to 50 characters.")]
    [JsonPropertyName("Title")]
    public string Title { get; set; } = string.Empty;

    [MinLength(50, ErrorMessage = "The description must be at least 50 characters long.")]
    [JsonPropertyName("Description")]
    public string Description { get; set; } = string.Empty;

    [RegularExpression(@"[https?://\S+|www\.\S+]{10,300}", ErrorMessage = "The Image-URL must be a link and be between 10 to 300 characters.")]
    [JsonPropertyName("HouseImageUrl")]
    public string HouseImageUrl { get; set; } = string.Empty;

    [JsonPropertyName("BedroomImageUrl")]
    public string? BedroomImageUrl { get; set; }

    [JsonPropertyName("BathroomImageUrl")]
    public string? BathroomImageUrl { get; set; }

    [RegularExpression(@"^[A-ZÆØÅ][a-zAæøå]{1,25},\s[A-ZÆØÅ][a-zA-Zæøå]{1,25}$", ErrorMessage = "E.g. 'Oslo, Norway'")]
    [JsonPropertyName("Location")]
    public string Location { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "The price must be greater than 0.")]
    [JsonPropertyName("PricePerNight")]
    public decimal PricePerNight { get; set; }

    [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "The value must be a whole number and at least 1.")]
    [JsonPropertyName("Bedrooms")]
    public int Bedrooms { get; set; }

    [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "The value must be a whole number and at least 1.")]
    [JsonPropertyName("Bathrooms")]
    public int Bathrooms { get; set; }

    public string UserId { get; set; } = string.Empty;
    public virtual User? User { get; set; } = default!;

    public virtual List<Reservation>? Reservations { get; set; }
}
