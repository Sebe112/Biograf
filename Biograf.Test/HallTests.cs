using Biograf.Api.Controllers;
using Biograf.Application.Dtos.Halls;
using Biograf.Domain.Interfaces;
using Biograf.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Biograf.Test;

public class HallControllerTest
{
    private class FakeHallRepository : IHall
    {
       private readonly List<Hall> halls;

        public FakeHallRepository()
        {
            halls = new List<Hall>();

            Hall hall1 = new Hall();
            hall1.Id = 1;
            hall1.Name = "Sal 1";
            hall1.Rows = 5;
            hall1.Columns = 10;
            hall1.Layout = "[]";

            halls.Add(hall1);
        }

        public Task<List<Hall>> GetAllAsync()
        {
            return Task.FromResult(halls);
        }

        public Task<Hall?> GetByIdAsync(int id)
        {
            Hall? foundHall = null;

            for (int i = 0; i < halls.Count; i++)
            {
                Hall currentHall = halls[i];

                if (currentHall.Id == id)
                {
                    foundHall = currentHall;
                    break;
                }
            }

            return Task.FromResult(foundHall);
        }

        public Task<Hall> AddAsync(Hall hall)
        {
            halls.Add(hall);
            return Task.FromResult(hall);
        }

        public Task<bool> UpdateAsync(Hall hall)
        {
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return Task.FromResult(true);
        }
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithHalls()
    {
        // Arrange
        var controller = new HallsController(new FakeHallRepository());

        // Act
        var result = await controller.GetAll();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var halls = Assert.IsType<List<HallDto>>(ok.Value);
        Assert.Single(halls);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ReturnsNotFound()
    {
        // Arrange
        var controller = new HallsController(new FakeHallRepository());

        // Act
        var result = await controller.GetById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
