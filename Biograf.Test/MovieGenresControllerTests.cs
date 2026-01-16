using Biograf.Api.Controllers;
using Biograf.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Biograf.Test;

public class MovieGenresControllerTests
{
    private class FakeMovieGenreRepository : IMovieGenre
    {
        private readonly List<MovieGenreLink> _links;

        public FakeMovieGenreRepository()
        {
            _links = new List<MovieGenreLink>();
        }

        public Task<bool> AddGenreToMovieAsync(int movieId, int genreId)
        {
            bool exists = false;

            for (int i = 0; i < _links.Count; i++)
            {
                MovieGenreLink link = _links[i];
                if (link.MovieId == movieId && link.GenreId == genreId)
                {
                    exists = true;
                    break;
                }
            }

            if (exists)
            {
                return Task.FromResult(false);
            }

            _links.Add(new MovieGenreLink(movieId, genreId));
            return Task.FromResult(true);
        }

        public Task<bool> RemoveGenreFromMovieAsync(int movieId, int genreId)
        {
            for (int i = 0; i < _links.Count; i++)
            {
                MovieGenreLink link = _links[i];
                if (link.MovieId == movieId && link.GenreId == genreId)
                {
                    _links.RemoveAt(i);
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }

        private sealed class MovieGenreLink
        {
            public MovieGenreLink(int movieId, int genreId)
            {
                MovieId = movieId;
                GenreId = genreId;
            }

            public int MovieId { get; }
            public int GenreId { get; }
        }
    }

    [Fact]
    public async Task Add_WhenNew_ReturnsOk()
    {
        // Arrange
        var controller = new MovieGenresController(new FakeMovieGenreRepository());

        // Act
        var result = await controller.Add(1, 2);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Add_WhenExists_ReturnsBadRequest()
    {
        // Arrange
        var repository = new FakeMovieGenreRepository();
        await repository.AddGenreToMovieAsync(1, 2);
        var controller = new MovieGenresController(repository);

        // Act
        var result = await controller.Add(1, 2);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Remove_WhenNotFound_ReturnsNotFound()
    {
        // Arrange
        var controller = new MovieGenresController(new FakeMovieGenreRepository());

        // Act
        var result = await controller.Remove(1, 2);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
