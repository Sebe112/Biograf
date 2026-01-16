import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { HallService } from '../../services/hall.service';
import { MovieService } from '../../services/movie.service';
import { ScreeningService } from '../../services/screening.service';
import { Hall } from '../../models/hall';
import { Movie } from '../../models/movie';
import { Screening } from '../../models/screening';

@Component({
  selector: 'app-halls',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './halls.component.html',
  styleUrl: './halls.component.css'
})
export class HallsComponent implements OnInit {
  halls: Hall[] = [];
  movies: Movie[] = [];
  screenings: Screening[] = [];
  selectedHallId: number | null = null;
  screeningStatus = '';

  constructor(
    private hallService: HallService,
    private movieService: MovieService,
    private screeningService: ScreeningService
  ) {}

  ngOnInit(): void {
    this.hallService.getAll().subscribe({
      next: this.handleHalls.bind(this),
      error: this.handleHallsError.bind(this)
    });

    this.movieService.getAll().subscribe({
      next: this.handleMovies.bind(this),
      error: this.handleMoviesError.bind(this)
    });

    this.screeningService.getAll().subscribe({
      next: this.handleScreenings.bind(this),
      error: this.handleScreeningsError.bind(this)
    });
  }

  onHallChange(event: Event): void {
    const target = event.target as HTMLSelectElement | null;
    const value = target ? target.value : '';

    if (value) {
      this.selectedHallId = Number(value);
    } else {
      this.selectedHallId = null;
    }

  }

  getScreeningsForSelectedHall(): Screening[] {
    if (this.selectedHallId === null) {
      return [];
    }

    const list: Screening[] = [];
    for (let i = 0; i < this.screenings.length; i++) {
      const screening = this.screenings[i];
      if (screening.hallId === this.selectedHallId) {
        list.push(screening);
      }
    }

    return list;
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

  private handleHalls(halls: Hall[]): void {
    this.halls = halls;
  }

  private handleHallsError(): void {
    this.halls = [];
  }

  private handleMovies(movies: Movie[]): void {
    this.movies = movies;
  }

  private handleMoviesError(): void {
    this.movies = [];
  }

  private handleScreenings(screenings: Screening[]): void {
    this.screenings = screenings;
    this.screeningStatus = '';
  }

  private handleScreeningsError(): void {
    this.screenings = [];
    this.screeningStatus = 'Could not load screenings.';
  }
}
