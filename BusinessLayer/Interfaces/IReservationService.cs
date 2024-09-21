using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationResponse>> GetAllReservationsAsync();
        Task<ReservationResponse> GetReservationByIdAsync(int id);
        Task<ReservationResponse> CreateReservationAsync(ReservationRequest reservationRequest);
        Task<ReservationResponse> UpdateReservationAsync(int id, ReservationRequest reservationRequest);
        Task<Reservation> DeleteReservationAsync(int id);
    }
}
