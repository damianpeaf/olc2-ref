

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


// * Agregar controladores
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});


// * Agregar swagger
// * dotnet add package Swashbuckle.AspNetCore
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
// http://localhost:5076/openapi/v1.json
if (app.Environment.IsDevelopment())
{
    // * Agregar swagger
    // app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// * Agregar controladores
app.MapControllers();

app.Run();
