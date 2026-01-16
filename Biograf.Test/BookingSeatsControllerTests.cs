using Biograf.Api.Controllers;
using Biograf.Application.Dtos.Bookings;
using Biograf.Domain.Interfaces;
using Biograf.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Biograf.Test;

public class BookingSeatsControllerTests
{
    private class FakeBookingSeatRepository : IBookingSeat
    {
        private readonly List<BookingSeat> _bookingSeats;

        public FakeBookingSeatRepository()
        {
            _bookingSeats = new List<BookingSeat>();

            BookingSeat seat1 = new BookingSeat();
            seat1.BookingId = 1;
            seat1.Booking = new Booking { Id = 1, UserId = "user-1", User = new ApplicationUser(), ScreeningId = 1, Screening = new Screening() };
            seat1.SeatId = 1;
            seat1.Seat = new Seat { Id = 1, HallId = 1, Hall = new Hall { Id = 1, Name = "Hall 1" }, RowIndex = 1, ColumnIndex = 1, Label = "A1" };
            seat1.ScreeningId = 1;
            seat1.Price = 100;
            _bookingSeats.Add(seat1);
        }

        public Task<List<BookingSeat>> GetByScreeningAsync(int screeningId)
        {
            List<BookingSeat> result = new List<BookingSeat>();

            for (int i = 0; i < _bookingSeats.Count; i++)
            {
                BookingSeat current = _bookingSeats[i];
                if (current.ScreeningId == screeningId)
                {
                    result.Add(current);
                }
            }

            return Task.FromResult(result);
        }

        public Task<bool> IsSeatBookedAsync(int screeningId, int seatId)
        {
            for (int i = 0; i < _bookingSeats.Count; i++)
            {
                BookingSeat current = _bookingSeats[i];
                if (current.ScreeningId == screeningId && current.SeatId == seatId)
                {
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }

        public Task AddAsync(int bookingId, int seatId, int screeningId, decimal price)
        {
            BookingSeat newSeat = new BookingSeat();
            newSeat.BookingId = bookingId;
            newSeat.Booking = new Booking { Id = bookingId, UserId = "user-1", User = new ApplicationUser(), ScreeningId = screeningId, Screening = new Screening() };
            newSeat.SeatId = seatId;
            newSeat.Seat = new Seat { Id = seatId, HallId = 1, Hall = new Hall { Id = 1, Name = "Hall 1" }, RowIndex = 1, ColumnIndex = 1, Label = "A1" };
            newSeat.ScreeningId = screeningId;
            newSeat.Price = price;
            _bookingSeats.Add(newSeat);
            return Task.CompletedTask;
        }

        public Task<bool> RemoveAsync(int bookingId, int seatId)
        {
            for (int i = 0; i < _bookingSeats.Count; i++)
            {
                BookingSeat current = _bookingSeats[i];
                if (current.BookingId == bookingId && current.SeatId == seatId)
                {
                    _bookingSeats.RemoveAt(i);
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }
    }

    [Fact]
    public async Task GetByScreening_ReturnsOkResult_WithSeats()
    {
        // Arrange
        var controller = new BookingSeatsController(new FakeBookingSeatRepository());

        // Act
        var result = await controller.GetByScreening(1);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var seats = Assert.IsType<List<BookingSeatDto>>(ok.Value);
        Assert.Single(seats);
    }

    [Fact]
    public async Task RemoveSeat_WhenNotFound_ReturnsNotFound()
    {
        // Arrange
        var controller = new BookingSeatsController(new FakeBookingSeatRepository());

        // Act
        var result = await controller.RemoveSeat(999, 999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
