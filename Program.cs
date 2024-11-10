using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.HttpOverrides;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<CarWorkshopContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.WebHost.ConfigureKestrel(serverOptions => {
    serverOptions.ListenAnyIP(5001, listenOptions => {
        var certPath = Environment.GetEnvironmentVariable("CERTIFICATE_PATH") ?? "/certs/certificate.crt";
        var keyPath = Environment.GetEnvironmentVariable("CERTIFICATE_KEY_PATH") ?? "/certs/certificate.key";
        
        try {
            var certificate = X509Certificate2.CreateFromPemFile(certPath, keyPath);
            listenOptions.UseHttps(certificate);
        }
        catch (Exception ex) {
            Console.WriteLine($"Error configuring HTTPS: {ex.Message}");
            throw;
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.Run();