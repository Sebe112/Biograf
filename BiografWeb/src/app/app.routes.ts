import { Routes } from '@angular/router';

import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { HallsComponent } from './pages/halls/halls.component';
import { MovieDetailComponent } from './pages/movie-detail/movie-detail.component';
import { AdminComponent } from './pages/admin/admin.component';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'halls', component: HallsComponent },
  { path: 'movie/:id', component: MovieDetailComponent },
  { path: 'admin', component: AdminComponent },
  { path: 'login', component: LoginComponent },

  { path: '**', redirectTo: 'home' }
];
