import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { AuthResponse } from '../../models/auth-response';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  username = '';
  email = '';
  password = '';
  status = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onSubmit(): void {
    this.status = '';
    this.authService.register(this.username, this.email, this.password).subscribe({
      next: this.handleRegisterSuccess.bind(this),
      error: this.handleRegisterError.bind(this)
    });
  }

  private handleRegisterSuccess(response: AuthResponse): void {
    this.authService.setCurrentUser(response);
    this.status = 'Account created. You are now logged in.';
    this.router.navigate(['/home']);
  }

  private handleRegisterError(): void {
    this.status = 'Registration failed. Check the details and try again.';
  }
}
