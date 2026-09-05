using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CartService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register EF Core with SQL Server (LocalDB)
builder.Services.AddDbContext<CartDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Register repository and service
builder.Services.AddScoped<CartService.Repositories.ICartRepository, CartService.Repositories.CartRepository>();
builder.Services.AddScoped<CartService.Services.ICartService, CartService.Services.CartService>();
builder.Services.AddScoped<CartService.Repositories.IUserRepository, CartService.Repositories.UserRepository>();
builder.Services.AddScoped<CartService.Services.IUserService, CartService.Services.UserService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// JWT bearer authentication: every request must carry a valid access token,
// and its claims are the only source of truth for the caller's identity.
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                Console.WriteLine($"[JWT] Token received: {(string.IsNullOrEmpty(context.Token) ? "NULL/EMPTY" : context.Token.Substring(0, 20) + "...")}");
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"[JWT] AUTH FAILED: {context.Exception.GetType().Name} - {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("[JWT] Token successfully validated!");
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSection["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["SigningKey"]!)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Dev-only helper: mints a token signed with the same Jwt config above,
    // so local testing never needs the signing key pasted anywhere else.
    app.MapPost("/api/dev/token", (Guid? userId, IConfiguration config) =>
    {
        var resolvedUserId = userId ?? Guid.NewGuid();
        var section = config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(section["SigningKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: section["Issuer"],
            audience: section["Audience"],
            claims: new[] { new Claim(ClaimTypes.NameIdentifier, resolvedUserId.ToString()) },
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return Results.Ok(new
        {
            userId = resolvedUserId,
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
