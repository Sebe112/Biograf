import { Injectable } from '@angular/core';
import { Cinema } from '../models/cinema';

@Injectable({
  providedIn: 'root'
})
export class CinemaService {
  private cinemas: Cinema[] = [
    {
      id: 1,
      name: 'Nordisk Film Palads',
      city: 'København',
      address: 'Axeltorv 9, 1609 København V',
      screens: 17,
      movies: [
        { movieId: 1, times: ['I dag 18:00', 'I dag 21:15', 'Lørdag 16:00'] },
        { movieId: 4, times: ['I morgen 19:00', 'Søndag 20:30'] }
      ]
    },
    {
      id: 2,
      name: 'Biocity Aarhus',
      city: 'Aarhus',
      address: 'Skt. Knuds Torv 15, 8000 Aarhus',
      screens: 10,
      movies: [
        { movieId: 2, times: ['I dag 20:45', 'Lørdag 19:00'] },
        { movieId: 3, times: ['Fredag 18:15', 'Søndag 14:30'] }
      ]
    },
    {
      id: 3,
      name: 'Kinopalæet Frederikshavn',
      city: 'Frederikshavn',
      address: 'Danmarksgade 33, 9900 Frederikshavn',
      screens: 6,
      movies: [
        { movieId: 1, times: ['I dag 17:30'] },
        { movieId: 3, times: ['Lørdag 20:00'] }
      ]
    }
  ];

  getCinemas(): Cinema[] {
    return this.cinemas;
  }
}
