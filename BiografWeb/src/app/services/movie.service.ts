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
    },
    {
      id: 5,
      title: 'Barbie',
      description: 'Greta Gerwigs farverige metakomedierejse fra Barbieland til virkeligheden.',
      durationMinutes: 114,
      ageRating: '7',
      genres: ['Comedy', 'Adventure'],
      posterUrl: 'https://image.tmdb.org/t/p/w500/iuFNMS8U5cb6xfzi51Dbkovj7vM.jpg',
      showtimes: ['I dag 18:15', 'Lørdag 20:45']
    },
    {
      id: 6,
      title: 'Oppenheimer',
      description: 'Christopher Nolans IMAX-epos om skabelsen af atombomben.',
      durationMinutes: 180,
      ageRating: '15',
      genres: ['Drama', 'History'],
      posterUrl: 'https://image.tmdb.org/t/p/w500/bjBeCOvR2vcNQZPgoE5pEu4eMiU.jpg',
      showtimes: ['I dag 21:15', 'Søndag 16:00']
    },
    {
      id: 7,
      title: 'Wonka',
      description: 'Musikalsk eventyr om den unge Willy Wonka før chokoladeimperiet.',
      durationMinutes: 116,
      ageRating: '7',
      genres: ['Family', 'Fantasy'],
      posterUrl: 'https://image.tmdb.org/t/p/w500/qhb1qOilapbapxWQn9jtRCMwXJF.jpg',
      showtimes: ['Lørdag 13:00', 'Søndag 11:30']
    },
    {
      id: 8,
      title: 'Napoleon',
      description: 'Ridley Scott følger Napoleons magt og fald med episke slag.',
      durationMinutes: 158,
      ageRating: '15',
      genres: ['Drama', 'War'],
      posterUrl: 'https://image.tmdb.org/t/p/w500/ncKCQVXgk4BcQV6XbvesgZ2zLvZ.jpg',
      showtimes: ['Fredag 20:00', 'Søndag 18:30']
    },
    {
      id: 9,
      title: 'Inside Out 2',
      description: 'Pixars følelsesunivers vender tilbage med nye emotioner i teenagehjernen.',
      durationMinutes: 100,
      ageRating: '7',
      genres: ['Animation', 'Family'],
      posterUrl: 'https://image.tmdb.org/t/p/w500/iuFNMS8U5cb6xfzi51Dbkovj7vM.jpg',
      showtimes: ['Lørdag 15:00', 'Søndag 12:00']
    }
  ];

  constructor() { }

  getMovies(): Movie[] {
    return this.movies;
  }

  getFeatured(count: number = 3): Movie[] {
    return this.movies.slice(0, count);
  }

  getMovieById(id: number): Movie | undefined {
    return this.movies.find(movie => movie.id === id);
  }
}
