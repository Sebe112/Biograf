import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { AuthResponse } from '../../models/auth-response';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  usernameOrEmail = '';
  password = '';
  loginStatus = '';
  currentUser: AuthResponse | null = null;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.currentUser = this.authService.currentUser;
    if (this.currentUser) {
      this.loginStatus = 'Already logged in as ' + this.currentUser.username + '.';
    }
  }

  onSubmit(): void {
    this.loginStatus = '';
    this.authService.login(this.usernameOrEmail, this.password).subscribe({
      next: this.handleLoginSuccess.bind(this),
      error: this.handleLoginError.bind(this)
    });
  }

  private handleLoginSuccess(response: AuthResponse): void {
    this.authService.setCurrentUser(response);
    this.currentUser = response;
    this.loginStatus = 'Logged in as ' + response.username + '.';
    this.router.navigate(['/home']);
  }

  private handleLoginError(): void {
    this.loginStatus = 'Login failed. Check your credentials.';
  }
}
