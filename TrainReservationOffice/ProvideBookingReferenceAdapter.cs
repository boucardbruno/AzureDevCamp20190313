using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TrainReservationOffice;

public class ProvideBookingReferenceAdapter(string uriBookingReferenceService) : IProvideBookingReference
{
    public async Task<BookingReference> GetBookingReference()
    {
        using var client = new HttpClient();
        var value = new MediaTypeWithQualityHeaderValue("application/json");
        client.BaseAddress = new Uri(uriBookingReferenceService);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(value);

        // HTTP GET
        var response = await client.GetAsync("/booking_reference");
        response.EnsureSuccessStatusCode();
        return new BookingReference(await response.Content.ReadAsStringAsync());
    }
}