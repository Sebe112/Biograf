Configuration
Update the connection string and JWT settings:
- `Biograf.Api/appsettings.json`

Example:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=BiografDb;Trusted_Connection=True;TrustServerCertificate=True"
},
"Jwt": {
  "Key": "your-secret-key",
  "Issuer": "Biograf.Api",
  "Audience": "BiografWeb",
  "ExpiresMinutes": "120"
}
```

dotnet ef database update --project Biograf.Infrastructure --startup-project Biograf.Api
dotnet run --project Biograf.Api

Swagger will be available at:
https://localhost:<port>/swagger

cd BiografWeb
npm install
ng serve

Frontend runs on:
http://localhost:4200

cd Biograf.test
dotnet test
