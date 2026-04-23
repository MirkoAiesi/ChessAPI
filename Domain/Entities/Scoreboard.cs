using Domain.Enum;

namespace Domain.Entities;

public class Scoreboard
{
    public string Pseudo { get; set; } = "";
    public int MatchesPlayed { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Draws { get; set; }
    public double Score { get; set; }
}