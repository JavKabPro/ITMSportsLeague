using Microsoft.EntityFrameworkCore;
using SportsLeague.Domain.Entities;

namespace SportsLeague.DataAccess.Context
{
    public class LeagueDbContext : DbContext
    {
        public LeagueDbContext(DbContextOptions<LeagueDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Tournament> Tournaments => Set<Tournament>();    // NUEVO (Fase 3)

        public DbSet<Referee> Referees => Set<Referee>();              // NUEVO (Fase 3)
        public DbSet<TournamentTeam> TournamentTeams => Set<TournamentTeam>(); // NUEVO (Fase 3)

        public DbSet<Sponsor> Sponsors { get; set; } // NUEVO (Parcial 2)
        public DbSet<TournamentSponsor> TournamentSponsors { get; set; } // NUEVO (Parcial 2)

        public DbSet<Match> Matches => Set<Match>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(t => t.City)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(t => t.Stadium)
                      .HasMaxLength(150);
                entity.Property(t => t.LogoUrl)
                      .HasMaxLength(500);
                entity.Property(t => t.CreatedAt)
                      .IsRequired();
                entity.Property(t => t.UpdatedAt)
                      .IsRequired(false);
                entity.HasIndex(t => t.Name)
                      .IsUnique();
            });

            // ── Player Configuration ──
            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.FirstName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(p => p.LastName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(p => p.BirthDate)
                      .IsRequired();
                entity.Property(p => p.Number)
                      .IsRequired();
                entity.Property(p => p.Position)
                      .IsRequired();
                entity.Property(p => p.CreatedAt)
                      .IsRequired();
                entity.Property(p => p.UpdatedAt)
                      .IsRequired(false);

                // Relación 1:N con Team
                entity.HasOne(p => p.Team)
                      .WithMany(t => t.Players)
                      .HasForeignKey(p => p.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Índice único compuesto: número de camiseta único por equipo
                entity.HasIndex(p => new { p.TeamId, p.Number })
                      .IsUnique();
            });
            // ── Referee Configuration ──
            modelBuilder.Entity<Referee>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.FirstName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(r => r.LastName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(r => r.Nationality)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(r => r.CreatedAt)
                      .IsRequired();
                entity.Property(r => r.UpdatedAt)
                      .IsRequired(false);
            });
            // ── Tournament Configuration ──
            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(t => t.Season)
                      .IsRequired()
                      .HasMaxLength(20);
                entity.Property(t => t.StartDate)
                      .IsRequired();
                entity.Property(t => t.EndDate)
                      .IsRequired();
                entity.Property(t => t.Status)
                      .IsRequired();
                entity.Property(t => t.CreatedAt)
                      .IsRequired();
                entity.Property(t => t.UpdatedAt)
                      .IsRequired(false);
            });
            // ── TournamentTeam Configuration ──
            modelBuilder.Entity<TournamentTeam>(entity =>
            {
                entity.HasKey(tt => tt.Id);
                entity.Property(tt => tt.RegisteredAt)
                      .IsRequired();
                entity.Property(tt => tt.CreatedAt)
                      .IsRequired();
                entity.Property(tt => tt.UpdatedAt)
                      .IsRequired(false);

                // Relación con Tournament
                entity.HasOne(tt => tt.Tournament)
                      .WithMany(t => t.TournamentTeams)
                      .HasForeignKey(tt => tt.TournamentId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relación con Team
                entity.HasOne(tt => tt.Team)
                      .WithMany(t => t.TournamentTeams)
                      .HasForeignKey(tt => tt.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Índice único compuesto: un equipo solo una vez por torneo
                entity.HasIndex(tt => new { tt.TournamentId, tt.TeamId })
                      .IsUnique();
            });
            base.OnModelCreating(modelBuilder);

            // ── TournamentSponsor Configuration ──
            modelBuilder.Entity<TournamentSponsor>(entity =>
            {
                // 1. Índice Único Compuesto (La clave del parcial)
                // Esto evita que Coca-Cola patrocine dos veces la "Liga 2025-1"
                entity.HasIndex(ts => new { ts.TournamentId, ts.SponsorId }).IsUnique();

                // 2. Relación con Torneo
                entity.HasOne(ts => ts.Tournament)
                      .WithMany(t => t.TournamentSponsors)
                      .HasForeignKey(ts => ts.TournamentId);

                // 3. Relación con Patrocinador
                entity.HasOne(ts => ts.Sponsor)
                      .WithMany(s => s.TournamentSponsors)
                      .HasForeignKey(ts => ts.SponsorId);

                // Configurar el decimal para que SQL no de advertencias de precisión
                entity.Property(ts => ts.ContractAmount).HasPrecision(18, 2);
            });
            // ── Match Configuration ──
            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.MatchDate)
                      .IsRequired();
                entity.Property(m => m.Venue)
                      .HasMaxLength(150);
                entity.Property(m => m.Matchday)
                      .IsRequired();
                entity.Property(m => m.Status)
                      .IsRequired();
                entity.Property(m => m.CreatedAt)
                      .IsRequired();
                entity.Property(m => m.UpdatedAt)
                      .IsRequired(false);

                // Relación con Tournament (Cascade: eliminar torneo elimina partidos)
                entity.HasOne(m => m.Tournament)
                      .WithMany(t => t.Matches)
                      .HasForeignKey(m => m.TournamentId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relación con HomeTeam (Restrict: evita ciclo de cascada)
                entity.HasOne(m => m.HomeTeam)
                      .WithMany(t => t.HomeMatches)
                      .HasForeignKey(m => m.HomeTeamId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relación con AwayTeam (Restrict: evita ciclo de cascada)
                entity.HasOne(m => m.AwayTeam)
                      .WithMany(t => t.AwayMatches)
                      .HasForeignKey(m => m.AwayTeamId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relación con Referee (Restrict: no eliminar árbitro con partidos)
                entity.HasOne(m => m.Referee)
                      .WithMany(r => r.Matches)
                      .HasForeignKey(m => m.RefereeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}

