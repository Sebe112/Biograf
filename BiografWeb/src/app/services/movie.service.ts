import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Movie } from '../models/movie';
import { API_BASE_URL } from '../api.config';

export type MovieRequest = {
  title: string;
  description?: string | null;
  durationMinutes: number;
};

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  constructor(private http: HttpClient) {}

  getAll(): Observable<Movie[]> {
    return this.http.get<Movie[]>(API_BASE_URL + '/api/movies');
  }

  getById(id: number): Observable<Movie> {
    return this.http.get<Movie>(API_BASE_URL + '/api/movies/' + id);
  }

  create(request: MovieRequest): Observable<Movie> {
    return this.http.post<Movie>(API_BASE_URL + '/api/movies', request);
  }

  update(id: number, request: MovieRequest): Observable<void> {
    return this.http.put<void>(API_BASE_URL + '/api/movies/' + id, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(API_BASE_URL + '/api/movies/' + id);
  }
}
