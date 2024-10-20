using Microsoft.EntityFrameworkCore;
using ServerBuilding.Models;
using ServerBuilding.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//Read connection string from app settings.json


// script 1 to add
// אתחול מחרוזת חיבור למסד הנתונים
string connectionString = "Server = (localdb)\\MSSQLLocalDB;Initial Catalog=TamiDB;User ID=TaskAdminLogin;Password=kukuPassword;";

// script 2 to add
//Add Database to dependency injection
builder.Services.AddDbContext<TamiDBContext>(
        options => options.UseSqlServer(connectionString));

// script 1 for login
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = true;
});

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
app.UseStaticFiles();
//script 2 for login
app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();
