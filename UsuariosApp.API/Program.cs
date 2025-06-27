using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using UsuariosApp.API.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRouting(map => map.LowercaseUrls = true);
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer(); //Swagger
builder.Services.AddSwaggerGen(); //Swagger

//Injeção de dependência para o DataContext (Entity Framework)
builder.Services.AddDbContext<DataContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("UsuariosApp"))
    );

var app = builder.Build();

app.MapOpenApi();
app.UseSwagger(); //Swagger
app.UseSwaggerUI(); //Swagger
app.MapScalarApiReference(s => s.WithTheme(ScalarTheme.BluePlanet)); //Scalar

app.UseAuthorization();
app.MapControllers();
app.Run();
