
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Group_Project_2.Controllers;
using Group_Project_2.DAL;
using Group_Project_2.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
//using Group_Project_2.ViewModels;


namespace XunitTestGroup_Project_2.Controllers;

public class HouseControllerTests
{
    [Fact]
    public async Task TestTable()
    {
        // arrange
        var houseList = new List<House>()
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
            Bathrooms = 2
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
            Bathrooms = 3
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
            Bathrooms = 1
            }
        };
        var mockHouseRepository = new Mock<IHouseRepository>();
        mockHouseRepository.Setup(repo => repo.GetAll()).ReturnsAsync(houseList);
        var mockLogger = new Mock<ILogger<HouseController>>();
        var mockUserRepo = new Mock<IUserRepository>();
        var houseController = new HouseController(mockHouseRepository.Object, mockLogger.Object, mockUserRepo.Object);

        // act
        var result = await houseController.GetAll();

        // assert
        //var viewResult = Assert.IsType<List<House>>(result);
        //var viewHouseList = Assert.IsAssignableFrom<>(viewResult.ViewData.Model);
        //Assert.Equal(2, itemListViewModel.Items.Count());
        //Assert.Equal(houseList, houseListViewModel.Items);
    }

    [Fact]
    public async Task TestCreateNotOk()
    {
        //arrange
        var testHouse = new House
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
            Bathrooms = 2
        };
        var mockHouseRepository = new Mock<IHouseRepository>();
        mockHouseRepository.Setup(repo => repo.Create(testHouse)).ReturnsAsync(false);

        var mockLogger = new Mock<ILogger<HouseController>>();
        var mockUserRepo = new Mock<IUserRepository>();

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Email, "john.h@gmail.com"),
        }, "mock"));

        var controllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var houseController = new HouseController(mockHouseRepository.Object, mockLogger.Object, mockUserRepo.Object)
        {
            ControllerContext = controllerContext
        };

        //act
        var result = await houseController.Create(testHouse);

    }
}
 