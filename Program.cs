using inmobiliaria_santi.Models;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios al contenedor
builder.Services.AddControllersWithViews();

// Inyectar IConfiguration para acceder a las configuraciones de appsettings.json
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Registrar los repositorios que vamos a utilizar
builder.Services.AddScoped<RepositorioPropietario>();
builder.Services.AddScoped<RepositorioInquilino>();
builder.Services.AddScoped<RepositorioInmueble>();
builder.Services.AddScoped<RepositorioTipoInmueble>();


var app = builder.Build();

// Configuración del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Configurar la autenticación (si es necesario)
app.UseAuthorization();

// Configuración de rutas del controlador
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
