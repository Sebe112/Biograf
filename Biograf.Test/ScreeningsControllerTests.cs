using Biograf.Api.Controllers;
using Biograf.Application.Dtos.Screenings;
using Biograf.Domain.Interfaces;
using Biograf.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Biograf.Test;

public class ScreeningsControllerTests
{
    private class FakeScreeningRepository : IScreening
    {
        private readonly List<Screening> _screenings;

        public FakeScreeningRepository()
        {
            _screenings = new List<Screening>();

            Screening screening1 = new Screening();
            screening1.Id = 1;
            screening1.MovieId = 1;
            screening1.Movie = new Movie { Id = 1, Title = "Movie 1" };
            screening1.HallId = 1;
            screening1.Hall = new Hall { Id = 1, Name = "Hall 1" };
            screening1.StartsAt = DateTime.UtcNow;
            screening1.EndsAt = DateTime.UtcNow.AddHours(2);
            _screenings.Add(screening1);
        }

        public Task<List<Screening>> GetAllAsync()
        {
            return Task.FromResult(_screenings);
        }

        public Task<Screening?> GetByIdAsync(int id)
        {
            Screening? found = null;

            for (int i = 0; i < _screenings.Count; i++)
            {
                Screening currentScreening = _screenings[i];
                if (currentScreening.Id == id)
                {
                    found = currentScreening;
                    break;
                }
            }

            return Task.FromResult(found);
        }

        public Task<Screening?> GetWithHallAndSeatsAsync(int id)
        {
            return GetByIdAsync(id);
        }

        public Task<Screening> AddAsync(Screening screening)
        {
            _screenings.Add(screening);
            return Task.FromResult(screening);
        }

        public Task<bool> UpdateAsync(Screening screening)
        {
            bool updated = false;

            for (int i = 0; i < _screenings.Count; i++)
            {
                Screening currentScreening = _screenings[i];
                if (currentScreening.Id == screening.Id)
                {
                    _screenings[i] = screening;
                    updated = true;
                    break;
                }
            }

            return Task.FromResult(updated);
        }

        public Task<bool> DeleteAsync(int id)
        {
            for (int i = 0; i < _screenings.Count; i++)
            {
                if (_screenings[i].Id == id)
                {
                    _screenings.RemoveAt(i);
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithScreenings()
    {
        // Arrange
        var controller = new ScreeningsController(new FakeScreeningRepository());

        // Act
        var result = await controller.GetAll();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var screenings = Assert.IsType<List<ScreeningDto>>(ok.Value);
        Assert.Single(screenings);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ReturnsNotFound()
    {
        // Arrange
        var controller = new ScreeningsController(new FakeScreeningRepository());

        // Act
        var result = await controller.GetById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetWithHall_WhenNotFound_ReturnsNotFound()
    {
        // Arrange
        var controller = new ScreeningsController(new FakeScreeningRepository());

        // Act
        var result = await controller.GetWithHall(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
