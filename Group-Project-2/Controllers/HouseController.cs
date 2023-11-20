using Microsoft.AspNetCore.Mvc;
using Group_Project_2.Models;
using Group_Project_2.ViewModels;
using Group_Project_2.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Group_Project_2.Controllers;

public class HouseController : Controller
{
    private readonly IHouseRepository _houseRepository;
    private readonly ILogger<HouseController> _logger;
    private readonly UserManager<User> _userManager;

    public HouseController(IHouseRepository houseRepository, ILogger<HouseController> logger, UserManager<User> userManager)
    {
        _houseRepository = houseRepository;
        _logger = logger;
        _userManager = userManager;
    }

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

    [HttpGet]
    [Authorize(Roles = "Host")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Host")]
    public async Task<IActionResult> Create(House house)
    {
        var loggedInUser = await _userManager.GetUserAsync(User);
        if (ModelState.IsValid && loggedInUser != null)
        {
            house.UserId = loggedInUser.Id;
            bool returnOk = await _houseRepository.Create(house);
            if (returnOk)
                return RedirectToAction(nameof(Grid));
        }
        _logger.LogWarning("[HouseController] House creation failed {@house}", house);
        return View(house);
    }

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

    [HttpPost]
    [Authorize(Roles = "Host")]
    public async Task<IActionResult> Update(House house)
    {
        if (ModelState.IsValid)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            bool returnOk = await _houseRepository.Update(house);
            if (returnOk)
                return RedirectToAction(nameof(Grid));
        }
        _logger.LogWarning("[HouseController] House update failed {@house}", house);
        return View(house);
    }

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

    [HttpPost]
    [Authorize(Roles = "Admin, Host")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var loggedInUser = await _userManager.GetUserAsync(User);
        var house = await _houseRepository.GetHouseById(id);

        if(house == null)
        {
            return BadRequest("House not found");
        }

        bool returnOk = await _houseRepository.Delete(id);
        if (!returnOk)
        {
            _logger.LogError("[HouseController] House deletion failed for the HouseId {HouseId:0000}", id);
            return BadRequest("House deletion failed");
        }
        return RedirectToAction(nameof(Grid));
    }
}
