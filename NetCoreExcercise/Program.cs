using Microsoft.EntityFrameworkCore;
using NetCoreExercise.Data;
using NetCoreExercise.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Getting database connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adding database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseCors(e =>
{
    e.AllowAnyHeader();
    e.AllowAnyMethod();
    e.AllowAnyOrigin();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();