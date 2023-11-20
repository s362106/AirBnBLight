using Microsoft.EntityFrameworkCore;
using Group_Project_2.Models;

namespace Group_Project_2.DAL;

public class HouseRepository : IHouseRepository
{
    private readonly HouseDbContext _db;
    private readonly ILogger<HouseRepository> _logger;

    public HouseRepository(HouseDbContext db, ILogger<HouseRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<House>?> GetAll()
    {
        try
        {
            return await _db.Houses.ToListAsync();
        } catch (Exception e)
        {
            _logger.LogError("[HouseRepository] house ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<House?> GetHouseById(int id)
    {
        try
        {
            return await _db.Houses.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[HouseRepository] house FindAsync(id) failed when GetItemById() for HouseId {HouseId:0000}, error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<bool> Create(House house)
    {
        try
        {
            _db.Houses.Add(house);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[HouseRepository] house creation failed for house {@house}, error message: {e}", house, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(House house)
    {
        try
        {
            _db.Houses.Update(house);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[HouseRepository] house FindAsync(id) failed when updating the HouseId {HouseId:0000}, error message: {e}", house, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var house = await _db.Houses.FindAsync(id);
            if (house == null)
            {
                _logger.LogError("[HouseRepository] house not found for the HouseId {HouseId:0000}", id);
                return false;
            }

            _db.Houses.Remove(house);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[HouseRepository] house deletion failed for the HouseId {HouseId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }

    public async Task<IEnumerable<Reservation>?> GetReservations()
    {
        try
        {
            return await _db.Reservations.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[HouseRepository] reservations ToListAsync() failed when GetReservations(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<Reservation?> GetReservationById(int id)
    {
        try
        {
            return await _db.Reservations.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[HouseRepository] house FindAsync(id) failed when GetItemById() for HouseId {HouseId:0000}, error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<bool> CreateReservation(Reservation reservation)
    {
        try
        {
            _db.Reservations.Add(reservation);
            await _db.SaveChangesAsync();
            return true;
        } catch (Exception e)
        {
            _logger.LogError("[HouseRepository] reservation creation failed for reservation {@reservation}, error message: {e}", reservation, e.Message);
            return false;
        }
    }

    public async Task<bool> UpdateReservation(Reservation reservation)
    {
        try
        {
            _db.Reservations.Update(reservation);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[HouseRepository] reservation FindAsync(id) failed when updating the ReservationId {ReservationId:0000}, error message: {e}", reservation, e.Message);
            return false;
        }
    }

    public async Task<bool> DeleteReservation(int id)
    {
        try
        {
            var reservation = await _db.Reservations.FindAsync(id);
            if (reservation == null)
            {
                _logger.LogError("[HouseRepository] reservation not found for the ReservationId {ReservationId:0000}", id);
                return false;
            }

            _db.Reservations.Remove(reservation);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[HouseRepository] house deletion failed for the HouseId {HouseId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }

}

