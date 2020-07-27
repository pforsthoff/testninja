using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TestNinja.Models;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class BookingTests
    {
        private Booking _existingBooking;
        private Mock<IBookingRepository> _bookingRepository;

        [SetUp]
        public void SetUp()
        {
            _existingBooking = new Booking
            {
                Id = 2,
                ArrivalDate = ArriveOn(2017, 1, 15),
                DepartureDate = DepartOn(2017, 1, 20),
                Reference = "a"
            };
            _bookingRepository = new Mock<IBookingRepository>();
            _bookingRepository.Setup(r => r.GetActiveBookings(1)).Returns(new List<Booking>
            {
               _existingBooking
            }.AsQueryable());
            //arrange
        }
        [Test]
        public void OverlappingBookingsExist_StartFinishBeforeExistingBooking_ReturnEmptyString()
        {
            //act
            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = Before(_existingBooking.ArrivalDate, days: 2),
                DepartureDate = Before(_existingBooking.ArrivalDate),
            }, _bookingRepository.Object);

            Assert.That(result, Is.Empty);
        }
        [Test]
        public void OverlappingBookingsExist_StartBeforeFinishesAfterExistingBooking_ReturnsExistingBooking()
        {
            //act
            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = Before(_existingBooking.ArrivalDate),
                DepartureDate = After(_existingBooking.DepartureDate),
            }, _bookingRepository.Object);

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }
        [Test]
        public void OverlappingBookingsExist_StartsAndFinishesInMiddleOfExistingBooking_ReturnsExistingBooking()
        {
            //act
            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = After(_existingBooking.ArrivalDate),
                DepartureDate = Before(_existingBooking.DepartureDate),
            }, _bookingRepository.Object);

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }
        [Test]
        public void OverlappingBookingsExist_StartInMiddleFinishesAfterExistingBooking_ReturnsExistingBooking()
        {
            //act
            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = After(_existingBooking.ArrivalDate),
                DepartureDate = After(_existingBooking.DepartureDate),
            }, _bookingRepository.Object);

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }
        [Test]
        public void OverlappingBookingsExist_StartsAndFinishesAfterExistingBooking_ReturnsEmptyString()
        {
            //act
            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = After(_existingBooking.ArrivalDate),
                DepartureDate = After(_existingBooking.DepartureDate, days: 2),
            }, _bookingRepository.Object);

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }
        [Test]
        public void OverlappingBookingsExist_OverlapButNewBookingCancelled_ReturnsEmptyString()
        {
            //act
            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = After(_existingBooking.ArrivalDate),
                DepartureDate = After(_existingBooking.DepartureDate),
                Status = "Cancelled",
            }, _bookingRepository.Object);

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }
        private DateTime Before(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(-days);
        }
        private DateTime After(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(days);
        }
        private DateTime ArriveOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 14, 0, 0);
        }
        private DateTime DepartOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 10, 0, 0);
        }
    }
}
