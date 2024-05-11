using Cards.Data;
using Cards.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CardsDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("CardsConnectionString")));

builder.Services.AddScoped<INumericDataRepository, SQLNumericDataRepository>(); //injectionNumericDataRepository with th implemenetation SQLNumericDAataREpository

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
