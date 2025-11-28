import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { MovieService } from '../../services/movie.service';
import { HallService } from '../../services/hall.service';
import { Movie } from '../../models/movie';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  featured: Movie[] = [];
  movies: Movie[] = [];
  selectedMovieId: number | null = null;
  showings: {
    hallName: string;
    hallId: number;
    times: string[];
    format?: string;
    screenType?: string;
    sound: string;
  }[] = [];

  constructor(
    private movieService: MovieService,
    private hallService: HallService
  ) {
    this.featured = this.movieService.getFeatured(4);
    this.movies = this.movieService.getMovies();
    if (this.movies.length) {
      this.selectedMovieId = this.movies[0].id;
      this.updateShowings();
    }
  }

  updateShowings(): void {
    if (!this.selectedMovieId) {
      this.showings = [];
      return;
    }
    this.showings = this.hallService.getShowtimesForMovie(this.selectedMovieId);
  }
}
