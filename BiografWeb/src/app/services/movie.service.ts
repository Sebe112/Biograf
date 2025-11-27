import { Injectable } from '@angular/core';
import { Movie } from '../models/movie';

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  private movies: Movie[] = [
    {
      id: 1,
      title: 'Star Wars: A New Hope',
      description: 'En klassisk rumopera med lyssværd, rumskibe og The Force.',
      durationMinutes: 121,
      ageRating: '11',
      genres: ['Sci-Fi', 'Action'],
      posterUrl: 'https://image.tmdb.org/t/p/w500/6FfCtAuVAW8XJjZ7eWeLibRLWTw.jpg',
      showtimes: ['I dag 19:00', 'I morgen 21:30']
    },
    {
      id: 2,
      title: 'The Dark Knight',
      description: 'Batman vs. Joker. Mørk og intens superheltefilm.',
      durationMinutes: 152,
      ageRating: '15',
      genres: ['Action', 'Crime'],
      posterUrl: 'https://image.tmdb.org/t/p/w500/qJ2tW6WMUDux911r6m7haRef0WH.jpg',
      showtimes: ['I dag 20:15', 'Lørdag 17:45']
    },
    {
      id: 3,
      title: 'Lord of the Rings: The Fellowship of the Ring',
      description: 'En episk fantasyrejse gennem Midgård.',
      durationMinutes: 178,
      ageRating: '11',
      genres: ['Fantasy', 'Adventure'],
      posterUrl: 'https://image.tmdb.org/t/p/w500/6oom5QYQ2yQTMJIbnvbkBL9cHo6.jpg',
      showtimes: ['Fredag 19:00', 'Søndag 14:00']
    },
    {
      id: 4,
      title: 'Dune: Part Two',
      description: 'Paul Atreides samler Fremen og tager kampen mod Harkonnen-huset.',
      durationMinutes: 166,
      ageRating: '13',
      genres: ['Sci-Fi', 'Adventure'],
      posterUrl: 'https://image.tmdb.org/t/p/w500/8b8f1e7lA1QWy0b6a5Bs86lv8qs.jpg',
      showtimes: ['I dag 17:30', 'Mandag 20:00']
    }
  ];

  constructor() { }

  getMovies(): Movie[] {
    return this.movies;
  }

  getFeatured(count: number = 3): Movie[] {
    return this.movies.slice(0, count);
  }
}
