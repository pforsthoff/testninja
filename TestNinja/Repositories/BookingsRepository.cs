using System.Collections.Generic;
using System.Linq;

namespace TestNinja.Models
{
    public class BookingsRepository : IBookingRepository
    {
        private readonly BookingContext _db;

        public BookingsRepository()
        {
            _db = new BookingContext();
        }

        public IEnumerable<Booking> GetAllBookings(Booking currentBooking)
        {
            return _db.Booking.Where(x =>x.Id != currentBooking.Id && x.Status != "Cancelled").ToList();
        }
        public Booking GetBooking(int id)
        {
            return _db.Booking.SingleOrDefault(b => b.Id == id);
        }
        public IQueryable<Booking> GetActiveBookings(int? excludedBookingId = null)
        {
            var unitOfWork = new UnitOfWork();

            var bookings =
                unitOfWork.Query<Booking>()
                    .Where(
                        b => b.Status != "Cancelled");
            if(excludedBookingId.HasValue)
            {
                bookings = bookings.Where(b => b.Id != excludedBookingId.Value);
            }
            return bookings;
        }
        public class UnitOfWork
        {
            public IQueryable<T> Query<T>()
            {
                return new List<T>().AsQueryable();
            }
        }
    }
}
