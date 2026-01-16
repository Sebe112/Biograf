export interface AuthResponse {
  token: string;
  expiresAtUtc: string;
  userId: string;
  username: string;
  roles: string[];
}
