using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(ConfigureSwaggerGen);

// Database
builder.Services.AddDbContext<BiografDbContext>(ConfigureDbContext);

// Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(ConfigureIdentityOptions)
    .AddEntityFrameworkStores<BiografDbContext>()
    .AddDefaultTokenProviders();

// JWT
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? throw new Exception("Jwt:Key missing");
var issuer = jwtSection["Issuer"] ?? throw new Exception("Jwt:Issuer missing");
var audience = jwtSection["Audience"] ?? throw new Exception("Jwt:Audience missing");

builder.Services.AddAuthentication(ConfigureAuthentication)
    .AddJwtBearer(ConfigureJwtBearer);

builder.Services.AddAuthorization();
builder.Services.AddCors(ConfigureCors);

// Repositories
builder.Services.AddScoped<IHall, HallRepository>();
builder.Services.AddScoped<ISeat, SeatRepository>();
builder.Services.AddScoped<IMovie, MovieRepository>();
builder.Services.AddScoped<IGenre, GenreRepository>();
builder.Services.AddScoped<IMovieGenre, MovieGenreRepository>();
builder.Services.AddScoped<IScreening, ScreeningRepository>();
builder.Services.AddScoped<IBooking, BookingRepository>();
builder.Services.AddScoped<IBookingSeat, BookingSeatRepository>();

// Services
builder.Services.AddScoped<JwtTokenService>();

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(ConfigureSwaggerUi);
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed roller
await SeedRolesAsync(app.Services);
await SeedAdminUserAsync(app.Services);

app.Run();

void ConfigureDbContext(DbContextOptionsBuilder options)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
}

void ConfigureIdentityOptions(IdentityOptions options)
{
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
}

void ConfigureAuthentication(AuthenticationOptions options)
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}

void ConfigureJwtBearer(JwtBearerOptions options)
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.FromSeconds(30)
    };
}

void ConfigureSwaggerUi(SwaggerUIOptions options)
{
    options.InjectStylesheet("/swagger-dark.css");
}

void ConfigureSwaggerGen(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options)
{
    var scheme = new OpenApiSecurityScheme();
    scheme.Name = "Authorization";
    scheme.Type = SecuritySchemeType.Http;
    scheme.Scheme = "bearer";
    scheme.BearerFormat = "JWT";
    scheme.In = ParameterLocation.Header;
    scheme.Description = "Paste your JWT token here.";
    options.AddSecurityDefinition("Bearer", scheme);

    var reference = new OpenApiReference();
    reference.Type = ReferenceType.SecurityScheme;
    reference.Id = "Bearer";

    var requirementScheme = new OpenApiSecurityScheme();
    requirementScheme.Reference = reference;

    var requirement = new OpenApiSecurityRequirement();
    requirement.Add(requirementScheme, new List<string>());

    options.AddSecurityRequirement(requirement);
}

void ConfigureCors(CorsOptions options)
{
    options.AddPolicy("Frontend", ConfigureCorsPolicy);
}

void ConfigureCorsPolicy(CorsPolicyBuilder policy)
{
    policy.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod();
}

static async Task SeedRolesAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = ["User", "Admin"];
    foreach (var role in roles)
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
}

static async Task SeedAdminUserAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    var username = "admin";
    var email = "admin@biograf.local";
    var password = "Admin123!";

    var user = await userManager.FindByNameAsync(username);
    if (user == null)
    {
        user = await userManager.FindByEmailAsync(email);
    }

    if (user == null)
    {
        var newUser = new ApplicationUser();
        newUser.UserName = username;
        newUser.Email = email;

        var result = await userManager.CreateAsync(newUser, password);
        if (!result.Succeeded)
        {
            return;
        }

        user = newUser;
    }

    var isAdmin = await userManager.IsInRoleAsync(user, "Admin");
    if (!isAdmin)
    {
        await userManager.AddToRoleAsync(user, "Admin");
    }
}
