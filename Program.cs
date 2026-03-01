using Microsoft.EntityFrameworkCore;
using VideoGameApiVsa.Data;
using Carter;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddCarter();

#region Database configuration

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString != null)
    builder.Services.AddDbContext<VideoGameDbContext>(options => options.UseMySQL(connectionString));

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapCarter();

app.Run();
