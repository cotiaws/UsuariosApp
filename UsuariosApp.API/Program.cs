using Scalar.AspNetCore;
using UsuariosApp.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

if(builder.Environment.IsDevelopment())
    builder.Host.AddSerilogConfig();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerDocConfig();
builder.Services.AddDependencyInjection(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.UseSwaggerDocConfig();
app.MapScalarApiReference(options => {
    options.WithTheme(ScalarTheme.BluePlanet);
});
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }