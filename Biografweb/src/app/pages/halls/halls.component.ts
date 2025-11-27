import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { HallService } from '../../services/hall.service';
import { MovieService } from '../../services/movie.service';
import { Hall } from '../../models/hall';
import { Movie } from '../../models/movie';

type HallProgram = Hall & { program: { movie: Movie; times: string[]; format?: string }[] };

@Component({
  selector: 'app-halls',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './halls.component.html',
  styleUrl: './halls.component.css'
})
export class HallsComponent implements OnInit {
  venue: { name: string; tagline: string; address: string; website: string } | null = null;
  halls: HallProgram[] = [];

  constructor(
    private hallService: HallService,
    private movieService: MovieService
  ) {}

  ngOnInit(): void {
    this.venue = this.hallService.getVenue();

    this.halls = this.hallService.getHalls().map(hall => {
      const program: { movie: Movie; times: string[]; format?: string }[] = [];

      hall.shows.forEach(show => {
        const movie = this.movieService.getMovieById(show.movieId);
        if (movie) {
          program.push({ movie, times: show.times, format: show.format });
        }
      });

      return { ...hall, program };
    });
  }
}
