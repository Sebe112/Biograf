import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Screening } from '../models/screening';
import { ScreeningWithHall } from '../models/screening-with-hall';
import { Seat } from '../models/seat';
import { API_BASE_URL } from '../api.config';

@Injectable({
  providedIn: 'root'
})
export class ScreeningService {
  constructor(private http: HttpClient) {}

  getAll(): Observable<Screening[]> {
    return this.http.get<Screening[]>(API_BASE_URL + '/api/screenings');
  }

  getById(id: number): Observable<Screening> {
    return this.http.get<Screening>(API_BASE_URL + '/api/screenings/' + id);
  }

  getWithHall(id: number): Observable<ScreeningWithHall> {
    return this.http.get<ScreeningWithHall>(API_BASE_URL + '/api/screenings/' + id + '/with-hall');
  }

  getSeats(id: number): Observable<Seat[]> {
    return this.http.get<Seat[]>(API_BASE_URL + '/api/screenings/' + id + '/seats');
  }

  create(request: ScreeningRequest): Observable<Screening> {
    return this.http.post<Screening>(API_BASE_URL + '/api/screenings', request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(API_BASE_URL + '/api/screenings/' + id);
  }
}

export type ScreeningRequest = {
  movieId: number;
  hallId: number;
  startsAt: string;
  endsAt: string;
  basePrice: number;
};
