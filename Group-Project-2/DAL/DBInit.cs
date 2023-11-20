
using Group_Project_2.Models;

namespace Group_Project_2.DAL
{
    public static class DBInit
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            ItemDbContext context = serviceScope.ServiceProvider.GetRequiredService<ItemDbContext>();
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!context.Items.Any())
            {
                var items = new List<Item>
                {
                    new Item
                    {
                        Name = "Pizza",
                        Price = 150,
                        Description = "Delicious Italian dish with a thin crust topped with tomato sauce, cheese, and various toppings.",
                        ImageUrl = "assets/images/pizza.jpg"
                    },
                    new Item
                    {
                        Name = "Fried Chicken Leg",
                        Price = 20,
                        Description = "Crispy and succulent chicken leg that is deep-fried to perfection, often served as a popular fast food item.",
                        ImageUrl = "assets/images/chickenleg.jpg"
                    },
                    new Item
                    {
                        Name = "French Fries",
                        Price = 50,
                        Description = "Crispy, golden-brown potato slices seasoned with salt and often served as a popular side dish or snack.",
                        ImageUrl = "assets/images/frenchfries.jpg"
                    },
                    new Item
                    {
                        Name = "Grilled Ribs",
                        Price = 250,
                        Description = "Tender and flavorful ribs grilled to perfection, usually served with barbecue sauce.",
                        ImageUrl = "assets/images/ribs.jpg"
                    },
                    new Item
                    {
                        Name = "Tacos",
                        Price = 150,
                        Description = "Tortillas filled with various ingredients such as seasoned meat, vegetables, and salsa, folded into a delicious handheld meal.",
                        ImageUrl = "assets/images/tacos.jpg"
                    },
                    new Item
                    {
                        Name = "Fish and Chips",
                        Price = 180,
                        Description = "Classic British dish featuring battered and deep-fried fish served with thick-cut fried potatoes.",
                        ImageUrl = "assets/images/fishandchips.jpg"
                    },
                    new Item
                    {
                        Name = "Cider",
                        Price = 50,
                        Description = "Refreshing alcoholic beverage made from fermented apple juice, available in various flavors.",
                        ImageUrl = "assets/images/cider.jpg"
                    },
                    new Item
                    {
                        Name = "Coke",
                        Price = 30,
                        Description = "Popular carbonated soft drink known for its sweet and refreshing taste.",
                        ImageUrl = "assets/images/coke.jpg"
                    },
                };
                context.AddRange(items);
                context.SaveChanges();
            }

        }
    }
}
