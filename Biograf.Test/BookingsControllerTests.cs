using Biograf.Api.Controllers;
using Biograf.Application.Dtos.Bookings;
using Biograf.Domain.Interfaces;
using Biograf.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Biograf.Test;

public class BookingsControllerTests
{
    private class FakeBookingSeatRepository : IBookingSeat
    {
        public Task<List<BookingSeat>> GetByScreeningAsync(int screeningId)
        {
            return Task.FromResult(new List<BookingSeat>());
        }

        public Task<bool> IsSeatBookedAsync(int screeningId, int seatId)
        {
            return Task.FromResult(false);
        }

        public Task AddAsync(int bookingId, int seatId, int screeningId, decimal price)
        {
            return Task.CompletedTask;
        }

        public Task<bool> RemoveAsync(int bookingId, int seatId)
        {
            return Task.FromResult(false);
        }
    }

    private class FakeScreeningRepository : IScreening
    {
        public Task<List<Screening>> GetAllAsync()
        {
            return Task.FromResult(new List<Screening>());
        }

        public Task<Screening?> GetByIdAsync(int id)
        {
            return Task.FromResult<Screening?>(null);
        }

        public Task<Screening?> GetWithHallAndSeatsAsync(int id)
        {
            return Task.FromResult<Screening?>(null);
        }

        public Task<Screening> AddAsync(Screening screening)
        {
            return Task.FromResult(screening);
        }

        public Task<bool> UpdateAsync(Screening screening)
        {
            return Task.FromResult(false);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return Task.FromResult(false);
        }
    }

    private class FakeBookingRepository : IBooking
    {
        private readonly List<Booking> _bookings;

        public FakeBookingRepository()
        {
            _bookings = new List<Booking>();

            Booking booking1 = new Booking();
            booking1.Id = 1;
            booking1.UserId = "user-1";
            booking1.User = new ApplicationUser { UserName = "user-1" };
            booking1.ScreeningId = 1;
            booking1.Screening = new Screening { Id = 1 };
            _bookings.Add(booking1);
        }

        public Task<Booking?> GetByIdAsync(int id)
        {
            Booking? found = null;

            for (int i = 0; i < _bookings.Count; i++)
            {
                Booking current = _bookings[i];
                if (current.Id == id)
                {
                    found = current;
                    break;
                }
            }

            return Task.FromResult(found);
        }

        public Task<List<Booking>> GetByUserIdAsync(string userId)
        {
            List<Booking> result = new List<Booking>();

            for (int i = 0; i < _bookings.Count; i++)
            {
                Booking current = _bookings[i];
                if (current.UserId == userId)
                {
                    result.Add(current);
                }
            }

            return Task.FromResult(result);
        }

        public Task<Booking> AddAsync(Booking booking)
        {
            _bookings.Add(booking);
            return Task.FromResult(booking);
        }

        public Task<bool> UpdateStatusAsync(int bookingId, BookingStatus status)
        {
            for (int i = 0; i < _bookings.Count; i++)
            {
                Booking current = _bookings[i];
                if (current.Id == bookingId)
                {
                    current.Status = status;
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }
    }

    [Fact]
    public async Task GetMyBookings_ReturnsOkResult_WithBookings()
    {
        // Arrange
        var controller = new BookingsController(
            new FakeBookingRepository(),
            new FakeBookingSeatRepository(),
            new FakeScreeningRepository());
        var context = new DefaultHttpContext();
        var identity = new ClaimsIdentity();
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "user-1"));
        context.User = new ClaimsPrincipal(identity);
        controller.ControllerContext = new ControllerContext { HttpContext = context };

        // Act
        var result = await controller.GetMyBookings();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var bookings = Assert.IsType<List<BookingDto>>(ok.Value);
        Assert.Single(bookings);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ReturnsNotFound()
    {
        // Arrange
        var controller = new BookingsController(
            new FakeBookingRepository(),
            new FakeBookingSeatRepository(),
            new FakeScreeningRepository());

        // Act
        var result = await controller.GetById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
