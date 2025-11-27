import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/user';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  email = '';
  password = '';
  loginStatus = '';
  user: User | null = null;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const current = this.authService.currentUser;
    if (current) {
      this.user = current;
      this.loginStatus = `Du er logget ind som ${current.name}.`;
      this.router.navigate(['/home']);
    }
  }

  onSubmit(): void {
    const result = this.authService.login(this.email, this.password);
    this.user = result.user ?? null;
    this.loginStatus = result.success
      ? `Du er logget ind som ${result.user?.name}.`
      : result.message;

    if (result.success) {
      this.router.navigate(['/home']);
    }
  }
}
