using LibraryWebApp.Models;
using LibraryWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure MongoDB settings
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

// Register services
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<BookService>();
builder.Services.AddSingleton<CategoryService>();
builder.Services.AddSingleton<BookingService>();

// Add session support for authentication
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
