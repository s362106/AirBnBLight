using Group_Project_2.Models;

namespace Group_Project_2.DAL
{
	public interface IHouseRepository
	{
		Task<IEnumerable<House>?> GetAll();
		Task<House?> GetHouseById(int id);
        Task<bool> Create(House house);
        Task<bool> Update(House house);
        Task<bool> Delete(int id);
        Task<IEnumerable<Reservation>?> GetReservations();
        Task<Reservation?> GetReservationById(int id);
        Task<bool> UpdateReservation(Reservation reservation);
        Task<bool> DeleteReservation(int id);
        Task<bool> CreateReservation(Reservation reservation);
    }
}
