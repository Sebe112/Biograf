import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MovieService } from '../../services/movie.service';
import { Movie } from '../../models/movie';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  featured: Movie[] = [];

  constructor(private movieService: MovieService) {}

  ngOnInit(): void {
    this.movieService.getAll().subscribe({
      next: this.handleMovies.bind(this),
      error: this.handleMoviesError.bind(this)
    });
  }

  private handleMovies(movies: Movie[]): void {
    const items: Movie[] = [];

    for (let i = 0; i < movies.length && i < 4; i++) {
      items.push(movies[i]);
    }

    this.featured = items;
  }

  private handleMoviesError(): void {
    this.featured = [];
  }
}
