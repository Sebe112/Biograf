namespace Biograf.Infrastructure.Data;

/// <summary>
/// EF Core database context for the cinema app.
/// </summary>
public class BiografDbContext : IdentityDbContext<ApplicationUser>
{
    public BiografDbContext(DbContextOptions<BiografDbContext> options) : base(options) { }

    public DbSet<Hall> Halls
    {
        get { return Set<Hall>(); }
    }

    public DbSet<Seat> Seats
    {
        get { return Set<Seat>(); }
    }

    public DbSet<Movie> Movies
    {
        get { return Set<Movie>(); }
    }

    public DbSet<Genre> Genres
    {
        get { return Set<Genre>(); }
    }

    public DbSet<MovieGenre> MovieGenres
    {
        get { return Set<MovieGenre>(); }
    }

    public DbSet<Screening> Screenings
    {
        get { return Set<Screening>(); }
    }

    public DbSet<Booking> Bookings
    {
        get { return Set<Booking>(); }
    }

    public DbSet<BookingSeat> BookingSeats
    {
        get { return Set<BookingSeat>(); }
    }

    /// <summary>
    /// Configures entity relationships and constraints.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MovieGenre>()
            .HasKey("MovieId", "GenreId");

        modelBuilder.Entity<Genre>()
            .HasIndex("Name")
            .IsUnique();

        modelBuilder.Entity<Seat>()
            .HasIndex("HallId", "RowIndex", "ColumnIndex")
            .IsUnique();

        modelBuilder.Entity<BookingSeat>()
            .HasKey("BookingId", "SeatId");

        modelBuilder.Entity<BookingSeat>()
            .HasIndex("ScreeningId", "SeatId")
            .IsUnique();

        modelBuilder.Entity<Booking>()
            .HasOne("User")
            .WithMany()
            .HasForeignKey("UserId")
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BookingSeat>()
            .HasOne("Seat")
            .WithMany()
            .HasForeignKey("SeatId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
