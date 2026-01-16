using Biograf.Api.Controllers;
using Biograf.Application.Dtos.Genres;
using Biograf.Domain.Interfaces;
using Biograf.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Biograf.Test;

public class GenresControllerTests
{
    private class FakeGenreRepository : IGenre
    {
        private readonly List<Genre> _genres;

        public FakeGenreRepository()
        {
            _genres = new List<Genre>();

            Genre genre1 = new Genre();
            genre1.Id = 1;
            genre1.Name = "Action";
            _genres.Add(genre1);
        }

        public Task<List<Genre>> GetAllAsync()
        {
            return Task.FromResult(_genres);
        }

        public Task<Genre?> GetByIdAsync(int id)
        {
            Genre? found = null;

            for (int i = 0; i < _genres.Count; i++)
            {
                Genre currentGenre = _genres[i];
                if (currentGenre.Id == id)
                {
                    found = currentGenre;
                    break;
                }
            }

            return Task.FromResult(found);
        }

        public Task<Genre> AddAsync(Genre genre)
        {
            _genres.Add(genre);
            return Task.FromResult(genre);
        }

        public Task<bool> DeleteAsync(int id)
        {
            for (int i = 0; i < _genres.Count; i++)
            {
                if (_genres[i].Id == id)
                {
                    _genres.RemoveAt(i);
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithGenres()
    {
        // Arrange
        var controller = new GenresController(new FakeGenreRepository());

        // Act
        var result = await controller.GetAll();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var genres = Assert.IsType<List<GenreDto>>(ok.Value);
        Assert.Single(genres);
    }

    [Fact]
    public async Task Delete_WhenNotFound_ReturnsNotFound()
    {
        // Arrange
        var controller = new GenresController(new FakeGenreRepository());

        // Act
        var result = await controller.Delete(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
