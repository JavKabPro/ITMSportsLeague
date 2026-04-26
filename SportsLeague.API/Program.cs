using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.DataAccess.Repositories;
using SportsLeague.Domain.Helpers;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Repositories.SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using SportsLeague.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Entity Framework Core ──
builder.Services.AddDbContext<LeagueDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Repositories ──
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IRefereeRepository, RefereeRepository>();           // NUEVO
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();     // NUEVO
builder.Services.AddScoped<ITournamentTeamRepository, TournamentTeamRepository>(); // NUEVO
builder.Services.AddScoped<ISponsorRepository, SponsorRepository>();            // NUEVO (Parcial 2)
builder.Services.AddScoped<ITournamentSponsorRepository, TournamentSponsorRepository>(); // NUEVO (Parcial 2)
builder.Services.AddScoped<IMatchRepository, MatchRepository>();                // NUEVO FASE 4
builder.Services.AddScoped<IMatchResultRepository, MatchResultRepository>(); // NUEVO FASE 5
builder.Services.AddScoped<IGoalRepository, GoalRepository>();          // NUEVO FASE 5
builder.Services.AddScoped<ICardRepository, CardRepository>();          // NUEVO FASE 5



// ── Services ──
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IRefereeService, RefereeService>();           // NUEVO
builder.Services.AddScoped<ITournamentService, TournamentService>();     // NUEVO
builder.Services.AddScoped<ISponsorService, SponsorService>();          // NUEVO (Parcial 2)
builder.Services.AddScoped<IMatchService, MatchService>();              // NUEVO FASE 4
builder.Services.AddScoped<IMatchEventService, MatchEventService>();    // NUEVO FASE 5
builder.Services.AddScoped<MatchValidationHelper>();                    // NUEVO FASE 5



// ── AutoMapper ──
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// ── Controllers ──
builder.Services.AddControllers();

// ── Swagger ──
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ── Middleware Pipeline ──
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
