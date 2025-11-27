export interface Movie {
  id: number;
  title: string;
  description: string;
  durationMinutes: number;
  ageRating: string;    // fx "11", "15", "Tilladt for alle"
  genres: string[];     // liste af genrer
  posterUrl?: string;
  showtimes: string[];  // lokale dummy-visningstidspunkter
}
