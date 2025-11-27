import { Injectable } from '@angular/core';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private users: User[] = [
    { id: 1, name: 'Sara Admin', email: 'sara.admin@biograf.dk', isAdmin: true, password: 'admin123' },
    { id: 2, name: 'Jonas Bruger', email: 'jonas.bruger@biograf.dk', isAdmin: false, password: 'hej123' },
    { id: 3, name: 'Maja Medlem', email: 'maja.medlem@biograf.dk', isAdmin: false, password: 'popcorn' }
  ];

  getUsers(): User[] {
    return this.users;
  }

  getAdmins(): User[] {
    return this.users.filter(user => user.isAdmin);
  }

  authenticate(email: string, password: string): User | undefined {
    return this.users.find(user =>
      user.email.toLowerCase() === email.toLowerCase() && user.password === password
    );
  }
}
