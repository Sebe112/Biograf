import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { Observable, of } from 'rxjs';

import { LoginComponent } from './login.component';
import { AuthService } from '../../services/auth.service';
import { AuthResponse } from '../../models/auth-response';

class FakeAuthService {
  currentUser: AuthResponse | null = null;

  login(usernameOrEmail: string, password: string): Observable<AuthResponse> {
    void usernameOrEmail;
    void password;
    return of({
      token: 'test',
      expiresAtUtc: new Date().toISOString(),
      userId: '1',
      username: 'test',
      roles: []
    });
  }

  setCurrentUser(): void {}
}

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoginComponent],
      providers: [
        provideRouter([]),
        { provide: AuthService, useClass: FakeAuthService }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
