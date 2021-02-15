using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace TestNinja.Models
{
    public static class BookingHelper
    {
        public static string OverlappingBookingsExist(Booking booking, IBookingRepository bookingRepository)
        {
            if (booking.Status == "Cancelled")
                return string.Empty;

            var bookings = bookingRepository.GetActiveBookings(booking.Id);
            var overlappingBooking =
                bookings.FirstOrDefault(
                    b =>
                        booking.ArrivalDate < b.DepartureDate
                        && b.ArrivalDate < booking.DepartureDate
                      );

            return overlappingBooking == null ? string.Empty : overlappingBooking.Reference;
        }
        public static string BookingExists(int id, IBookingRepository bookingRepository)
        {
            var booking = bookingRepository.GetBooking(id);
            return booking == null ? string.Empty : booking.Reference;
        }
    }

    public class Booking
    {
        public string Status { get; set; }
        public int Id { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Reference { get; set; }
    }
    public class BookingContext
    {
        public DbSet<Booking> Booking { get; set; }

        public void SaveChanges()
        {
        }
    }
}