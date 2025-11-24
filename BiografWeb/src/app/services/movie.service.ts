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
      posterUrl: 'https://image.tmdb.org/t/p/w500/6FfCtAuVAW8XJjZ7eWeLibRLWTw.jpg'
    },
    {
      id: 2,
      title: 'The Dark Knight',
      description: 'Batman vs. Joker. Mørk og intens superheltefilm.',
      durationMinutes: 152,
      ageRating: '15',
      genres: ['Action', 'Crime'],
      posterUrl: 'https://image.tmdb.org/t/p/w500/qJ2tW6WMUDux911r6m7haRef0WH.jpg'
    },
        {
      id: 3,
      title: 'Lord of the Rings: The Fellowship of the Ring',
      description: 'En episk fantasyrejse gennem Midgård.',
      durationMinutes: 178,
      ageRating: '11',
      genres: ['Fantasy', 'Adventure'],
      posterUrl: 'https://image.tmdb.org/t/p/w500/6oom5QYQ2yQTMJIbnvbkBL9cHo6.jpg'
    }
  ];

  constructor() { }

  getMovies(): Movie[] {
    return this.movies;
  }
}
