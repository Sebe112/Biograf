import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MovieService } from '../../services/movie.service';
import { HallService } from '../../services/hall.service';
import { ScreeningService } from '../../services/screening.service';
import { Movie } from '../../models/movie';
import { Hall } from '../../models/hall';
import { Screening } from '../../models/screening';

@Component({
  selector: 'app-movie-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './movie-detail.component.html',
  styleUrl: './movie-detail.component.css'
})
export class MovieDetailComponent implements OnInit {
  movie?: Movie;
  halls: Hall[] = [];
  screenings: Screening[] = [];
  movieId: number | null = null;
  loadError = '';

  constructor(
    private route: ActivatedRoute,
    private movieService: MovieService,
    private hallService: HallService,
    private screeningService: ScreeningService
  ) {}

  ngOnInit(): void {
    const idValue = this.route.snapshot.paramMap.get('id');
    if (idValue) {
      this.movieId = Number(idValue);
    }

    if (this.movieId === null || Number.isNaN(this.movieId)) {
      this.loadError = 'Movie not found.';
      return;
    }

    this.movieService.getById(this.movieId).subscribe({
      next: this.handleMovie.bind(this),
      error: this.handleMovieError.bind(this)
    });

    this.hallService.getAll().subscribe({
      next: this.handleHalls.bind(this),
      error: this.handleHallsError.bind(this)
    });

    this.screeningService.getAll().subscribe({
      next: this.handleScreenings.bind(this),
      error: this.handleScreeningsError.bind(this)
    });
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

  private handleMovie(movie: Movie): void {
    this.movie = movie;
  }

  private handleMovieError(): void {
    this.loadError = 'Movie not found.';
  }

  private handleHalls(halls: Hall[]): void {
    this.halls = halls;
  }

  private handleHallsError(): void {
    this.halls = [];
  }

  private handleScreenings(screenings: Screening[]): void {
    const list: Screening[] = [];

    if (this.movieId !== null) {
      for (let i = 0; i < screenings.length; i++) {
        const screening = screenings[i];
        if (screening.movieId === this.movieId) {
          list.push(screening);
        }
      }
    }

    this.screenings = list;
  }

  private handleScreeningsError(): void {
    this.screenings = [];
  }
}
