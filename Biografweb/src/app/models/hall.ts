export interface HallShow {
  movieId: number;
  times: string[];
  format?: string; // fx Atmos, 70mm
}

export interface Hall {
  id: number;
  name: string;
  seats: number;
  sound: string;
  screenType?: string;
  shows: HallShow[];
}
