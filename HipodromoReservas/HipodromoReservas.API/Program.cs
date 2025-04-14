using HipodromoReservas.Application.Services;
using HipodromoReservas.Domain.Interfaces.IRepository;
using HipodromoReservas.Domain.Interfaces.IService;
using HipodromoReservas.Infrastructure.Data;
using HipodromoReservas.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HipodromoReservaContext>(options =>
    options.UseInMemoryDatabase("HipodromoReservaDb"));

builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IWaitingListRepository, WaitingListRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IWaitingListService, WaitingListService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();