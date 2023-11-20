using Microsoft.AspNetCore.Mvc;
using Group_Project_2.DAL;
using Microsoft.EntityFrameworkCore;
using Group_Project_2.ViewModels;
using Group_Project_2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Group_Project_2.Controllers;

public class ReservationController : Controller
{
    public readonly IHouseRepository _houseRepository;
    private readonly HouseDbContext _houseDbContext;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<ReservationController> _logger;

    public ReservationController(IHouseRepository houseRepository, ILogger<ReservationController> logger, HouseDbContext houseDbContext, UserManager<User> userManager)
    {
        _houseRepository = houseRepository;
        _logger = logger;
        _houseDbContext = houseDbContext;
        _userManager = userManager;
    }

    [Authorize]
    public async Task<IActionResult> Table()
    {
        var reservations = await _houseRepository.GetReservations();
        if (reservations == null)
        {
            _logger.LogError("[ReservationController] Reservation list not found while executing _houseRepository.GetReservations()");
            return NotFound("Reservation list not found");
        }

        ReservationListViewModel reservationListViewModel;
        if (User.IsInRole("Admin"))
        {
            reservationListViewModel = new ReservationListViewModel(reservations, "Table");
            return View(reservationListViewModel);
        }
        else
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            var userReservations = reservations.Where(r => r.UserId == loggedInUser.Id).ToList();
            reservationListViewModel = new ReservationListViewModel(userReservations, "Table");
            return View(reservationListViewModel);
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        var reservations = await _houseRepository.GetReservations();
        var right = reservations?.FirstOrDefault(r => r.ReservationId == id);
        if (right == null)
        {
            _logger.LogError("[ReservationController] Reservation not found for the ReservationId executing {ReservationId:0000}", id);
            return NotFound("Reservation not found");
        }
        return View(right);
    }

    [HttpGet]
    [Authorize(Roles = "Host, Tenant")]
    public async Task<IActionResult> CreateReservation()
    {
        var houses = await _houseDbContext.Houses.ToListAsync();
        var loggedInUser = await _userManager.GetUserAsync(User);

        if (loggedInUser == null)
        {
            return Unauthorized("User not authenticated");
        }

        var createReservationListViewModel = new CreateReservationViewModel
        {
            Reservation = new Reservation {
                UserId = loggedInUser.Id,
                DateCreated = DateTime.Now
            },

            HouseSelectList = houses.Select(house => new SelectListItem
            {
                Value = house.HouseId.ToString(),
                Text = house.HouseId.ToString() + ": " + house.Title
            }).ToList(),
        };

        return View(createReservationListViewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Tenant, Host")]
    public async Task<IActionResult> CreateReservation(Reservation reservation)
    {
        if (ModelState.IsValid)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (loggedInUser == null)
            {
                return Unauthorized("User not authenticated");
            }

            var house = _houseDbContext.Houses.Find(reservation.HouseId);


            if (house == null)
            {
                return BadRequest("House not found");
            }

            TimeSpan duration = reservation.CheckOutDate - reservation.CheckInDate;

            var newReservation = new Reservation
            {
                HouseId = reservation.HouseId,
                User = loggedInUser,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                BookingDuration = duration.Days,
                TotalPrice = house.PricePerNight * duration.Days,
                DateCreated = DateTime.Now
            };

            bool returnOk = await _houseRepository.CreateReservation(newReservation);
            if (returnOk)
                return RedirectToAction(nameof(Table));
        }
        _logger.LogWarning("[ReservationController] Reservation creation failed {@reservation}", reservation);
        return BadRequest("Reservation creation failed");   
    }

    [HttpGet]
    [Authorize(Roles = "Tenant, Host")]
    public async Task<IActionResult> CreateReservationFromId(int houseId)
    {
        var loggedInUser = await _userManager.GetUserAsync(User);

        if (loggedInUser == null)
        {
            return Unauthorized("User not authenticated");
        }

        var house = await _houseRepository.GetHouseById(houseId);

        if (house == null)
        {
            _logger.LogError("House not found for houseId: {houseId}", houseId);

            return BadRequest("House not found");
        }

        var newReservation = new Reservation
        {
            HouseId = houseId,
            House = house,
            UserId = loggedInUser.Id,
        };

        return View(newReservation);
    }


    [HttpGet]
    [Authorize(Roles = "Tenant, Host")]
    public async Task<IActionResult> Update(int id)
    {
        var reservation = await _houseRepository.GetReservationById(id);
        var loggedInUser = await _userManager.GetUserAsync(User);
        if (reservation == null)
        {
            _logger.LogError("[ReservationController] Reservation not found when updating the ReservationId {ReservationId:0000}", id);
            return BadRequest("Reservation not found for the ReservationId");
        }

        if (!reservation.UserId.Equals(loggedInUser.Id) && !User.IsInRole("Admin"))
        {
            return Unauthorized("You do not have permission to update this reservation.");
        }

        var houses = await _houseDbContext.Houses.ToListAsync();
        ViewBag.HouseSelectList = houses.Select(house => new SelectListItem
        {
            Value = house.HouseId.ToString(),
            Text = house.HouseId.ToString() + ": " + house.Title
        }).ToList();

        return View(reservation);
    }

    [HttpPost]
    [Authorize(Roles = "Tenant, Host")]
    public async Task<IActionResult> Update(Reservation reservation)
    {
        if (ModelState.IsValid)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            reservation.UserId = loggedInUser.Id;

            var newHouse = _houseDbContext.Houses.Find(reservation.HouseId);
            if (newHouse == null)
            {
                return BadRequest("House not found");
            }

            TimeSpan duration = reservation.CheckOutDate - reservation.CheckInDate;
            reservation.BookingDuration = duration.Days;
            reservation.TotalPrice = newHouse.PricePerNight * duration.Days;
            reservation.DateCreated = DateTime.Now;

            bool returnOk = await _houseRepository.UpdateReservation(reservation);
            if (returnOk)
                return RedirectToAction(nameof(Table));
        }

        _logger.LogWarning("[ReservationController] Reservation update failed {@reservation}", reservation);

        return View(reservation);
    }

    [HttpGet]
    [Authorize(Roles = "Tenant, Host, Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var reservation = await _houseRepository.GetReservationById(id);
        var loggedInUser = await _userManager.GetUserAsync(User);
        if (reservation == null)
        {
            _logger.LogError("[ReservationController] Reservation not found for the ReservationId {ReservationId:0000}", id);
            return BadRequest("Reservation not found for the ReservationId");
        }

        if (!reservation.UserId.Equals(loggedInUser.Id) && !User.IsInRole("Admin"))
        {
            return Unauthorized("You do not have permission to delete this reservation");
        }
        return View(reservation);
    }

    [HttpPost]
    [Authorize(Roles = "Tenant, Host, Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var loggedInUser = await _userManager.GetUserAsync(User);
        var reservation = await _houseRepository.GetReservationById(id);

        if (reservation == null)
        {
            return BadRequest("Reservation not found");
        }
        else if(!reservation.UserId.Equals(loggedInUser.Id) && !User.IsInRole("Admin"))
        {
            return Unauthorized("You do not have permission to delete this reservation.");
        }

        bool returnOk = await _houseRepository.DeleteReservation(id);
        if (!returnOk)
        {
            _logger.LogError("[ReservationController] Reservation deletion failed for the ReservationId {ReservationId:0000}", id);
            return BadRequest("Deletion of reservation failed.");
        }

        return RedirectToAction(nameof(Table));
    }
}
