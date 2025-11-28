import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MovieService } from '../../services/movie.service';
import { HallService } from '../../services/hall.service';
import { Movie } from '../../models/movie';

@Component({
  selector: 'app-movie-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './movie-detail.component.html',
  styleUrl: './movie-detail.component.css'
})
export class MovieDetailComponent implements OnInit {
  movie?: Movie;
  showtimes: {
    hallId: number;
    hallName: string;
    sound: string;
    screenType?: string;
    times: string[];
    format?: string;
  }[] = [];

  selectedTime = '';
  selectedHallId: number | null = null;
  status = '';
  // Sædegrid for valgt sal (fra hallService), null = gang.
  seatLayout: (string | null)[][] = [];
  // Aktuelt valgte sæder i UI.
  selectedSeats = new Set<string>();
  // Fastlåste sæder per sal/tid (hardkodet dummy).
  reservedByTime: Record<string, string[]> = {};
  // Sæder som brugeren tidligere har “booket” (gemmes i sessionStorage).
  bookedByTime: Record<string, string[]> = {};

  constructor(
    private route: ActivatedRoute,
    private movieService: MovieService,
    private hallService: HallService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.movie = this.movieService.getMovieById(id);
    this.showtimes = this.hallService.getShowtimesForMovie(id);

    const qp = this.route.snapshot.queryParamMap;
    const qpHall = qp.get('hall') ? Number(qp.get('hall')) : null;
    const qpTime = qp.get('time') ?? null;

    if (this.showtimes.length) {
      const fallbackHall = this.showtimes[0].hallId;
      const fallbackTime = this.showtimes[0].times[0];

      const hallToUse = qpHall && this.showtimes.some(s => s.hallId === qpHall) ? qpHall : fallbackHall;
      const hallEntry = this.showtimes.find(s => s.hallId === hallToUse);
      const timeToUse = qpTime && hallEntry?.times.includes(qpTime) ? qpTime : hallEntry?.times[0] ?? fallbackTime;

      this.selectedHallId = hallToUse;
      this.selectedTime = timeToUse;
      this.seatLayout = this.hallService.getSeatLayout(this.selectedHallId);
    }

    // Dummy reserverede sæder per sal|tidspunkt.
    this.reservedByTime = {
      '1|I dag 18:00': ['A1', 'A2', 'B4', 'C5'],
      '1|I dag 21:00': ['B1', 'B2', 'B3'],
      '2|I dag 19:15': ['D2', 'D3'],
      '3|I dag 17:00': ['C3', 'C4'],
      '1|Lørdag 16:30': ['A3', 'A4'],
      '3|Lørdag 19:30': ['E1', 'E2'],
      '3|Lørdag 13:00': ['B5', 'B6', 'C5', 'C6'],
      '3|Søndag 12:00': ['A1', 'A2', 'A3'],
      '1|Søndag 18:15': ['F1', 'F2'],
      '1|Søndag 19:30': ['D5', 'D6'],
      '2|Søndag 14:00': ['C2', 'C3'],
      '1|Søndag 16:00': ['E4', 'E5'],
      '3|Søndag 11:30': ['A7', 'A8']
    };

    const stored = sessionStorage.getItem('dummyBookedSeats');
    if (stored) {
      try {
        this.bookedByTime = JSON.parse(stored);
      } catch {
        this.bookedByTime = {};
      }
    }
  }

  select(hallId: number, time: string): void {
    this.selectedHallId = hallId;
    this.selectedTime = time;
    this.seatLayout = this.hallService.getSeatLayout(hallId);
    this.status = '';
    this.selectedSeats.clear();
  }

  book(): void {
    if (!this.selectedTime) {
      this.status = 'Vælg et tidspunkt først.';
      return;
    }
    if (!this.selectedSeats.size || this.selectedHallId === null) {
      this.status = 'Vælg mindst ét sæde.';
      return;
    }
    const count = this.selectedSeats.size;
    const seatsList = Array.from(this.selectedSeats).sort().join(', ');
    this.status = `Billetter reserveret (dummy) til ${this.selectedTime} i sal ${this.selectedHallId} for ${count} person(er). Sæder: ${seatsList}.`;

    const key = this.key(this.selectedHallId, this.selectedTime);
    const existing = this.bookedByTime[key] ?? [];
    this.bookedByTime[key] = Array.from(new Set([...existing, ...this.selectedSeats]));
    sessionStorage.setItem('dummyBookedSeats', JSON.stringify(this.bookedByTime));

    this.selectedSeats.clear();
  }

  isReserved(seat: string): boolean {
    if (this.selectedHallId === null) return false;
    const key = this.key(this.selectedHallId, this.selectedTime);
    return (this.reservedByTime[key]?.includes(seat) ?? false) ||
      (this.bookedByTime[key]?.includes(seat) ?? false);
  }

  isSelected(seat: string): boolean {
    return this.selectedSeats.has(seat);
  }

  toggleSeat(seat: string): void {
    if (!this.selectedTime || this.selectedHallId === null || this.isReserved(seat)) {
      return;
    }
    this.selectedSeats.has(seat) ? this.selectedSeats.delete(seat) : this.selectedSeats.add(seat);
  }

  private key(hallId: number, time: string): string {
    return `${hallId}|${time}`;
  }
}
