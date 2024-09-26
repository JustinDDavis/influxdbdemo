using Coravel;
using app.Invocables;
using app.Services; // You'll create this namespace soon :)

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScheduler();
builder.Services.AddTransient<WriteRandomPlaneAltitudeInvocable>();

builder.Services.AddSingleton<InfluxDBService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Services.UseScheduler(scheduler =>
{
    scheduler.Schedule<WriteRandomPlaneAltitudeInvocable>().EveryFiveSeconds();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
