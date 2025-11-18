using AseguradoraPTecnica.Business.Interfaces;
using AseguradoraPTecnica.Business.Services;
using AseguradoraPTecnica.Data.Context;
using AseguradoraPTecnica.Data.Interfaces;
using AseguradoraPTecnica.Data.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<DatabaseConnection>(provider =>
    new DatabaseConnection(connectionString ?? ""));

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbConnection = scope.ServiceProvider.GetRequiredService<DatabaseConnection>();
        await dbConnection.TestConnectionAsync();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
