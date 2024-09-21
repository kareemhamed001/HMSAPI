using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IReservationRepository
    {
        Task<IEnumerable<ReservationResponse>> GetAllReservationsAsync();
        Task<ReservationResponse?> GetReservationByIdAsync(int id);
        Task<ReservationResponse> CreateReservationAsync(Reservation reservation);
        Task<ReservationResponse> UpdateReservationAsync(int id, Reservation reservation);
        Task<Reservation> DeleteReservationAsync(Reservation reservation);
    }
}
