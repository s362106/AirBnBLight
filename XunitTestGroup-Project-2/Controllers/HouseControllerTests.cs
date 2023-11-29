
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Group_Project_2.Controllers;
using Group_Project_2.DAL;
using Group_Project_2.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Net;
//using Group_Project_2.ViewModels;


namespace XunitTestGroup_Project_2.Controllers;

public class HouseControllerTests
{
    [Fact]
    public async Task TestReadOk()
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
    public async Task TestReadNotOk()
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

    [Fact]
    public async Task TestUpdateOk()
    {
        // Arrange
        var testHouse = new House
        {
            HouseId = 1,
            // other properties
        };

        var mockHouseRepository = new Mock<IHouseRepository>();
        mockHouseRepository.Setup(repo => repo.GetHouseById(testHouse.HouseId)).ReturnsAsync(new House());
        mockHouseRepository.Setup(repo => repo.Update(testHouse)).ReturnsAsync(true);

        var mockLogger = new Mock<ILogger<HouseController>>();

        // Mock user repository to return a user when GetUserByEmail is called
        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User());

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

        // Act
        var result = await houseController.Update(testHouse);

        // Assert
        Assert.True(result is ObjectResult, $"Unexpected result type: {result.GetType().Name}");

        if (result is OkObjectResult okResult)
        {
            // Successful update
            var successValue = (bool)okResult.Value.GetType().GetProperty("success")!.GetValue(okResult.Value);
            Assert.True(successValue, $"Expected success to be true, but was {successValue}");
        }
        else if (result is BadRequestObjectResult badRequestResult)
        {
            // Unsuccessful update
            var errorMessage = (string)badRequestResult.Value;
            Assert.Equal("Invalid house data", errorMessage);
        }
        else
        {
            // Unexpected result type
            Assert.True(false, $"Unexpected result type: {result.GetType().Name}");
        }
    }

    [Fact]
    public async Task TestDeleteOk()
    {
        // Arrange
        var houseIdToDelete = 1;

        var mockHouseRepository = new Mock<IHouseRepository>();
        mockHouseRepository.Setup(repo => repo.Delete(houseIdToDelete)).ReturnsAsync(true); // Simulate successful delete

        var mockLogger = new Mock<ILogger<HouseController>>();

        // Mock user repository to return a user when GetUserByEmail is called
        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User());

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

        // Act
        var result = await houseController.DeleteConfirmed(houseIdToDelete);

        // Assert
        Assert.True(result is ObjectResult, $"Unexpected result type: {result.GetType().Name}");

        if (result is OkObjectResult okResult)
        {
            // Successful delete
            var successValue = (bool)okResult.Value.GetType().GetProperty("success")!.GetValue(okResult.Value);
            Assert.True(successValue, $"Expected success to be true, but was {successValue}");
        }
        else
        {
            // Unexpected result type
            Assert.True(false, $"Unexpected result type: {result.GetType().Name}");
        }
    }

    [Fact]
    public async Task TestDeleteNotOk()
    {
        // Arrange
        var houseIdToDelete = 1;

        var mockHouseRepository = new Mock<IHouseRepository>();
        mockHouseRepository.Setup(repo => repo.Delete(houseIdToDelete)).ReturnsAsync(false); // Simulate unsuccessful delete

        var mockLogger = new Mock<ILogger<HouseController>>();

        // Mock user repository to return a user when GetUserByEmail is called
        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User());

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

        // Act
        var result = await houseController.DeleteConfirmed(houseIdToDelete);

        // Assert
        if (result is ObjectResult objectResult)
        {
            if (objectResult.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                // Unsuccessful delete
                var errorMessage = (string)objectResult.Value;
                Assert.Contains("House deletion failed", errorMessage, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                // Unexpected result type
                Assert.True(false, $"Unexpected result type: {result.GetType().Name}");
            }
        }
        else
        {
            // Unexpected result type
            Assert.True(false, $"Unexpected result type: {result.GetType().Name}");
        }
    }

    [Fact]
    public async Task TestUpdateNotOk()
    {
        // Arrange
        var testHouse = new House
        {
            HouseId = 1,
            // other properties
        };

        var mockHouseRepository = new Mock<IHouseRepository>();
        mockHouseRepository.Setup(repo => repo.GetHouseById(testHouse.HouseId)).ReturnsAsync(new House());
        mockHouseRepository.Setup(repo => repo.Update(testHouse)).ReturnsAsync(false); // Simulate unsuccessful update

        var mockLogger = new Mock<ILogger<HouseController>>();

        // Mock user repository to return a user when GetUserByEmail is called
        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User());

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

        // Act
        var result = await houseController.Update(testHouse);

        // Assert
        if (result is ObjectResult objectResult)
        {
            if (objectResult.StatusCode == (int)HttpStatusCode.OK)
            {
                // Unexpected successful update
                var successValue = (bool)objectResult.Value.GetType().GetProperty("success")!.GetValue(objectResult.Value);
                Assert.False(successValue, $"Expected success to be false, but was {successValue}");
            }
            else if (objectResult.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                // Unsuccessful update
                var errorMessage = (string)objectResult.Value;
                Assert.Contains("House update failed", errorMessage, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                // Unexpected result type
                Assert.True(false, $"Unexpected result type: {result.GetType().Name}");
            }
        }
        else
        {
            // Unexpected result type
            Assert.True(false, $"Unexpected result type: {result.GetType().Name}");
        }
    }

    [Fact]
    public async Task TestCreateOk()
    {
        // Arrange
        var testHouse = new House
        {
            HouseId = 1,
            // other properties
        };

        var mockHouseRepository = new Mock<IHouseRepository>();
        mockHouseRepository.Setup(repo => repo.Create(testHouse)).ReturnsAsync(true); // Simulate successful create

        var mockLogger = new Mock<ILogger<HouseController>>();

        // Mock user repository to return a user when GetUserByEmail is called
        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User());

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

        // Act
        var result = await houseController.Create(testHouse);

        // Assert
        if (result is ObjectResult objectResult)
        {
            if (objectResult.StatusCode == (int)HttpStatusCode.OK)
            {
                // Successful create
                var successValue = (bool)objectResult.Value.GetType().GetProperty("success")!.GetValue(objectResult.Value);
                Assert.True(successValue, $"Expected success to be true, but was {successValue}");
            }
            else
            {
                // Unexpected result type
                Assert.True(false, $"Unexpected result type: {result.GetType().Name}");
            }
        }
        else
        {
            // Unexpected result type
            Assert.True(false, $"Unexpected result type: {result.GetType().Name}");
        }
    }

    [Fact]
    public async Task TestCreateNotOk()
    {
        // Arrange
        var testHouse = new House
        {
            HouseId = 1,
            // other properties
        };

        var mockHouseRepository = new Mock<IHouseRepository>();
        mockHouseRepository.Setup(repo => repo.Create(testHouse)).ReturnsAsync(false); // Simulate unsuccessful create

        var mockLogger = new Mock<ILogger<HouseController>>();

        // Mock user repository to return a user when GetUserByEmail is called
        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User());

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

        // Act
        var result = await houseController.Create(testHouse);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<ObjectResult>(result);

        var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
        Assert.Contains("House creation failed", objectResult.Value.ToString(), StringComparison.OrdinalIgnoreCase);
    }







}
