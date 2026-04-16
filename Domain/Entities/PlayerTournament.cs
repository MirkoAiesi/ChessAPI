namespace Domain.Entities;

public class PlayerTournament
{
    public int PlayerId { get; set; }
    public int TournamentId { get; set; }
    public decimal Score { get; set; }
    public int MatchPlayed { get; set; } = 0;
    public int NumberOfVictories { get; set; } = 0;
    public int NumberOfLosses { get; set; } = 0;
    public int NumberOfDraws { get; set; } = 0;
    
}