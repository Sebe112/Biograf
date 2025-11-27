import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { User } from '../models/user';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(private userService: UserService) {}

  login(email: string, password: string): { success: boolean; message: string; user?: User } {
    const user = (this.userService as any).authenticate?.(email, password) ?? null;

    if (!user) {
      return { success: false, message: 'Forkert email eller kode. Prøv igen.' };
    }

    this.currentUserSubject.next(user);
    return { success: true, message: `Velkommen, ${user.name}!`, user };
  }

  logout(): void {
    this.currentUserSubject.next(null);
  }

  get currentUser(): User | null {
    return this.currentUserSubject.value;
  }
}
