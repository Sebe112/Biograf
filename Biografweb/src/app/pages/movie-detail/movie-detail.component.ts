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
  status = '';
  seatRows = ['A', 'B', 'C', 'D', 'E', 'F'];
  seatCols = [1, 2, 3, 4, 5, 6, 7, 8];
  selectedSeats = new Set<string>();
  reservedByTime: Record<string, string[]> = {};
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

    if (this.showtimes.length) {
      this.selectedTime = this.showtimes[0].times[0];
    }

    // Dummy reserverede sæder per tidspunkt
    this.reservedByTime = {
      'I dag 18:00': ['A1', 'A2', 'B4', 'C5'],
      'I dag 21:00': ['B1', 'B2', 'B3'],
      'I dag 17:00': ['C3', 'C4'],
      'I dag 19:15': ['D2', 'D3'],
      'Lørdag 16:30': ['A3', 'A4'],
      'Lørdag 19:30': ['E1', 'E2'],
      'Lørdag 13:00': ['B5', 'B6', 'C5', 'C6'],
      'Søndag 12:00': ['A1', 'A2', 'A3'],
      'Søndag 18:15': ['F1', 'F2'],
      'Søndag 19:30': ['D5', 'D6'],
      'Søndag 14:00': ['C2', 'C3'],
      'Søndag 16:00': ['E4', 'E5'],
      'Søndag 11:30': ['A7', 'A8']
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

  select(time: string): void {
    this.selectedTime = time;
    this.status = '';
    this.selectedSeats.clear();
  }

  book(): void {
    if (!this.selectedTime) {
      this.status = 'Vælg et tidspunkt først.';
      return;
    }
    if (!this.selectedSeats.size) {
      this.status = 'Vælg mindst ét sæde.';
      return;
    }
    const count = this.selectedSeats.size;
    const seatsList = Array.from(this.selectedSeats).sort().join(', ');
    this.status = `Billetter reserveret (dummy) til ${this.selectedTime} for ${count} person(er). Sæder: ${seatsList}.`;

    const existing = this.bookedByTime[this.selectedTime] ?? [];
    this.bookedByTime[this.selectedTime] = Array.from(new Set([...existing, ...this.selectedSeats]));
    sessionStorage.setItem('dummyBookedSeats', JSON.stringify(this.bookedByTime));

    this.selectedSeats.clear();
  }

  isReserved(seat: string): boolean {
    return (this.reservedByTime[this.selectedTime]?.includes(seat) ?? false) ||
      (this.bookedByTime[this.selectedTime]?.includes(seat) ?? false);
  }

  isSelected(seat: string): boolean {
    return this.selectedSeats.has(seat);
  }

  toggleSeat(seat: string): void {
    if (!this.selectedTime || this.isReserved(seat)) {
      return;
    }
    if (this.selectedSeats.has(seat)) {
      this.selectedSeats.delete(seat);
    } else {
      this.selectedSeats.add(seat);
    }
  }
}
