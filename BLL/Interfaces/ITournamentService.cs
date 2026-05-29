using Domain.Entities;

namespace BLL.Interfaces;

public interface ITournamentService
{
    Task<List<Tournament>> GetAllTournament();
    Task<Tournament> GetTournamentById(int id);
    void AddTournament(Tournament t);
    void RemoveTournament(int id);
    Task<List<Tournament>> GetLastTournament();
    Task AddPlayerToTournament(int playerId, int tournamentId);
    
    Task<List<Player>> GetPlayersByTournament(int tournamentId);
    Task AddCategorieToTournament(int categoryId, int tournamentId);
    Task RemovePlayerToTournament(int playerId, int tournamentId);
    Task StartTournament(int tournamentId);
    
    Task ResultByMatch(int idMatch);
    Task UpdateRoundMatch(int tournamentId);
    Task<List<Match>> GetTournamentByMatch(int tournamentId, int currentRound);
    Task<List<Scoreboard>> GetScoreboard(int tournamentId);
    Task<List<Scoreboard>> GetScoreboardByRound(int tournamentId, int round);
    Task<bool> RemoveCategorieFromTournament(int categorieId, int tournamentId);
}