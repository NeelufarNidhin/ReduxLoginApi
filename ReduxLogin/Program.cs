using Microsoft.EntityFrameworkCore;
using ReduxLogin.Data;
using ReduxLogin.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddCors();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JWTService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(options => options
.WithOrigins("http://localhost:3000")
.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials());

app.UseAuthorization();

app.MapControllers();

app.Run();

