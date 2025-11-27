import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  email = '';
  password = '';
  loginStatus = '';
  users: User[] = [];
  loggedInUser: User | null = null;

  constructor(private userService: UserService, private authService: AuthService) {
    this.users = this.userService.getUsers();
    this.authService.currentUser$.subscribe(user => this.loggedInUser = user);
  }

  onSubmit(): void {
    const result = this.authService.login(this.email, this.password);
    this.loginStatus = result.message;
    if (result.success) {
      this.email = '';
      this.password = '';
    }
  }

  logout(): void {
    this.authService.logout();
    this.loginStatus = 'Du er logget ud af demoen.';
  }
}
