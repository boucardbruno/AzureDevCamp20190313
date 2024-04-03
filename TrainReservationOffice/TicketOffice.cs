using System.Threading.Tasks;

namespace TrainReservationOffice;

public class TicketOffice(IProvideTrainData provideTrainData, IProvideBookingReference provideBookingReference)
{
    public async Task<Reservation> Reserve(TrainId trainId, RequestForSeats requestedForSeats)
    {
        var train = await provideTrainData.GetTrainById(trainId);

        if (train.DoesNotExceedOverallTrainCapacity(requestedForSeats))
        {
            var reservationAttempt = train.BuildReservationAttempt(requestedForSeats);

            if (reservationAttempt.IsFulFilled()) return await BookingReservationAttempt(reservationAttempt);
        }

        return new ReservationFailure(trainId);
    }

    private async Task<Reservation> BookingReservationAttempt(ReservationAttempt reservationAttempt)
    {
        var bookingReference = await provideBookingReference.GetBookingReference();
        reservationAttempt = reservationAttempt.AssignBookingReference(bookingReference);
        await provideTrainData.BookSeats(reservationAttempt);
        return reservationAttempt.Confirm();
    }
}