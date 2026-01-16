import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../api.config';
import { BookingSeat } from '../models/booking-seat';
import { Booking } from '../models/booking';

type CreateBookingRequest = {
  screeningId: number;
  seatIds: number[];
};

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  constructor(private http: HttpClient) {}

  getBookedSeats(screeningId: number): Observable<BookingSeat[]> {
    return this.http.get<BookingSeat[]>(API_BASE_URL + '/api/bookingseats/by-screening/' + screeningId);
  }

  getMyBookings(): Observable<Booking[]> {
    return this.http.get<Booking[]>(API_BASE_URL + '/api/bookings/my');
  }

  createBooking(screeningId: number, seatIds: number[]): Observable<Booking> {
    const payload: CreateBookingRequest = {
      screeningId: screeningId,
      seatIds: seatIds
    };

    return this.http.post<Booking>(API_BASE_URL + '/api/bookings', payload);
  }

  removeSeat(bookingId: number, seatId: number): Observable<void> {
    const url = API_BASE_URL + '/api/bookingseats/booking/' + bookingId + '/seat/' + seatId;
    return this.http.delete<void>(url);
  }
}
