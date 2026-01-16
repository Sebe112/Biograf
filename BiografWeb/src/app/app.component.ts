import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';
import { AuthResponse } from './models/auth-response';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Biograf';

  constructor(private authService: AuthService) {}

  get currentUser$() {
    return this.authService.currentUser$;
  }

  isAdmin(user: AuthResponse | null): boolean {
    if (!user || !user.roles) {
      return false;
    }

    return user.roles.indexOf('Admin') >= 0;
  }

  logout(): void {
    this.authService.logout();
  }
}
