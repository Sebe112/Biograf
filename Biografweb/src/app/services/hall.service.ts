import { Injectable } from '@angular/core';
import { Hall } from '../models/hall';

@Injectable({
  providedIn: 'root'
})
export class HallService {
  private venue = {
    name: 'Empire Bio',
    tagline: 'Nørrebro-lounge med fokus på kuraterede titler og roligt foyer-liv.',
    address: 'Guldbergsgade 29F, 2200 København N',
    website: 'https://www.empirebio.dk/',
  };

  // Dummy hall-setup inspireret af Empire Bio / Palads typer sale.
  private halls: Hall[] = [
    {
      id: 1,
      name: 'Sal 1 – Lounge',
      seats: 139,
      sound: 'Dolby 7.1',
      screenType: 'Scope',
      shows: [
        { movieId: 4, times: ['I dag 18:00', 'I dag 21:00'], format: 'Atmos' },
        { movieId: 1, times: ['Lørdag 16:30'] },
        { movieId: 6, times: ['Søndag 19:30'], format: 'IMAX' }
      ]
    },
    {
      id: 2,
      name: 'Sal 2 – Balkon',
      seats: 92,
      sound: 'Dolby 5.1',
      screenType: 'Flat',
      shows: [
        { movieId: 2, times: ['I dag 19:15', 'Lørdag 20:00'] },
        { movieId: 3, times: ['Søndag 14:00'] },
        { movieId: 5, times: ['Lørdag 18:00'] }
      ]
    },
    {
      id: 3,
      name: 'Sal 3 – Art',
      seats: 48,
      sound: 'Dolby 5.1',
      screenType: 'Flat',
      shows: [
        { movieId: 3, times: ['I dag 17:00', 'Lørdag 19:30'] },
        { movieId: 1, times: ['Søndag 18:15'] },
        { movieId: 7, times: ['Lørdag 13:00'], format: 'Danish VO' },
        { movieId: 9, times: ['Søndag 12:00'], format: 'DK Tale' }
      ]
    }
  ];

  getVenue() {
    return this.venue;
  }

  getHalls(): Hall[] {
    return this.halls;
  }

  getShowtimesForMovie(movieId: number) {
    return this.halls
      .map(hall => {
        const show = hall.shows.find(s => s.movieId === movieId);
        if (!show) return null;
        return {
          hallId: hall.id,
          hallName: hall.name,
          sound: hall.sound,
          screenType: hall.screenType,
          times: show.times,
          format: show.format
        };
      })
      .filter(Boolean) as {
        hallId: number;
        hallName: string;
        sound: string;
        screenType?: string;
        times: string[];
        format?: string;
      }[];
  }
}
