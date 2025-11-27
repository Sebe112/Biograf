import { Component } from '@angular/core';
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
export class HomeComponent {
  featured: Movie[] = [];

  get featuredHero(): Movie | null {
    return this.featured.length ? this.featured[0] : null;
  }

  constructor(private movieService: MovieService) {
    this.featured = this.movieService.getFeatured();
  }
}
