import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { Observable, of } from 'rxjs';

import { MoviesComponent } from './movies.component';
import { MovieService } from '../../services/movie.service';
import { Movie } from '../../models/movie';

class FakeMovieService {
  getAll(): Observable<Movie[]> {
    return of([]);
  }
}

describe('MoviesComponent', () => {
  let component: MoviesComponent;
  let fixture: ComponentFixture<MoviesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MoviesComponent],
      providers: [
        provideRouter([]),
        { provide: MovieService, useClass: FakeMovieService }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MoviesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
