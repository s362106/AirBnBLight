using Microsoft.AspNetCore.Identity;
using Group_Project_2.Models;
using System.Runtime.Intrinsics.X86;

namespace Group_Project_2.DAL;
public class DBInit
{

    public static async void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
        HouseDbContext context = serviceScope.ServiceProvider.GetRequiredService<HouseDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if (!context.Users.Any())
        {
            string email = "admin@admin.com";
            string password = "Admin123,";

            if (await userManager.FindByEmailAsync(email.ToUpper()) == null)
            {
                var user = new User
                {
                    Email = email,
                    FirstName = "Admin",
                    LastName = "Admin",
                    UserName = email,
                    NormalizedEmail = email.ToUpper(),
                    NormalizedUserName = email.ToUpper()
                };

                await userManager.CreateAsync(user, password);
            }

            var email1 = "john.h@gmail.com";
            var user1 = new User
            {
                FirstName = "John",
                LastName = "Hopkins",
                Email = email1,
                PhoneNumber = "12345678",
                NormalizedEmail = email1.ToUpper(),
                NormalizedUserName = email1.ToUpper(),
                UserName = email1
            };

            await userManager.CreateAsync(user1, "JohnH2000,");
            var email2 = "sarah.t@gmail.com";
            var user2 = new User
            {
                FirstName = "Sarah",
                LastName = "Tucker",
                Email = email2,
                PhoneNumber = "98765432",
                NormalizedEmail = email2.ToUpper(),
                NormalizedUserName = email2.ToUpper(),
                UserName = email2
            };

            await userManager.CreateAsync(user2, "SarahT2000,");
        }

        if (!context.Houses.Any())
        {
            var user1 = userManager.Users.FirstOrDefault(u => u.FirstName == "John");
            var user2 = userManager.Users.FirstOrDefault(u => u.FirstName == "Sarah");
            if (user1 != null)
            {
                var houses = new List<House>()
            {
                new House
                {
                    HouseId = 1,
                    Title = "Charming cottage on Åsen",
                    Description = "Small charming cottage with charm on Øståsen in Vikersund.",
                    HouseImageUrl = "assets/images/aasen_cottage.jpg",
                    BedroomImageUrl = "assets/images/aasen_bed.jpg",
                    BathroomImageUrl = "assets/images/aasen_bath.jpg",
                    Location = "Modum, Viken, Norway",
                    PricePerNight = 990,
                    Bedrooms = 3,
                    Bathrooms = 2,
                    UserId = user1.Id
                },
                new House
                {
                    HouseId = 2,
                    Title = "Big villa in Holmenkollen",
                    Description = "Modern big villa with swimming pool and nice view.",
                    HouseImageUrl = "assets/images/villa.jpg",
                    BedroomImageUrl = "assets/images/villa_bed.jpg",
                    BathroomImageUrl = "assets/images/villa_bath.jpg",
                    Location = "Oslo, Norway",
                    PricePerNight = 1190,
                    Bedrooms = 4,
                    Bathrooms = 3,
                    UserId = user1.Id
                },
                new House
                {
                    HouseId = 3,
                    Title = "Typical Norwegian cottage",
                    Description = "Typical Norwegian cottage, very cosy near water",
                    HouseImageUrl = "assets/images/small_cottage.jpg",
                    BedroomImageUrl = "assets/images/cottage_bed.jpg",
                    BathroomImageUrl = "assets/images/cottage_bath.jpg",
                    Location = "Sandefjord, Norway",
                    PricePerNight = 690,
                    Bedrooms = 1,
                    Bathrooms = 1,
                    UserId = user2.Id
                },
                new House
                {
                    HouseId = 4,
                    Title = "Northern Lights Retreat",
                    Description = "Experience the magic of the Northern Lights from this cozy log cabin in the Arctic Circle of Norway.",
                    HouseImageUrl = "assets/images/northernlights.jpg",
                    BedroomImageUrl = "assets/images/northernlights_bed.jpg",
                    BathroomImageUrl = "assets/images/northernlights_bath.jpg",
                    Location = "Tromsø, Norway",
                    PricePerNight = 790,
                    Bedrooms = 2,
                    Bathrooms = 1,
                    UserId = user2.Id
                },
                new House
                {
                    HouseId = 5,
                    Title = "Bergen City Apartment",
                    Description = "Stay in the heart of Bergen in this stylish and modern city apartment.",
                    HouseImageUrl = "assets/images/bergen.jpg",
                    BedroomImageUrl = "assets/images/bergen_bed.jpg",
                    BathroomImageUrl = "assets/images/bergen_bath.jpg",
                    Location = "Bergen, Norway",
                    PricePerNight = 490,
                    Bedrooms = 2,
                    Bathrooms = 1,
                    UserId = user1.Id
                }
            };
                context.AddRange(houses);
                context.SaveChanges();
            }
        }

        if (!context.Reservations.Any())
        {
            var user1 = userManager.Users.FirstOrDefault(u => u.FirstName == "John");
            var user2 = userManager.Users.FirstOrDefault(u => u.FirstName == "Admin");
            var reservations = new List<Reservation>
            {
                new Reservation
                {
                    CheckInDate = new DateTime(2023, 09, 28),
                    CheckOutDate = new DateTime(2023, 10, 15),
                    HouseId = 1,
                    DateCreated = DateTime.Today.AddDays(-25),
                    UserId = user1.Id
                },
                new Reservation
                {
                    CheckInDate = new DateTime(2023, 10, 20),
                    CheckOutDate = new DateTime(2023, 10, 27),
                    HouseId = 2,
                    DateCreated = DateTime.Today.AddDays(-10),
                    UserId = user2.Id,
                },
            };

            foreach (var reservation in reservations)
            {
                var house = context.Houses.Find(reservation.HouseId);
                if (house != null)
                {
                    TimeSpan duration = reservation.CheckOutDate - reservation.CheckInDate;
                    reservation.BookingDuration = duration.Days;
                    reservation.TotalPrice = reservation.BookingDuration * house.PricePerNight;
                }
            }
            context.AddRange(reservations);
            context.SaveChanges();
        }
    }
}
