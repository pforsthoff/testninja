using System.Collections.Generic;
using System.Linq;

namespace TestNinja.Models
{
    public interface IBookingRepository
    {
        IEnumerable<Booking> GetAllBookings(Booking currentBooking);
        Booking GetBooking(int id);
        IQueryable<Booking> GetActiveBookings(int? excludedBookingId = null);

    }
}