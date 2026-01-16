import { BookingSeat } from './booking-seat';

export interface Booking {
  id: number;
  userId: string;
  screeningId: number;
  createdAt: string;
  status: number;
  totalPrice: number;
  bookingSeats: BookingSeat[];
}
