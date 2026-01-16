import { Hall } from './hall';

export interface ScreeningWithHall {
  id: number;
  movieId: number;
  hallId: number;
  startsAt: string;
  endsAt: string;
  basePrice: number;
  hall?: Hall | null;
}
