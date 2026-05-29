using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Categorie> Categories { get; set; }
    public DbSet<Match> Matchs { get; set; }
    public DbSet<PlayerTournament> PlayerTournaments { get; set; }
    public DbSet<TournamentCategorie> TournamentCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.ToTable("tournaments");
            entity.HasKey(t => t.TournamentId);
            entity.Property(t => t.TournamentId).HasColumnName("id");
        });

        modelBuilder.Entity<Categorie>(entity =>
        {
            entity.ToTable("categories");
            entity.HasKey(c => c.CategorieId);
            entity.Property(c => c.CategorieId).HasColumnName("id");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.ToTable("players");
            entity.HasKey(p => p.PlayerId);
            entity.Property(p => p.PlayerId).HasColumnName("id");
    
            // ✅ Conversion DateOnly <-> DateTime
            entity.Property(p => p.BirthDate)
                .HasConversion(
                    d => d.ToDateTime(TimeOnly.MinValue), // DateOnly → DateTime pour SQL
                    d => DateOnly.FromDateTime(d)          // DateTime → DateOnly pour C#
                );
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.ToTable("matchs");
            entity.HasKey(m => m.MatchId);
            entity.Property(m => m.MatchId).HasColumnName("id");
            entity.Property(m => m.MatchTournamentId).HasColumnName("tournamentId");
            entity.Property(m => m.PlayerOne).HasColumnName("player_oneId");
            entity.Property(m => m.PlayerTwo).HasColumnName("player_twoId");
            entity.Property(m => m.Result)
                .HasColumnName("result")
                .HasConversion(
                    v => ((int)v).ToString(), 
                    v => (MatchResult)int.Parse(v)    
                );
            entity.Property(m => m.Round).HasColumnName("round");
            entity.Ignore(m => m.Score);
        });


        // ✅ Jointure Tournament <-> Player via PlayerTournament
        modelBuilder.Entity<Tournament>()
            .HasMany(t => t.Players)
            .WithMany()
            .UsingEntity<PlayerTournament>(
                j => j
                    .HasOne<Player>()
                    .WithMany()
                    .HasForeignKey(pt => pt.PlayerId),
                j => j
                    .HasOne<Tournament>()
                    .WithMany()
                    .HasForeignKey(pt => pt.TournamentId),
                j =>
                {
                    j.ToTable("players_tournaments");
                    j.HasKey(pt => new { pt.PlayerId, pt.TournamentId });
                }
            );
        
        modelBuilder.Entity<Tournament>()
            .HasMany(t => t.Categories)
            .WithMany()
            .UsingEntity<TournamentCategorie>(
                j => j
                    .HasOne<Categorie>()
                    .WithMany()
                    .HasForeignKey(tc => tc.CategoryId),
                j => j
                    .HasOne<Tournament>()
                    .WithMany()
                    .HasForeignKey(tc => tc.TournamentId),
                j =>
                {
                    j.ToTable("tournament_category");
                    j.HasKey(tc => new { tc.TournamentId, tc.CategoryId });
                }
            );
    }
}