using Microsoft.AspNetCore.Mvc;
using Group_Project_2.Models;
using Group_Project_2.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Group_Project_2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HouseController : Controller
{
    private readonly IHouseRepository _houseRepository;
    private readonly ILogger<HouseController> _logger;

    public HouseController(IHouseRepository houseRepository, ILogger<HouseController> logger)
    {
        _houseRepository = houseRepository;
        _logger = logger;
    }

    /*
    public async Task<IActionResult> Table()
    {
        var houses = await _houseRepository.GetAll();
        if (houses == null)
        {
            _logger.LogError("[HouseController] House list not found while executing _houseRepository.GetAll()");
            return NotFound("House list not found");
        }
        var houseListViewModel = new HouseListViewModel(houses, "Table");
        return View(houseListViewModel);
    }

    public async Task<IActionResult> Grid()
    {
        var houses = await _houseRepository.GetAll();
        if (houses == null)
        {
            _logger.LogError("[HouseController] House list not found while executing _houseRepository.GetAll()");
            return NotFound("House list not found");
        }
        var houseListViewModel = new HouseListViewModel(houses, "Grid");
        return View(houseListViewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var house = await _houseRepository.GetHouseById(id);
        if (house == null)
        {
            _logger.LogError("[HouseController] House not found for the HouseId executing {HouseId:0000}", id);
            return NotFound("House not found for the HouseId"); 
        }
        return View(house);
    }
    */

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var houses = await _houseRepository.GetAll(); //this part would not be called during the unit test
        if (houses == null)
        {
            _logger.LogError("[HouseController] House list not found while executing _houseRepository.GetAll()");
            return NotFound("House list not found");
        }
        return Ok(houses);
    }

    /*
    [HttpGet]
    [Authorize(Roles = "Host")]
    public IActionResult Create()
    {
        return View();
    }
    */

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] House house)
    {
        if (house == null)
        {
            return BadRequest("Invalid house data");
        }
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

    /*
    [HttpGet]
    [Authorize(Roles = "Host")]
    public async Task<IActionResult> Update(int id)
    {
        var house = await _houseRepository.GetHouseById(id);
        var loggedInUser = await _userManager.GetUserAsync(User);
        if (house == null)
        {
            _logger.LogError("[HouseController] House not found when updating the HouseId {HouseId:0000}", id);
            return BadRequest("House not found for the HouseId");
        }

        if(!house.UserId.Equals(loggedInUser.Id) && !User.IsInRole("Admin"))
        {
            return Unauthorized("You do not have permission to update this house.");
        }
        return View(house);
    }
    */

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(House house)
    {
        if (house == null)
        {
            return BadRequest("Invalid house data");
        }
        bool returnOk = await _houseRepository.Update(house);
        if (returnOk)
        {
            var response = new { success = true, message = "House " + house.Title + " updated successfully" };
            return Ok(response);
        }
        else
        {
            var response = new { success = false, message = "House update failed" };
            return Ok(response);
        }
    }

    /*
    [HttpGet]
    [Authorize(Roles = "Admin, Host")]
    public async Task<IActionResult> Delete(int id)
    {
        var house = await _houseRepository.GetHouseById(id);
        var loggedInUser = await _userManager.GetUserAsync(User);
        if (house == null)
        {
            _logger.LogError("[HouseController] House not found for the HouseId {HouseId:0000}", id);
            return BadRequest("House not found for the HouseId");
        }

        else if(house.UserId.Equals(loggedInUser.Id)  || User.IsInRole("Admin"))
        {
            return View(house);
        }
        return Unauthorized("You do not have permission to delete this house");
    }
    */

    [HttpDelete]
    [Authorize(Roles = "Admin, Host")]
    public async Task<IActionResult> DeleteConfirmed(int id)
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
}
