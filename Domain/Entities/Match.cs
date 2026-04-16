using System.ComponentModel.DataAnnotations;
using Domain.Enum;

namespace Domain.Entities;

public class Match
{
    public int MatchId { get; set; }
    public int MatchTournamentId { get; set; }
    public int PlayerOne { get; set; }
    public int PlayerTwo { get; set; }
    public MatchResult Result { get; set; } = MatchResult.NotPlayed;
    public int Round { get; set; }
}