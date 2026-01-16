import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../api.config';
import { Genre } from '../models/genre';

export type GenreRequest = {
  name: string;
};

@Injectable({
  providedIn: 'root'
})
export class GenreService {
  constructor(private http: HttpClient) {}

  getAll(): Observable<Genre[]> {
    return this.http.get<Genre[]>(API_BASE_URL + '/api/genres');
  }

  create(request: GenreRequest): Observable<Genre> {
    return this.http.post<Genre>(API_BASE_URL + '/api/genres', request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(API_BASE_URL + '/api/genres/' + id);
  }
}
