using Microsoft.AspNetCore.Mvc;
using Group_Project_2.DAL;
using Microsoft.EntityFrameworkCore;
using Group_Project_2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Group_Project_2.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ReservationController : Controller
{
    public readonly IHouseRepository _houseRepository;
    private readonly HouseDbContext _houseDbContext;
    private readonly ILogger<ReservationController> _logger;
    private readonly IUserRepository _userRepository;

    public ReservationController(IHouseRepository houseRepository, ILogger<ReservationController> logger, HouseDbContext houseDbContext, IUserRepository userRepository)
    {
        _houseRepository = houseRepository;
        _logger = logger;
        _houseDbContext = houseDbContext;
        _userRepository = userRepository;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if(!string.IsNullOrEmpty(userEmail))
            {
                var reservations = await _houseRepository.GetReservations(); //unitTest (this is not called?)
                var user = await _userRepository.GetUserByEmail(userEmail);
                var userReservations = reservations?.Where(r => r.UserId == user?.Id);

                if (reservations == null || user == null)
                {
                    _logger.LogError("[ReservationController] Reservation list not found while executing _houseRepository.GetReservations()");
                    return NotFound("Reservation list not found");
                }
                var returnDetails = userReservations?.Select(uR => new
                {
                    uR.ReservationId,
                    uR.CheckInDate,
                    uR.CheckOutDate,
                    uR.TotalPrice,
                    uR.BookingDuration,
                    uR.DateCreated,
                    uR.HouseId,
                });

                return Ok(returnDetails);
            }

            return Unauthorized();
        }
        catch (Exception e)
        {
            _logger.LogError("[ReservationController] Error occurred in GetAll(). Error: ", e.Message);
            return StatusCode(500, new { success = false, message = "An unexpected error occurred while fetching all user reservations." });
        }
        
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateReservation([FromBody] Reservation reservation)
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (!string.IsNullOrEmpty(userEmail)) {
            var user = await _userRepository.GetUserByEmail(userEmail);
            var house = _houseDbContext.Houses.Find(reservation.HouseId);

            if (house == null || user == null)
            {
                return BadRequest("House not found");
            }
            if (reservation == null)
            {
                return BadRequest("Invalid reservation data");
            }

            TimeSpan duration = reservation.CheckOutDate - reservation.CheckInDate;

            var newReservation = new Reservation
            {
                HouseId = reservation.HouseId,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                BookingDuration = duration.Days,
                TotalPrice = house.PricePerNight * duration.Days,
                DateCreated = DateTime.Now,
                UserId = user.Id
            };

            bool returnOk = await _houseRepository.CreateReservation(newReservation);
            if (returnOk)
            {
                var response = new { success = true, message = "Reservation " + newReservation.ReservationId + " created successfully" };
                return Ok(response);
            }
            else
            {
                var response = new { success = false, message = "Reservation creation failed" };
                return Ok(response);
            }
        }

        return Unauthorized();
    }
 

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetReservationbyId(int id)
    {
        var reservation = await _houseRepository.GetReservationById(id);
        if (reservation == null)
        {
            _logger.LogError("[ReservationController] Reservation list not found while executing _houseRepository.GetReservationById()");
            return NotFound("Reservation list not found");
        }
        return Ok(reservation);
    }
    
    [HttpPut("update/{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Reservation reservation)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(userEmail))
        {
            return BadRequest("User email not found in claims");
        }

        var user = await _userRepository.GetUserByEmail(userEmail);
        var newHouse = await _houseDbContext.Houses.FindAsync(reservation.HouseId);

        if (newHouse == null || user == null || reservation == null)
        {
            return BadRequest("Invalid data: House or user not found, or invalid reservation data");
        }

        reservation.BookingDuration = (int)(reservation.CheckOutDate - reservation.CheckInDate).TotalDays;
        reservation.TotalPrice = newHouse.PricePerNight * reservation.BookingDuration;
        reservation.DateCreated = DateTime.Now;
        reservation.UserId = user.Id;

        bool isUpdateSuccessful = await _houseRepository.UpdateReservation(reservation);

        var response = new
        {
            success = isUpdateSuccessful,
            message = isUpdateSuccessful ? $"Reservation {reservation.ReservationId} updated successfully" : "Reservation update failed"
        };

        return Ok(response);
    }

    
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if(!string.IsNullOrEmpty(userEmail))
        {
            var user = await _userRepository.GetUserByEmail(userEmail);
            var reservation = await _houseRepository.GetReservationById(id);


            if (user == null || reservation == null)
            {
                _logger.LogError("[ReservationController] User {User} or Reservation {Reservation} are null", user.Id, reservation.ReservationId);
                return StatusCode(500, new { message = "Reservation or User is null" });
            }

            if (reservation.UserId == user.Id)
            {
                bool returnOk = await _houseRepository.DeleteReservation(id);
                if (!returnOk)
                {
                    _logger.LogError("[ReservationController] Reservation deletion failed for the ReservationId {ReservationId:0000}", id);
                    return StatusCode(500, new { message = "Reservation deletion failed" });
                }
                var response = new { success = true, message = "Reservation " + id.ToString() + " deleted succesfully" };
                return Ok(response);
            }
            return Unauthorized();
        }
        return Unauthorized();
    }
}