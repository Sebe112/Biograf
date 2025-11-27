export interface CinemaShow {
  movieId: number;
  times: string[];
}

export interface Cinema {
  id: number;
  name: string;
  city: string;
  address: string;
  screens: number;
  movies: CinemaShow[];
}
