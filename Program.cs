using INMOBILIARIA_SANTI.Models; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Probar conexión a la base de datos apenas inicia la app
using (var scope = app.Services.CreateScope())
{
    try
    {
        Conexion conexionBD = new Conexion();
        var conexion = conexionBD.ObtenerConexion();

        if (conexion != null)
        {
            Console.WriteLine("Conexión establecida correctamente al iniciar la aplicación.");
            conexion.Close();
            Console.WriteLine("Conexión cerrada correctamente.");
        }
        else
        {
            Console.WriteLine("No se pudo conectar a la base de datos.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error al intentar conectar: " + ex.Message);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

