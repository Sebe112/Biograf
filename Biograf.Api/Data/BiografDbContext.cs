using Biograf.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Biograf.Api.Data;

public class BiografDbContext : DbContext
{
    public BiografDbContext(DbContextOptions<BiografDbContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<MovieGenre> MovieGenres => Set<MovieGenre>();
    public DbSet<Cinema> Cinemas => Set<Cinema>();
    public DbSet<Hall> Halls => Set<Hall>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Screening> Screenings => Set<Screening>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingSeat> BookingSeats => Set<BookingSeat>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MovieGenre>()
            .HasKey(mg => new { mg.MovieId, mg.GenreId });

        modelBuilder.Entity<MovieGenre>()
            .HasOne(mg => mg.Movie)
            .WithMany(m => m.Genres)
            .HasForeignKey(mg => mg.MovieId);

        modelBuilder.Entity<MovieGenre>()
            .HasOne(mg => mg.Genre)
            .WithMany(g => g.Movies)
            .HasForeignKey(mg => mg.GenreId);

        modelBuilder.Entity<BookingSeat>()
            .HasKey(bs => new { bs.BookingId, bs.SeatId });

        modelBuilder.Entity<BookingSeat>()
            .HasOne(bs => bs.Booking)
            .WithMany(b => b.Seats)
            .HasForeignKey(bs => bs.BookingId);

        modelBuilder.Entity<BookingSeat>()
            .HasOne(bs => bs.Seat)
            .WithMany(s => s.Bookings)
            .HasForeignKey(bs => bs.SeatId);

        modelBuilder.Entity<Seat>()
            .Ignore(s => s.Label);
    }
}
