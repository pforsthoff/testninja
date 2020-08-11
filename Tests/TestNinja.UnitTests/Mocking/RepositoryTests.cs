using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TestNinja.Models;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class RepositoryTests
    {
        private Booking _existingBooking;
        private Mock<IBookingRepository> _bookingRepository;

        [SetUp]
        public void SetUp()
        {
            var bookingInMemoryDatabase = new List<Booking>
            {
                new Booking() {Id = 1, ArrivalDate = ArriveOn(2017, 1, 15),
                    DepartureDate = DepartOn(2017, 1, 20),Reference = "A"},
                new Booking() {Id = 2, ArrivalDate = ArriveOn(2018, 1, 15),
                    DepartureDate = DepartOn(2018, 1, 20),Reference = "B"},
                new Booking() {Id = 3, ArrivalDate = ArriveOn(2019, 1, 15),
                    DepartureDate = DepartOn(2019, 1, 20),Reference = "C"}
            };
            _bookingRepository = new Mock<IBookingRepository>();
            _bookingRepository.Setup(x => x.GetBooking(It.IsAny<int>()))
                .Returns((int i) => bookingInMemoryDatabase.Single(bo => bo.Id == i));
       
        }
        [Test]
        public void GetBooking_BookingExists_ReturnsBooking()
        {
            //act
            var result = BookingHelper.BookingExists(1, _bookingRepository.Object);
            //Arrange

            //assert
            Assert.That(result, Is.Not.Empty);
        }
        [Test]
        public void GetBooking_BookingDoesntExist_ReturnsEmpty()
        {
            Assert.That(() => BookingHelper.BookingExists(5,_bookingRepository.Object),
                Throws.Exception
                    .TypeOf<InvalidOperationException>());
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
