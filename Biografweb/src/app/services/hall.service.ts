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
      name: 'Sal 1 - Lounge',
      seats: 139,
      sound: 'Lyd',
      screenType: undefined,
      shows: [
        { movieId: 4, times: ['I dag 18:00', 'I dag 21:00'], format: 'Atmos' },
        { movieId: 1, times: ['Lørdag 16:30'] },
        { movieId: 6, times: ['Søndag 19:30'], format: 'IMAX' }
      ]
    },
    {
      id: 2,
      name: 'Sal 2 - Balkon',
      seats: 92,
      sound: 'Lyd',
      screenType: undefined,
      shows: [
        { movieId: 2, times: ['I dag 19:15', 'Lørdag 20:00'] },
        { movieId: 3, times: ['Søndag 14:00'] },
        { movieId: 5, times: ['Lørdag 18:00'] }
      ]
    },
    {
      id: 3,
      name: 'Sal 3 - Art',
      seats: 48,
      sound: 'Lyd',
      screenType: undefined,
      shows: [
        { movieId: 3, times: ['I dag 17:00', 'Lørdag 19:30'] },
        { movieId: 1, times: ['Søndag 18:15'] },
        { movieId: 7, times: ['Lørdag 13:00'], format: 'Danish VO' },
        { movieId: 9, times: ['Søndag 12:00'], format: 'DK Tale' }
      ]
    }
  ];
  // 9 felter per række max med nuværende layout, null = gang
  private seatLayouts: Record<number, (string | null)[][]> = {
    1: [
      ['A1', 'A2', 'A3', null, 'A4', 'A5', 'A6', null, 'A7'],
      ['B1', 'B2', 'B3', null, 'B4', 'B5', 'B6', null, 'B7'],
      ['C1', 'C2', 'C3', null, 'C4', 'C5', 'C6', null, 'C7'],
      ['D1', 'D2', 'D3', null, 'D4', 'D5', 'D6', null, 'D7'],
      ['E1', 'E2', 'E3', null, 'E4', 'E5', 'E6', null, 'E7'],
      ['F1', 'F2', 'F3', null, 'F4', 'F5', 'F6', null, 'F7']
    ],
    2: [
      ['A1', 'A2', null, 'A3', 'A4', null, 'A5', 'A6'],
      ['B1', 'B2', null, 'B3', 'B4', null, 'B5', 'B6'],
      ['C1', 'C2', null, 'C3', 'C4', null, 'C5', 'C6'],
      ['D1', 'D2', null, 'D3', 'D4', null, 'D5', 'D6']
    ],
    3: [
      ['A1', 'A2', 'A3', null, 'A4', 'A5'],
      ['B1', 'B2', 'B3', null, 'B4', 'B5'],
      ['C1', 'C2', 'C3', null, 'C4', 'C5'],
      ['D1', 'D2', 'D3', null, 'D4', 'D5']
    ]
  };

  getVenue() {
    return this.venue;
  }

  getHalls(): Hall[] {
    return this.halls;
  }

  getSeatLayout(hallId: number): (string | null)[][] {
    return this.seatLayouts[hallId] ?? this.seatLayouts[1];
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
