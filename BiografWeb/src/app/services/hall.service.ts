import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Hall } from '../models/hall';
import { API_BASE_URL } from '../api.config';

export type HallRequest = {
  name: string;
  rows: number;
  columns: number;
};

@Injectable({
  providedIn: 'root'
})
export class HallService {
  constructor(private http: HttpClient) {}

  getAll(): Observable<Hall[]> {
    return this.http.get<Hall[]>(API_BASE_URL + '/api/halls');
  }

  create(request: HallRequest): Observable<Hall> {
    return this.http.post<Hall>(API_BASE_URL + '/api/halls', request);
  }

  update(id: number, request: HallRequest): Observable<void> {
    return this.http.put<void>(API_BASE_URL + '/api/halls/' + id, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(API_BASE_URL + '/api/halls/' + id);
  }
}
