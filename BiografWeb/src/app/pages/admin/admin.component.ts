import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { MovieService, MovieRequest } from '../../services/movie.service';
import { HallService, HallRequest } from '../../services/hall.service';
import { GenreService, GenreRequest } from '../../services/genre.service';
import { MovieGenreService } from '../../services/movie-genre.service';
import { ScreeningService, ScreeningRequest } from '../../services/screening.service';
import { BookingService } from '../../services/booking.service';
import { AuthResponse } from '../../models/auth-response';
import { Movie } from '../../models/movie';
import { Hall } from '../../models/hall';
import { Genre } from '../../models/genre';
import { Screening } from '../../models/screening';
import { Booking } from '../../models/booking';
import { BookingSeat } from '../../models/booking-seat';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent implements OnInit {
  user: AuthResponse | null = null;
  movies: Movie[] = [];
  halls: Hall[] = [];
  genres: Genre[] = [];
  screenings: Screening[] = [];
  bookings: Booking[] = [];
  bookedSeats: BookingSeat[] = [];

  movieForm: MovieRequest = {
    title: '',
    description: '',
    durationMinutes: 90
  };

  hallForm: HallRequest = {
    name: '',
    rows: 10,
    columns: 10
  };

  genreForm: GenreRequest = {
    name: ''
  };

  movieGenreForm = {
    movieId: 0,
    genreId: 0
  };

  screeningForm = {
    movieId: 0,
    hallId: 0,
    startsAt: '',
    endsAt: '',
    basePrice: 0
  };

  selectedScreeningId: number | null = null;

  editingMovieId: number | null = null;
  editingHallId: number | null = null;
  movieStatus = '';
  hallStatus = '';
  genreStatus = '';
  movieGenreStatus = '';
  screeningStatus = '';
  bookingStatus = '';
  bookingSeatStatus = '';

  constructor(
    private authService: AuthService,
    private movieService: MovieService,
    private hallService: HallService,
    private genreService: GenreService,
    private movieGenreService: MovieGenreService,
    private screeningService: ScreeningService,
    private bookingService: BookingService
  ) {}

  ngOnInit(): void {
    this.user = this.authService.currentUser;
    if (this.isAdmin(this.user)) {
      this.loadMovies();
      this.loadHalls();
      this.loadGenres();
      this.loadScreenings();
      this.loadBookings();
    }
  }

  isAdmin(user: AuthResponse | null): boolean {
    if (!user || !user.roles) {
      return false;
    }

    return user.roles.indexOf('Admin') >= 0;
  }

  loadMovies(): void {
    this.movieService.getAll().subscribe({
      next: this.handleMovies.bind(this),
      error: this.handleMoviesError.bind(this)
    });
  }

  loadHalls(): void {
    this.hallService.getAll().subscribe({
      next: this.handleHalls.bind(this),
      error: this.handleHallsError.bind(this)
    });
  }

  loadGenres(): void {
    this.genreService.getAll().subscribe({
      next: this.handleGenres.bind(this),
      error: this.handleGenresError.bind(this)
    });
  }

  loadScreenings(): void {
    this.screeningService.getAll().subscribe({
      next: this.handleScreenings.bind(this),
      error: this.handleScreeningsError.bind(this)
    });
  }

  loadBookings(): void {
    this.bookingService.getMyBookings().subscribe({
      next: this.handleBookings.bind(this),
      error: this.handleBookingsError.bind(this)
    });
  }

  saveMovie(): void {
    this.movieStatus = '';
    const payload = this.normalizeMovieForm();

    if (this.editingMovieId === null) {
      this.movieService.create(payload).subscribe({
        next: this.handleMovieSaved.bind(this),
        error: this.handleMovieSaveError.bind(this)
      });
    } else {
      this.movieService.update(this.editingMovieId, payload).subscribe({
        next: this.handleMovieSaved.bind(this),
        error: this.handleMovieSaveError.bind(this)
      });
    }
  }

  editMovie(movie: Movie): void {
    this.editingMovieId = movie.id;
    this.movieForm = {
      title: movie.title,
      description: movie.description ?? '',
      durationMinutes: movie.durationMinutes
    };
  }

  cancelMovieEdit(): void {
    this.editingMovieId = null;
    this.movieForm = {
      title: '',
      description: '',
      durationMinutes: 90
    };
  }

  deleteMovie(movie: Movie): void {
    this.movieService.delete(movie.id).subscribe({
      next: this.handleMovieSaved.bind(this),
      error: this.handleMovieSaveError.bind(this)
    });
  }

  saveHall(): void {
    this.hallStatus = '';
    const payload = this.normalizeHallForm();

    if (this.editingHallId === null) {
      this.hallService.create(payload).subscribe({
        next: this.handleHallSaved.bind(this),
        error: this.handleHallSaveError.bind(this)
      });
    } else {
      this.hallService.update(this.editingHallId, payload).subscribe({
        next: this.handleHallSaved.bind(this),
        error: this.handleHallSaveError.bind(this)
      });
    }
  }

  editHall(hall: Hall): void {
    this.editingHallId = hall.id;
    this.hallForm = {
      name: hall.name,
      rows: hall.rows,
      columns: hall.columns
    };
  }

  cancelHallEdit(): void {
    this.editingHallId = null;
    this.hallForm = {
      name: '',
      rows: 10,
      columns: 10
    };
  }

  deleteHall(hall: Hall): void {
    this.hallService.delete(hall.id).subscribe({
      next: this.handleHallSaved.bind(this),
      error: this.handleHallSaveError.bind(this)
    });
  }

  saveGenre(): void {
    this.genreStatus = '';
    const payload = this.normalizeGenreForm();

    this.genreService.create(payload).subscribe({
      next: this.handleGenreSaved.bind(this),
      error: this.handleGenreSaveError.bind(this)
    });
  }

  deleteGenre(genre: Genre): void {
    this.genreService.delete(genre.id).subscribe({
      next: this.handleGenreSaved.bind(this),
      error: this.handleGenreSaveError.bind(this)
    });
  }

  addMovieGenre(): void {
    this.movieGenreStatus = '';
    const payload = this.normalizeMovieGenreForm();
    if (!payload) {
      this.movieGenreStatus = 'Select both movie and genre.';
      return;
    }

    this.movieGenreService.add(payload.movieId, payload.genreId).subscribe({
      next: this.handleMovieGenreSaved.bind(this),
      error: this.handleMovieGenreSaveError.bind(this)
    });
  }

  removeMovieGenre(): void {
    this.movieGenreStatus = '';
    const payload = this.normalizeMovieGenreForm();
    if (!payload) {
      this.movieGenreStatus = 'Select both movie and genre.';
      return;
    }

    this.movieGenreService.remove(payload.movieId, payload.genreId).subscribe({
      next: this.handleMovieGenreSaved.bind(this),
      error: this.handleMovieGenreSaveError.bind(this)
    });
  }

  saveScreening(): void {
    this.screeningStatus = '';
    const payload = this.normalizeScreeningForm();

    if (!payload) {
      this.screeningStatus = 'Fill all screening fields.';
      return;
    }

    this.screeningService.create(payload).subscribe({
      next: this.handleScreeningSaved.bind(this),
      error: this.handleScreeningSaveError.bind(this)
    });
  }

  deleteScreening(screening: Screening): void {
    this.screeningService.delete(screening.id).subscribe({
      next: this.handleScreeningSaved.bind(this),
      error: this.handleScreeningSaveError.bind(this)
    });
  }

  onBookedScreeningChange(event: Event): void {
    const target = event.target as HTMLSelectElement | null;
    const value = target ? target.value : '';

    if (value) {
      this.selectedScreeningId = Number(value);
      this.loadBookedSeats();
    } else {
      this.selectedScreeningId = null;
      this.bookedSeats = [];
    }
  }

  loadBookedSeats(): void {
    if (this.selectedScreeningId === null) {
      return;
    }

    this.bookingSeatStatus = '';
    this.bookingService.getBookedSeats(this.selectedScreeningId).subscribe({
      next: this.handleBookedSeats.bind(this),
      error: this.handleBookedSeatsError.bind(this)
    });
  }

  removeBookedSeat(seat: BookingSeat): void {
    this.bookingSeatStatus = '';
    this.bookingService.removeSeat(seat.bookingId, seat.seatId).subscribe({
      next: this.handleBookedSeatRemoved.bind(this),
      error: this.handleBookedSeatsError.bind(this)
    });
  }

  getMovieTitle(movieId: number): string {
    for (let i = 0; i < this.movies.length; i++) {
      const movie = this.movies[i];
      if (movie.id === movieId) {
        return movie.title;
      }
    }

    return 'Movie ' + movieId;
  }

  getHallName(hallId: number): string {
    for (let i = 0; i < this.halls.length; i++) {
      const hall = this.halls[i];
      if (hall.id === hallId) {
        return hall.name;
      }
    }

    return 'Hall ' + hallId;
  }

  private normalizeMovieForm(): MovieRequest {
    return {
      title: this.movieForm.title.trim(),
      description: this.movieForm.description?.trim() ?? '',
      durationMinutes: Number(this.movieForm.durationMinutes)
    };
  }

  private normalizeHallForm(): HallRequest {
    return {
      name: this.hallForm.name.trim(),
      rows: Number(this.hallForm.rows),
      columns: Number(this.hallForm.columns)
    };
  }

  private normalizeGenreForm(): GenreRequest {
    return {
      name: this.genreForm.name.trim()
    };
  }

  private normalizeMovieGenreForm(): { movieId: number; genreId: number } | null {
    const movieId = Number(this.movieGenreForm.movieId);
    const genreId = Number(this.movieGenreForm.genreId);

    if (!movieId || !genreId) {
      return null;
    }

    return {
      movieId: movieId,
      genreId: genreId
    };
  }

  private normalizeScreeningForm(): ScreeningRequest | null {
    const movieId = Number(this.screeningForm.movieId);
    const hallId = Number(this.screeningForm.hallId);
    const startsAt = this.toIsoString(this.screeningForm.startsAt);
    const endsAt = this.toIsoString(this.screeningForm.endsAt);
    const basePrice = Number(this.screeningForm.basePrice);

    if (!movieId || !hallId || !startsAt || !endsAt) {
      return null;
    }

    return {
      movieId: movieId,
      hallId: hallId,
      startsAt: startsAt,
      endsAt: endsAt,
      basePrice: basePrice
    };
  }

  private toIsoString(value: string): string {
    if (!value) {
      return '';
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return '';
    }

    return date.toISOString();
  }

  private handleMovies(movies: Movie[]): void {
    this.movies = movies;
  }

  private handleMoviesError(): void {
    this.movies = [];
    this.movieStatus = 'Failed to load movies.';
  }

  private handleHalls(halls: Hall[]): void {
    this.halls = halls;
  }

  private handleHallsError(): void {
    this.halls = [];
    this.hallStatus = 'Failed to load halls.';
  }

  private handleGenres(genres: Genre[]): void {
    this.genres = genres;
  }

  private handleGenresError(): void {
    this.genres = [];
    this.genreStatus = 'Failed to load genres.';
  }

  private handleScreenings(screenings: Screening[]): void {
    this.screenings = screenings;
  }

  private handleScreeningsError(): void {
    this.screenings = [];
    this.screeningStatus = 'Failed to load screenings.';
  }

  private handleBookings(bookings: Booking[]): void {
    this.bookings = bookings;
  }

  private handleBookingsError(): void {
    this.bookings = [];
    this.bookingStatus = 'Failed to load bookings.';
  }

  private handleMovieSaved(): void {
    this.loadMovies();
    this.cancelMovieEdit();
    this.movieStatus = 'Saved.';
  }

  private handleMovieSaveError(): void {
    this.movieStatus = 'Save failed.';
  }

  private handleHallSaved(): void {
    this.loadHalls();
    this.cancelHallEdit();
    this.hallStatus = 'Saved.';
  }

  private handleHallSaveError(): void {
    this.hallStatus = 'Save failed.';
  }

  private handleGenreSaved(): void {
    this.loadGenres();
    this.genreForm = { name: '' };
    this.genreStatus = 'Saved.';
  }

  private handleGenreSaveError(): void {
    this.genreStatus = 'Save failed.';
  }

  private handleMovieGenreSaved(): void {
    this.movieGenreStatus = 'Saved.';
  }

  private handleMovieGenreSaveError(): void {
    this.movieGenreStatus = 'Save failed.';
  }

  private handleScreeningSaved(): void {
    this.loadScreenings();
    this.screeningStatus = 'Saved.';
    this.screeningForm = {
      movieId: 0,
      hallId: 0,
      startsAt: '',
      endsAt: '',
      basePrice: 0
    };
  }

  private handleScreeningSaveError(): void {
    this.screeningStatus = 'Save failed.';
  }

  private handleBookedSeats(seats: BookingSeat[]): void {
    this.bookedSeats = seats;
  }

  private handleBookedSeatsError(): void {
    this.bookedSeats = [];
    this.bookingSeatStatus = 'Failed to load booked seats.';
  }

  private handleBookedSeatRemoved(): void {
    this.bookingSeatStatus = 'Removed.';
    this.loadBookedSeats();
  }
}
