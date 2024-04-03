using System;
using System.Collections.Generic;
using System.Linq;
using Value;

namespace TrainReservationOffice;

public class Train(TrainId trainId, List<Seat> seats)
: ValueType<TrainId>
{
    private TrainId TrainId { get; } = trainId;
    private List<Seat> Seats { get; } = seats;
    
    private int ReservedSeats
    {
        get { return Seats.Count(seat => !seat.IsAvailable); }
    }
    
    public bool DoesNotExceedOverallTrainCapacity(RequestForSeats requestedForSeats)
    {
        return ReservedSeats + requestedForSeats.NumberOfSeats <=
               Math.Floor(ThresholdManager.GetTrainMaxCapacityThreshold() * Seats.Count);
    }

    public ReservationAttempt BuildReservationAttempt(RequestForSeats requestedForSeats)
    {
        var availableSeats = Seats
            .Where(s => s.IsAvailable)
            .Take(requestedForSeats.NumberOfSeats);

        return new ReservationAttempt(TrainId, requestedForSeats, availableSeats, new BookingReference(""));
    }

    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
        return new object[] { TrainId, new ListByValue<Seat>(Seats) };
    }
}