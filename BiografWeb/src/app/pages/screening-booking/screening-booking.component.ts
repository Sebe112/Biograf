import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ScreeningService } from '../../services/screening.service';
import { BookingService } from '../../services/booking.service';
import { AuthService } from '../../services/auth.service';
import { ScreeningWithHall } from '../../models/screening-with-hall';
import { Seat } from '../../models/seat';
import { BookingSeat } from '../../models/booking-seat';
import { Booking } from '../../models/booking';

@Component({
  selector: 'app-screening-booking',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './screening-booking.component.html',
  styleUrl: './screening-booking.component.css'
})
export class ScreeningBookingComponent implements OnInit {
  screeningId: number | null = null;
  screening: ScreeningWithHall | null = null;
  seats: Seat[] = [];
  seatRows: Seat[][] = [];
  bookedSeatIds: number[] = [];
  mySeatIds: number[] = [];
  selectedSeatIds: number[] = [];
  loadError = '';
  status = '';
  loginRequired = false;

  constructor(
    private route: ActivatedRoute,
    private screeningService: ScreeningService,
    private bookingService: BookingService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const raw = this.route.snapshot.paramMap.get('id');
    if (raw) {
      this.screeningId = Number(raw);
    }

    if (this.screeningId === null || Number.isNaN(this.screeningId)) {
      this.loadError = 'Screening not found.';
      return;
    }

    this.loginRequired = this.authService.currentUser === null;

    this.screeningService.getWithHall(this.screeningId).subscribe({
      next: this.handleScreening.bind(this),
      error: this.handleScreeningError.bind(this)
    });

    this.screeningService.getSeats(this.screeningId).subscribe({
      next: this.handleSeats.bind(this),
      error: this.handleSeatsError.bind(this)
    });

    this.bookingService.getBookedSeats(this.screeningId).subscribe({
      next: this.handleBookedSeats.bind(this),
      error: this.handleBookedSeatsError.bind(this)
    });

    if (!this.loginRequired) {
      this.bookingService.getMyBookings().subscribe({
        next: this.handleMyBookings.bind(this),
        error: this.handleMyBookingsError.bind(this)
      });
    }
  }

  onSeatClick(seat: Seat): void {
    if (!this.canSelectSeat(seat)) {
      return;
    }

    if (this.loginRequired) {
      this.status = 'Login required to book seats.';
      return;
    }

    if (this.isSeatSelected(seat.id)) {
      this.removeSeatId(this.selectedSeatIds, seat.id);
      return;
    }

    this.selectedSeatIds.push(seat.id);
  }

  bookSelectedSeats(): void {
    this.status = '';

    if (this.loginRequired) {
      this.status = 'Login required to book seats.';
      return;
    }

    if (this.screeningId === null) {
      this.status = 'Screening not found.';
      return;
    }

    if (this.selectedSeatIds.length === 0) {
      this.status = 'Select at least one seat.';
      return;
    }

    this.bookingService.createBooking(this.screeningId, this.selectedSeatIds).subscribe({
      next: this.handleBookingSuccess.bind(this),
      error: this.handleBookingError.bind(this)
    });
  }

  getSeatClass(seat: Seat): string {
    if (seat.isDisabledSeat) {
      return 'seat disabled';
    }

    if (this.isSeatMine(seat.id)) {
      return 'seat mine';
    }

    if (this.isSeatBookedByOther(seat.id)) {
      return 'seat booked';
    }

    if (this.isSeatSelected(seat.id)) {
      return 'seat selected';
    }

    return 'seat available';
  }

  getSelectedCount(): number {
    return this.selectedSeatIds.length;
  }

  getTotalPrice(): number {
    if (!this.screening || !this.screening.basePrice) {
      return 0;
    }

    return this.screening.basePrice * this.selectedSeatIds.length;
  }

  getRowLabel(row: Seat[]): string {
    if (!row.length) {
      return '';
    }

    const rowIndex = row[0].rowIndex;
    if (rowIndex >= 0 && rowIndex < 26) {
      return String.fromCharCode(65 + rowIndex);
    }

    return 'R' + (rowIndex + 1);
  }

  canSelectSeat(seat: Seat): boolean {
    if (seat.isDisabledSeat) {
      return false;
    }

    if (this.isSeatMine(seat.id)) {
      return false;
    }

    if (this.isSeatBookedByOther(seat.id)) {
      return false;
    }

    return true;
  }

  private handleScreening(screening: ScreeningWithHall): void {
    this.screening = screening;
  }

  private handleScreeningError(): void {
    this.loadError = 'Screening not found.';
  }

  private handleSeats(seats: Seat[]): void {
    this.seats = seats;
    this.buildSeatRows(seats);
  }

  private handleSeatsError(): void {
    this.seats = [];
    this.seatRows = [];
    this.loadError = 'Failed to load seats.';
  }

  private handleBookedSeats(seats: BookingSeat[]): void {
    this.bookedSeatIds = this.mapSeatIds(seats);
  }

  private handleBookedSeatsError(): void {
    this.bookedSeatIds = [];
  }

  private handleMyBookings(bookings: Booking[]): void {
    const list: number[] = [];

    for (let i = 0; i < bookings.length; i++) {
      const booking = bookings[i];
      if (this.screeningId !== null && booking.screeningId !== this.screeningId) {
        continue;
      }

      for (let j = 0; j < booking.bookingSeats.length; j++) {
        list.push(booking.bookingSeats[j].seatId);
      }
    }

    this.mySeatIds = list;
  }

  private handleMyBookingsError(): void {
    this.mySeatIds = [];
  }

  private handleBookingSuccess(): void {
    this.status = 'Booking complete.';
    this.selectedSeatIds = [];

    if (this.screeningId !== null) {
      this.bookingService.getBookedSeats(this.screeningId).subscribe({
        next: this.handleBookedSeats.bind(this),
        error: this.handleBookedSeatsError.bind(this)
      });

      this.bookingService.getMyBookings().subscribe({
        next: this.handleMyBookings.bind(this),
        error: this.handleMyBookingsError.bind(this)
      });
    }
  }

  private handleBookingError(): void {
    this.status = 'Booking failed. Seats might already be taken.';
  }

  private buildSeatRows(seats: Seat[]): void {
    const rows: { [key: number]: Seat[] } = {};
    const rowIndexes: number[] = [];

    for (let i = 0; i < seats.length; i++) {
      const seat = seats[i];
      const rowIndex = seat.rowIndex;

      if (!rows[rowIndex]) {
        rows[rowIndex] = [];
        rowIndexes.push(rowIndex);
      }

      rows[rowIndex].push(seat);
    }

    this.sortNumbers(rowIndexes);

    const result: Seat[][] = [];
    for (let i = 0; i < rowIndexes.length; i++) {
      const rowIndex = rowIndexes[i];
      const rowSeats = rows[rowIndex];
      this.sortSeatsByColumn(rowSeats);
      result.push(rowSeats);
    }

    this.seatRows = result;
  }

  private sortNumbers(values: number[]): void {
    for (let i = 0; i < values.length - 1; i++) {
      for (let j = 0; j < values.length - 1 - i; j++) {
        if (values[j] > values[j + 1]) {
          const temp = values[j];
          values[j] = values[j + 1];
          values[j + 1] = temp;
        }
      }
    }
  }

  private sortSeatsByColumn(seats: Seat[]): void {
    for (let i = 0; i < seats.length - 1; i++) {
      for (let j = 0; j < seats.length - 1 - i; j++) {
        if (seats[j].columnIndex > seats[j + 1].columnIndex) {
          const temp = seats[j];
          seats[j] = seats[j + 1];
          seats[j + 1] = temp;
        }
      }
    }
  }

  private mapSeatIds(seats: BookingSeat[]): number[] {
    const list: number[] = [];

    for (let i = 0; i < seats.length; i++) {
      list.push(seats[i].seatId);
    }

    return list;
  }

  private isSeatSelected(seatId: number): boolean {
    return this.isSeatIdInList(this.selectedSeatIds, seatId);
  }

  private isSeatMine(seatId: number): boolean {
    return this.isSeatIdInList(this.mySeatIds, seatId);
  }

  private isSeatBookedByOther(seatId: number): boolean {
    if (!this.isSeatIdInList(this.bookedSeatIds, seatId)) {
      return false;
    }

    return !this.isSeatMine(seatId);
  }

  private isSeatIdInList(list: number[], seatId: number): boolean {
    for (let i = 0; i < list.length; i++) {
      if (list[i] === seatId) {
        return true;
      }
    }

    return false;
  }

  private removeSeatId(list: number[], seatId: number): void {
    for (let i = 0; i < list.length; i++) {
      if (list[i] === seatId) {
        list.splice(i, 1);
        return;
      }
    }
  }
}
