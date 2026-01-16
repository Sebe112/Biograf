import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { API_BASE_URL } from '../api.config';
import { AuthResponse } from '../models/auth-response';

type LoginRequest = {
  usernameOrEmail: string;
  password: string;
};

type RegisterRequest = {
  username: string;
  email: string;
  password: string;
};

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<AuthResponse | null>;
  currentUser$: Observable<AuthResponse | null>;

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<AuthResponse | null>(this.readStoredUser());
    this.currentUser$ = this.currentUserSubject.asObservable();
  }

  login(usernameOrEmail: string, password: string): Observable<AuthResponse> {
    const payload: LoginRequest = { usernameOrEmail: usernameOrEmail, password: password };
    return this.http.post<AuthResponse>(API_BASE_URL + '/api/auth/login', payload);
  }

  register(username: string, email: string, password: string): Observable<AuthResponse> {
    const payload: RegisterRequest = { username: username, email: email, password: password };
    return this.http.post<AuthResponse>(API_BASE_URL + '/api/auth/register', payload);
  }

  setCurrentUser(response: AuthResponse): void {
    localStorage.setItem('auth', JSON.stringify(response));
    this.currentUserSubject.next(response);
  }

  logout(): void {
    localStorage.removeItem('auth');
    this.currentUserSubject.next(null);
  }

  get currentUser(): AuthResponse | null {
    return this.currentUserSubject.value;
  }

  private readStoredUser(): AuthResponse | null {
    const raw = localStorage.getItem('auth');
    if (!raw) {
      return null;
    }

    try {
      return JSON.parse(raw) as AuthResponse;
    } catch {
      localStorage.removeItem('auth');
      return null;
    }
  }
}
