using Microsoft.AspNetCore.Mvc;
using Group_Project_2.Models;
using Group_Project_2.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Group_Project_2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HouseController : Controller
{
    private readonly IHouseRepository _houseRepository;
    private readonly ILogger<HouseController> _logger;

    private readonly IUserRepository _userRepository;

    public HouseController(IHouseRepository houseRepository, ILogger<HouseController> logger, IUserRepository userRepository)
    {
        _houseRepository = houseRepository;
        _logger = logger;
        _userRepository = userRepository;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var houses = await _houseRepository.GetAll(); //this part would not be called during the unit test
        if (houses == null)
        {
            _logger.LogError("[HouseController] House list not found while executing _houseRepository.GetAll()");
            return NotFound("House list not found");
        }
        var returnDetails = houses.Select(hD => new
        {
            hD.HouseId,
            hD.BedroomImageUrl,
            hD.Bedrooms,
            hD.Description,
            hD.Location,
            hD.PricePerNight,
            hD.Title,
            hD.Bathrooms,
            hD.BathroomImageUrl,
            hD.HouseImageUrl
        });
        return Ok(returnDetails);
    }


    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] House house)
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var user = await _userRepository.GetUserByEmail(userEmail);

        if (house == null || user == null || userEmail == null)
        {
            return BadRequest("Invalid house data");
        }

        house.UserId = user.Id;
        bool returnOk = await _houseRepository.Create(house);
        if (returnOk)
        {
            var response = new { success = true, message = "House " + house.Title + " created successfully" };
            return Ok(response);
        }
        else
        {
            var response = new { success = false, message = "House creation failed" };
            return Ok(response);
        }
    }

    [HttpGet("{id}")]
    
    public async Task<IActionResult> GetHousebyId(int id)
    {
        var house = await _houseRepository.GetHouseById(id);
        if (house == null)
        {
            _logger.LogError("[HouseController] House list not found while executing _houseRepository.GetHouseById()");
            return NotFound("House list not found");
        }
        return Ok(house);
    }


    [HttpPost("update")]
    [Authorize]
    public async Task<IActionResult> Update(House house)
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var dbHouse = await _houseRepository.GetHouseById(house.HouseId);
        var user = await _userRepository.GetUserByEmail(userEmail);
        if (house == null || user == null || dbHouse == null)
        {
            return BadRequest("Invalid house data");
        }
        
        if(!(dbHouse.UserId == user.Id))
        {
            _logger.LogError("[HouseController] Update() unauthorized user.");
            return Unauthorized();
        }
        house.UserId = user.Id;
        bool returnOk = await _houseRepository.Update(house);
        if (returnOk)
        {
            var response = new { success = true, message = "House " + house.Title + " updated successfully" };
            return Ok(response);
        }
        else
        {
            _logger.LogError("[HouseController] House creation failed.");
            var response = new { success = false, message = "House update failed" };
            return Ok(response);
        }
    }


    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var house = await _houseRepository.GetHouseById(id);
        var user = await _userRepository.GetUserByEmail(userEmail);
        if (house == null || user == null || userEmail == null)
        {
            return BadRequest("Invalid house data");
        }
        if(house.UserId == user.Id)
        {
            bool returnOk = await _houseRepository.Delete(id);
            if (!returnOk)
            {
                _logger.LogError("[HouseController] House deletion failed for the HouseId {HouseId:0000}", id);
                return BadRequest("House deletion failed");
            }
            var response = new { success = true, message = "House " + id.ToString() + " deleted succesfully" };
            return Ok(response);
        }
        return Unauthorized();
    }
}
