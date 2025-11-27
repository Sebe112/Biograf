import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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

  onSubmit(): void {
    this.loginStatus = 'Demo-login: der sendes ingen API-kald endnu. Du kan frit teste UI-flowet.';
    console.log('Login attempt (dummy):', { email: this.email, password: this.password });
  }
}
