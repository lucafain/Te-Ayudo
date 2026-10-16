using TeAyudo.Models;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

BD.ConnectionString = builder.Configuration.GetConnectionString("TeAyudoDb")
    ?? "Server=(localdb)\\MSSQLLocalDB;Database=TeAyudo;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";

var app = builder.Build();

SqlConnectionStringBuilder connectionInfo = new(BD.ConnectionString);
app.Logger.LogInformation("Base de datos activa: {DataSource} / {Database}", connectionInfo.DataSource, connectionInfo.InitialCatalog);

try
{
    BD.EnsureDatabaseReady();
}
catch (Exception ex)
{
    app.Logger.LogWarning(ex, "No se pudo inicializar la base de datos al arrancar. La app seguirá abierta y el error se mostrará al usar funciones que dependan de BD.");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
