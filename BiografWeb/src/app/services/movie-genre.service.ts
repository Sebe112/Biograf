import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../api.config';

@Injectable({
  providedIn: 'root'
})
export class MovieGenreService {
  constructor(private http: HttpClient) {}

  add(movieId: number, genreId: number): Observable<void> {
    const url = API_BASE_URL + '/api/movie-genres?movieId=' + movieId + '&genreId=' + genreId;
    return this.http.post<void>(url, {});
  }

  remove(movieId: number, genreId: number): Observable<void> {
    const url = API_BASE_URL + '/api/movie-genres?movieId=' + movieId + '&genreId=' + genreId;
    return this.http.delete<void>(url);
  }
}
