using Microsoft.AspNetCore.Authentication.Cookies;
using Filmify.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Repositories (your custom services)
builder.Services.AddTransient<IRepositorioUsuarios, RepositorioUsuarios>();
builder.Services.AddTransient<IRepositorioHistorialConsultas, RepositorioHistorialConsultas>();
builder.Services.AddTransient<IRepositorioPacientes, RepositorioPacientes>();
builder.Services.AddTransient<IRepositorioConsultas, RepositorioConsultas>();
builder.Services.AddTransient<IRepositorioDoctores, RepositorioDoctores>();
builder.Services.AddTransient<IRepositorioPagos, RepositorioPagos>();
builder.Services.AddTransient<IRepositorioEspecialidades, RepositorioEspecialidades>();



//  Add Authentication + Cookie scheme
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/LoginRegistro/Login";      // redirect if not logged in
        options.LogoutPath = "/LoginRegistro/Logout";    // optional logout path
        options.AccessDeniedPath = "/Home/Index";  // optional denied page
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//  Important: authentication BEFORE authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
