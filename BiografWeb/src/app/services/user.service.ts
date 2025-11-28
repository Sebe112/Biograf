import { Injectable } from '@angular/core';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private users: User[] = [
    { id: 1, name: 'Sara Admin', email: 'sara.admin@biograf.dk', isAdmin: true },
    { id: 2, name: 'Jonas Bruger', email: 'jonas.bruger@biograf.dk', isAdmin: false },
    { id: 3, name: 'Maja Medlem', email: 'maja.medlem@biograf.dk', isAdmin: false }
  ];

  getUsers(): User[] {
    return this.users;
  }

  getAdmins(): User[] {
    return this.users.filter(user => user.isAdmin);
  }

  authenticate(email: string, _password: string): User | null {
    // password er ikke valideret i denne fase.
    return this.users.find(user => user.email.toLowerCase() === email.toLowerCase()) ?? null;
  }
}
