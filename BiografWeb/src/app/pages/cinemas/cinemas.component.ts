import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CinemaService } from '../../services/cinema.service';
import { MovieService } from '../../services/movie.service';
import { Cinema } from '../../models/cinema';
import { Movie } from '../../models/movie';

type CinemaProgram = Cinema & { program: { movie: Movie; times: string[] }[] };

@Component({
  selector: 'app-cinemas',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cinemas.component.html',
  styleUrl: './cinemas.component.css'
})
export class CinemasComponent implements OnInit {
  cinemas: CinemaProgram[] = [];

  constructor(
    private cinemaService: CinemaService,
    private movieService: MovieService
  ) {}

  ngOnInit(): void {
    this.cinemas = this.cinemaService.getCinemas()
      .map(cinema => {
        const program = cinema.movies
          .map(show => {
            const movie = this.movieService.getMovieById(show.movieId);
            if (!movie) {
              return undefined;
            }
            return { movie, times: show.times };
          })
          .filter((entry): entry is { movie: Movie; times: string[] } => Boolean(entry));

        return { ...cinema, program };
      });
  }
}
