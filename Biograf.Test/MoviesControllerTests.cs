using Biograf.Api.Controllers;
using Biograf.Application.Dtos.Movies;
using Biograf.Domain.Interfaces;
using Biograf.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Biograf.Test;

public class MoviesControllerTests
{
    private class FakeMovieRepository : IMovie
    {
        private readonly List<Movie> _movies;

        public FakeMovieRepository()
        {
            _movies = new List<Movie>();

            Movie movie1 = new Movie();
            movie1.Id = 1;
            movie1.Title = "Movie 1";
            _movies.Add(movie1);
        }

        public Task<List<Movie>> GetAllAsync()
        {
            return Task.FromResult(_movies);
        }

        public Task<Movie?> GetByIdAsync(int id)
        {
            Movie? found = null;

            for (int i = 0; i < _movies.Count; i++)
            {
                Movie currentMovie = _movies[i];
                if (currentMovie.Id == id)
                {
                    found = currentMovie;
                    break;
                }
            }

            return Task.FromResult(found);
        }

        public Task<Movie> AddAsync(Movie movie)
        {
            if (movie.Id == 0)
            {
                movie.Id = _movies.Count + 1;
            }

            _movies.Add(movie);
            return Task.FromResult(movie);
        }

        public Task<bool> UpdateAsync(Movie movie)
        {
            bool updated = false;

            for (int i = 0; i < _movies.Count; i++)
            {
                Movie currentMovie = _movies[i];
                if (currentMovie.Id == movie.Id)
                {
                    _movies[i] = movie;
                    updated = true;
                    break;
                }
            }

            return Task.FromResult(updated);
        }

        public Task<bool> DeleteAsync(int id)
        {
            for (int i = 0; i < _movies.Count; i++)
            {
                if (_movies[i].Id == id)
                {
                    _movies.RemoveAt(i);
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithMovies()
    {
        // Arrange
        var controller = new MoviesController(new FakeMovieRepository());

        // Act
        var result = await controller.GetAll();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var movies = Assert.IsType<List<MovieDto>>(ok.Value);
        Assert.Single(movies);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ReturnsNotFound()
    {
        // Arrange
        var controller = new MoviesController(new FakeMovieRepository());

        // Act
        var result = await controller.GetById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WithMovie()
    {
        // Arrange
        var controller = new MoviesController(new FakeMovieRepository());
        MovieCreateRequest request = new MovieCreateRequest();
        request.Title = "Movie 2";
        request.Description = "Description";
        request.DurationMinutes = 120;

        // Act
        var result = await controller.Create(request);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(result);
        var movie = Assert.IsType<MovieDto>(created.Value);
        Assert.Equal(2, movie.Id);
        Assert.Equal("GetById", created.ActionName);
    }
}
