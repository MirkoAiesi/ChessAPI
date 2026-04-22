using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;
using Domain.Enum;

namespace BLL.Services;

public class TournamentService : ITournamentService
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IPlayerRepository _playerRepository;

    public TournamentService(ITournamentRepository tournamentRepository, IPlayerRepository playerRepository)
    {
        _tournamentRepository = tournamentRepository;
        _playerRepository = playerRepository;
    }
    public async Task<List<Tournament>> GetAllTournament()
    {
        return await _tournamentRepository.GetAllTournament();
    }

    public async Task<Tournament> GetTournamentById(int id)
    {
        return await _tournamentRepository.GetTournamentById(id);
    }

    public void AddTournament(Tournament t)
    {
        _tournamentRepository.AddTournament(t);
    }

    public void UpdateTournament(int id, Tournament t)
    {
        throw new NotImplementedException();
    }

    public void RemoveTournament(int id)
    {
        var tournament = _tournamentRepository.GetTournamentById(id);
        if (tournament is null)
        {
            throw new Exception("Aucun tournois n'a été trouvé");
        }

        if (tournament.Status.ToString() != "En attente de joueurs")
        {
            throw new Exception("Impossible de supprimer un tournoi déjà commencé");
        }
        _tournamentRepository.RemoveTournament(id);
    }

    public async Task<List<Tournament>> GetLastTournament()
    {
        return await _tournamentRepository.GetLastTournament();
    }
    private bool IsPlayerEligibleByAge(Player player, Tournament tournament)
    {
        DateOnly referenceDate = DateOnly.FromDateTime(tournament.EndRegistration);

        int age = CalculateAgeAtDate(player.BirthDate, referenceDate);

        return tournament.Categories.Any(c =>
            age >= c.MinAge && age <= c.MaxAge
        );
    }
    private int CalculateAgeAtDate(DateOnly birthDate, DateOnly referenceDate)
    {
        int age = referenceDate.Year - birthDate.Year;

        if (referenceDate < birthDate.AddYears(age))
            age--;

        return age;
    }
    public async Task AddPlayerToTournament(int playerId, int tournamentId)
    {
        var tournament = await _tournamentRepository.GetTournamentById(tournamentId);
        var player = await _playerRepository.GetById(playerId);

        /*if (tournament is null || player is null)
            throw new Exception("Tournois/joueur introuvable");

        if (tournament.Status != "En attente de joueurs")
            throw new Exception("Le tournoi a déjà commencé");

        if (DateTime.UtcNow >= tournament.EndRegistration)
            throw new Exception("Les inscriptions sont fermées");

        if (await _tournamentRepository.IsPlayerRegistered(playerId, tournamentId))
            throw new Exception("Ce joueur est déjà inscrit");

        if (tournament.Players.Count >= tournament.MaxPlayer)
            throw new Exception("Tournoi déjà complet");

        if (player.Elo < tournament.MinElo || player.Elo > tournament.MaxElo)
            throw new Exception("ELO non conforme");

        if (tournament.WomenOnly && player.Gender != "Femme")
            throw new Exception("Tournoi réservé aux femmes");
        DateOnly referenceDate = DateOnly.FromDateTime(tournament.EndRegistration);
        int age = CalculateAgeAtDate(player.BirthDate, referenceDate);

        if (!tournament.Categories.Any(c => age >= c.MinAge && age <= c.MaxAge))
            throw new Exception("Le joueur n'a pas l'âge requis");*/

        var pt = new PlayerTournament
        {
            PlayerId = playerId,
            TournamentId = tournamentId
        };

        await _tournamentRepository.AddPlayerToTournament(pt);
    }

    public async Task<List<Player>> GetPlayersByTournament(int tournamentId)
    {
        return await _tournamentRepository.GetPlayersByTournament(tournamentId);
    }
    
    public async Task AddCategorieToTournament(int categoryId, int tournamentId)
    {
        var tournament = await _tournamentRepository.GetTournamentById(tournamentId);

        if (tournament == null)
            throw new Exception("Tournoi introuvable");

        bool alreadyLinked = await _tournamentRepository.IsCategorieLinked(categoryId, tournamentId);

        if (alreadyLinked)
            throw new Exception("Catégorie déjà associée à ce tournoi");

        await _tournamentRepository.AddCategorieToTournament(categoryId, tournamentId);
    }

    public async Task RemovePlayerToTournament(int playerId, int tournamentId)
    {
        var tournament = await _tournamentRepository.GetTournamentById(tournamentId);
        var player = await _playerRepository.GetById(playerId);

        if (tournament is null || player is null)
            throw new Exception("Tournois/joueur introuvable");
        if (tournament.Status != "En attente de joueurs")
            throw new Exception("Le tournoi a déjà commencé");
        if (!await _tournamentRepository.IsPlayerRegistered(playerId, tournamentId))
            throw new Exception("Ce joueur n'est pas inscrit");

        await _tournamentRepository.RemovePlayerToTournament(playerId, tournamentId);
    }

    public async Task StartTournament(int tournamentId)
    {
        var tournament = await _tournamentRepository.GetTournamentById(tournamentId);
        if (tournament is null)
        {
            throw new Exception("Aucun tournois trouvé");
        }
        if (tournament.Players.Count < tournament.MinPlayer)
        {
            Console.WriteLine("NBR PARTICIPANT" + tournament.Players.Count);
            throw new Exception("Il n'y a pas assez de joueurs pour commencer le tournois");
        }

        if (tournament.EndRegistration > DateTime.Now)
        {
            throw new Exception("Le delais d'inscription est toujours en cours, impossible de commencer le tournois");
        }

        if (tournament.Status != "En attente de joueurs")
        {
            throw new Exception("Le tournois à déjà commencé");
        }
        
        var matchs = GenerateDoubleRoundRobinBerger(tournament.Players);
        foreach (var m in matchs)
            m.MatchTournamentId = tournamentId;
        await _tournamentRepository.StartTournament(tournamentId, matchs);
        Console.WriteLine("MATCH " + matchs.Count);
    }

    private List<Match> GenerateDoubleRoundRobinBerger(List<Player> players)
    {
        var matches = new List<Match>();
        var list = new List<Player>(players);
        if (list.Count % 2 != 0)
            list.Add(null!);

        int n = list.Count;
        int rounds = n - 1;
        Console.WriteLine("LIST COUNT " + n);
        for (int round = 0; round < rounds; round++)
        {
            for (int i = 0; i < n / 2; i++)
            {
                var p1 = list[i];
                var p2 = list[n - 1 - i];

                if (p1 != null && p2 != null)
                {
                    matches.Add(new Match
                    {
                        PlayerOne = p1.PlayerId,
                        PlayerTwo = p2.PlayerId,
                        Round = round + 1
                    });
                    matches.Add(new Match
                    {
                        PlayerOne = p2.PlayerId,
                        PlayerTwo = p1.PlayerId,
                        Round = round + 1 + rounds
                    });
                }
                Console.WriteLine($"Round {round} - {p1?.PlayerId} vs {p2?.PlayerId}");
            }

            RotateBerger(list);
        }

        return matches;
    }

    private void RotateBerger(List<Player> players)
    {
        var fixedPlayer = players[0];
        var last = players[^1];
        players.RemoveAt(players.Count - 1);
        players.Insert(1, last);
        players[0] = fixedPlayer;
    }

    public async Task ResultByMatch(int matchId)
    {
        var matchExist = await _tournamentRepository.GetMatchById(matchId);
        if (matchExist == null)
        {
            throw new Exception("Aucun match trouvé");
        }

        var tournamentRound = await _tournamentRepository.GetTournamentById(matchExist.MatchTournamentId);
        if (tournamentRound.Round != matchExist.Round)
        {
            throw new Exception("Impossible de modifier ce match ");
        }
        var result = GetRandomResult();
        Console.WriteLine("result : " + result);
        await _tournamentRepository.ResultByMatch(matchId, result);
    }
    private MatchResult GetRandomResult()
    {
        var values = Enum.GetValues<MatchResult>();
        return values[Random.Shared.Next(values.Length)];
    }

    public async Task UpdateRoundMatch(int tournamentId)
    {
        var tournament = await _tournamentRepository.GetTournamentById(tournamentId);

        if (tournament == null)
            throw new Exception("Tournoi introuvable");

        int currentRound = tournament.Round;

        var matches = await _tournamentRepository
            .GetTournamentByMatch(tournamentId, currentRound);

        if (!matches.Any())
            throw new Exception("Aucun match trouvé pour cette ronde");

        if (matches.Any(m => m.Result == MatchResult.NotPlayed))
            throw new Exception("Tous les matchs ne sont pas encore terminés");

        await _tournamentRepository.UpdateRoundMatch(tournamentId, currentRound + 1);

    }
}