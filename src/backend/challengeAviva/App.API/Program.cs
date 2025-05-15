using App.API.Extension;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy
                    .WithOrigins("http://localhost:5173") // URL de tu frontend
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // Servicios de la app
        builder.Services.AddControllers();
        builder.Services.AddProjectDependencies(); // <- tu m�todo de inyecci�n central

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}