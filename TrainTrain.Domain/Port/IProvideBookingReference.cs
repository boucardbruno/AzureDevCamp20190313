using System.Threading.Tasks;

namespace TrainTrain.Domain.Port
{
    public interface IProvideBookingReference
    {
        Task<BookingReference> GetBookingReference();
    }
}