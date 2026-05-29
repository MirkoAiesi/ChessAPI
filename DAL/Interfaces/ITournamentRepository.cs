using Domain.Entities;
using Domain.Enum;

namespace DAL.Interfaces;

public interface ITournamentRepository
{
    Task<List<Tournament>> GetAllTournament();
    Task<Tournament> GetTournamentById(int id);
    void AddTournament(Tournament t);
    void RemoveTournament(int id);
    Task<List<Tournament>> GetLastTournament();
    Task AddPlayerToTournament(PlayerTournament pt);
    Task<List<Player>> GetPlayersByTournament(int tournamentId);
    Task<bool> IsPlayerRegistered(int playerId, int tournamentId);
    Task AddCategorieToTournament(int categoryId, int tournamentId);
    Task<bool> IsCategorieLinked(int categoryId, int tournamentId);
    Task RemovePlayerToTournament(int playerId, int tournamentId);
    Task StartTournament(int tournamentId, List<Match> matchs);

    Task ResultByMatch(int idMatch, MatchResult matchResult);
    Task<Match> GetMatchById(int id);
    Task UpdateRoundMatch(int tournamentId, int newRound, string endTournament);
    Task<List<Match>> GetTournamentByMatch(int tournamentId, int currentRound);
    //Task<List<Scoreboard>> GetScoreboard(int tournamentId, int round);
    Task<List<Scoreboard>> GetScoreboard(int tournamentId);
    Task<List<Scoreboard>> GetScoreboardByRound(int tournamentId, int round);
    Task<bool> RemoveCategorieFromTournament(int categorieId, int tournamentId);
}