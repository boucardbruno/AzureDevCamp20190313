using System.Threading.Tasks;

namespace TrainReservationOffice;

public interface IProvideBookingReference
{
    Task<BookingReference> GetBookingReference();
}