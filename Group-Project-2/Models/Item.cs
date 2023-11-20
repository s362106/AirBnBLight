using System.Text.Json.Serialization;

namespace Group_Project_2.Models
{
    public class Item
    {
        [JsonPropertyName("ItemId")]
        public int ItemId { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("Price")]
        public decimal Price { get; set; }
        [JsonPropertyName("Description")]
        public string? Description { get; set; }
        [JsonPropertyName("ImageUrl")]
        public string? ImageUrl { get; set; }
    }
}
