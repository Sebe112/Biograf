export interface HallShow {
  movieId: number;
  times: string[];
  format?: string; // fx Dansk Tale, 3D
}

export interface Hall {
  id: number;
  name: string;
  seats: number;
  sound: string;
  screenType?: string;
  shows: HallShow[];
}
