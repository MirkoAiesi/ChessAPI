using DAL.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories;

public class TournamentRepository : ITournamentRepository
{
    private readonly string _connectionString;

    public TournamentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }
    public async Task<List<Tournament>> GetAllTournament()
    {
        var tournament = new List<Tournament>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "SELECT * FROM tournaments";
        using SqlCommand command = new SqlCommand(query, connection);
        
        await connection.OpenAsync();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        while (reader.Read())
        {
            var addTournament = new Tournament
            {
                TournamentId = Convert.ToInt32(reader["id"]),
                Name = reader["name"].ToString() ?? "",
                Location = reader["location"].ToString() ?? "",
                MinPlayer = Convert.ToInt32(reader["minPlayer"]),
                MaxPlayer = Convert.ToInt32(reader["maxPlayer"]),
                MinElo = Convert.ToInt32(reader["minElo"]),
                MaxElo = Convert.ToInt32(reader["maxElo"]),
                Status = reader["status"].ToString() ?? "",
                Round = Convert.ToInt32(reader["round"]),
                WomenOnly = Convert.ToBoolean(reader["womenOnly"]),
                EndRegistration = Convert.ToDateTime(reader["endRegistration"]),
                CreateDate = Convert.ToDateTime(reader["createDate"]),
                UpdateDate = Convert.ToDateTime(reader["updateDate"])
            };

            tournament.Add(addTournament);
        }

        await connection.CloseAsync();
        return tournament;
    }
public async Task<Tournament?> GetTournamentById(int id)
{
    Tournament? tournament = null;

    using SqlConnection connection = new SqlConnection(_connectionString);

    string query = @"
        SELECT 
            t.*, 
            c.id AS categorieId, 
            c.name AS categorieName, 
            c.minAge, 
            c.maxAge,
            pt.playerId AS pt_playerId
        FROM tournaments t
        LEFT JOIN tournament_category tc ON t.id = tc.tournamentId
        LEFT JOIN categories c ON tc.categoryId = c.id
        LEFT JOIN players_tournaments pt ON t.id = pt.tournamentId
        WHERE t.id = @Id";

    using SqlCommand command = new SqlCommand(query, connection);
    command.Parameters.AddWithValue("@Id", id);

    await connection.OpenAsync();

    using SqlDataReader reader = await command.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        if (tournament == null)
        {
            tournament = new Tournament
            {
                TournamentId = Convert.ToInt32(reader["id"]),
                Name = reader["name"].ToString() ?? "",
                Location = reader["location"].ToString() ?? "",
                MinPlayer = Convert.ToInt32(reader["minPlayer"]),
                MaxPlayer = Convert.ToInt32(reader["maxPlayer"]),
                MinElo = reader["minElo"] != DBNull.Value ? Convert.ToInt32(reader["minElo"]) : null,
                MaxElo = reader["maxElo"] != DBNull.Value ? Convert.ToInt32(reader["maxElo"]) : null,
                Status = reader["status"].ToString() ?? "",
                Round = Convert.ToInt32(reader["round"]),
                WomenOnly = Convert.ToBoolean(reader["womenOnly"]),
                EndRegistration = Convert.ToDateTime(reader["endRegistration"]),
                CreateDate = Convert.ToDateTime(reader["createDate"]),
                UpdateDate = Convert.ToDateTime(reader["updateDate"]),
                Categories = new List<Categorie>(),
                Players = new List<Player>()
            };
        }
        
        if (reader["categorieId"] != DBNull.Value)
        {
            var categorie = new Categorie
            {
                CategorieId = Convert.ToInt32(reader["categorieId"]),
                Name = reader["categorieName"].ToString() ?? "",
                MinAge = Convert.ToInt32(reader["minAge"]),
                MaxAge = Convert.ToInt32(reader["maxAge"])
            };

            tournament.Categories.Add(categorie);
        }
        if (reader["pt_playerId"] != DBNull.Value)
        {
            var player = new Player
            {
                PlayerId = Convert.ToInt32(reader["pt_playerId"]),
            };

            tournament.Players.Add(player);
        }
    }
    await connection.CloseAsync();

    return tournament;
}

    public void AddTournament(Tournament t)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "INSERT INTO tournaments(name, location, minPlayer, maxPlayer, minElo, maxElo, status, round, womenOnly, endRegistration, createDate, updateDate) " +
                       "VALUES (@name,@location, @minPlayer, @maxPlayer, @minElo, @maxElo, @status, @round, @womenOnly, @endRegistration, @createDate, @updateDate )";
        using SqlCommand command = new SqlCommand(query, connection)
        {
            Parameters =
            {
                new SqlParameter("@name", t.Name),
                new SqlParameter("@location", t.Location),
                new SqlParameter("@minPlayer", t.MinPlayer),
                new SqlParameter("@maxPlayer", t.MaxPlayer),
                new SqlParameter("@minElo", t.MinElo),
                new SqlParameter("@maxElo", t.MaxElo),
                new SqlParameter("@status", t.Status),
                new SqlParameter("@round", t.Round),
                new SqlParameter("@womenOnly", t.WomenOnly),
                new SqlParameter("@endRegistration", t.EndRegistration),
                new SqlParameter("@createDate", t.CreateDate),
                new SqlParameter("@updateDate", t.UpdateDate),
            }
        };
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
    }
    public void RemoveTournament(int id)
    {
        {
            string query = "DELETE FROM tournaments WHERE (Id = @id)";
                
            using SqlConnection connection = new SqlConnection(_connectionString);
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    public async Task<List<Tournament>> GetLastTournament()
    {
        var tournament = new List<Tournament>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "SELECT TOP 10 * FROM tournaments WHERE status != 'Cloturé' ORDER BY updateDate DESC";
        using SqlCommand command = new SqlCommand(query, connection);
        
        await connection.OpenAsync();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        while (reader.Read())
        {
            var addTournament = new Tournament
            {
                TournamentId = Convert.ToInt32(reader["id"]),
                Name = reader["name"].ToString() ?? "",
                Location = reader["location"].ToString() ?? "",
                MinPlayer = Convert.ToInt32(reader["minPlayer"]),
                MaxPlayer = Convert.ToInt32(reader["maxPlayer"]),
                MinElo = Convert.ToInt32(reader["minElo"]),
                MaxElo = Convert.ToInt32(reader["maxElo"]),
                Status = reader["status"].ToString() ?? "",
                Round = Convert.ToInt32(reader["round"]),
                WomenOnly = Convert.ToBoolean(reader["womenOnly"]),
                EndRegistration = Convert.ToDateTime(reader["endRegistration"]),
                CreateDate = Convert.ToDateTime(reader["createDate"]),
                UpdateDate = Convert.ToDateTime(reader["updateDate"])
            };

            tournament.Add(addTournament);
        }

        await connection.CloseAsync();
        return tournament;
    }

    public async Task AddPlayerToTournament(PlayerTournament pt)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = "INSERT INTO players_tournaments (playerId, tournamentId) VALUES (@playerId, @tournamentId)";

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@playerId", pt.PlayerId);
        command.Parameters.AddWithValue("@tournamentId", pt.TournamentId);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        await connection.CloseAsync();
    }
    public async Task<List<Player>> GetPlayersByTournament(int tournamentId)
    {
        var players = new List<Player>();

        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"
        SELECT p.*
        FROM players p
        INNER JOIN players_tournaments pt ON p.id = pt.playerId
        WHERE pt.tournamentId = @tournamentId";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@tournamentId", tournamentId);

        await connection.OpenAsync();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            players.Add(new Player
            {
                PlayerId = Convert.ToInt32(reader["id"]),
            });
        }

        await connection.CloseAsync();
        return players;
    }
    public async Task<bool> IsPlayerRegistered(int playerId, int tournamentId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = "SELECT COUNT(1) FROM players_tournaments WHERE playerId = @playerId AND tournamentId = @tournamentId";

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@playerId", playerId);
        command.Parameters.AddWithValue("@tournamentId", tournamentId);

        await connection.OpenAsync();

        int count = (int)await command.ExecuteScalarAsync();
        await connection.CloseAsync();
        return count > 0;
    }

    public async Task AddCategorieToTournament(int categoryId, int tournamentId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"INSERT INTO tournament_category (tournamentId, categoryId) 
                     VALUES (@tournamentId, @categoryId)";

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@tournamentId", tournamentId);
        command.Parameters.AddWithValue("@categoryId", categoryId);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        await connection.CloseAsync();
    }
    public async Task<bool> IsCategorieLinked(int categoryId, int tournamentId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"SELECT COUNT(1) 
                     FROM tournament_category 
                     WHERE tournamentId = @tournamentId 
                     AND categoryId = @categoryId";

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@tournamentId", tournamentId);
        command.Parameters.AddWithValue("@categoryId", categoryId);

        await connection.OpenAsync();

        int count = (int)await command.ExecuteScalarAsync();
        await connection.CloseAsync();
        return count > 0;
    }

    public async Task RemovePlayerToTournament(int playerId, int tournamentId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "DELETE FROM players_tournaments WHERE playerId = @playerId AND tournamentId = @tournamentId";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@playerId", playerId);
        command.Parameters.AddWithValue("@tournamentId", tournamentId);
        await connection.OpenAsync();
        command.ExecuteNonQuery();
        await connection.CloseAsync();
    }

    public async Task StartTournament(int tournamentId, List<Match> matchs)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        using SqlTransaction transaction = connection.BeginTransaction();
        try
        {
            string query = "UPDATE tournaments SET round = 1, updateDate = @updateDate , status = @status" +
                           " WHERE id = @tournamentId";
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@tournamentId", tournamentId);
                command.Parameters.AddWithValue("@updateDate", DateTime.Now);
                command.Parameters.AddWithValue("@status", "Tournois en cours...");
                await command.ExecuteNonQueryAsync();
            }

            foreach (var match in matchs)
            {
                string insertQuery = "INSERT INTO matchs (tournamentId, player_oneId, player_twoId, result, round)" +
                                     "VALUES (@tournamentId, @playerOne, @playerTwo, @result, @round)";
                using SqlCommand cmd = new SqlCommand(insertQuery, connection, transaction);
                    cmd.Parameters.AddWithValue("@tournamentId", match.MatchTournamentId);
                    cmd.Parameters.AddWithValue("@playerOne", match.PlayerOne);
                    cmd.Parameters.AddWithValue("@playerTwo", match.PlayerTwo);
                    cmd.Parameters.AddWithValue("@result",
                        (object?)match.Result ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@round", match.Round);
                    await cmd.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
        }           
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }

    public async Task ResultByMatch(int idMatch, MatchResult matchResult)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "UPDATE matchs SET result = @result " +
                       "WHERE id = @idMatch";
        using SqlCommand command = new SqlCommand(query, connection);
        await connection.OpenAsync();
        command.Parameters.AddWithValue("@idMatch", idMatch);
        command.Parameters.AddWithValue("@result", matchResult);
        await command.ExecuteNonQueryAsync();
        await connection.CloseAsync();
    }

    public async Task<Match> GetMatchById(int id)
    {
        Match? match = null;
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "SELECT * FROM matchs WHERE id = @id";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        connection.Open();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (reader.Read())
        {
            match = new Match
            {
                MatchId = Convert.ToInt32(reader["id"]),
                MatchTournamentId = Convert.ToInt32(reader ["tournamentId"]),
                PlayerOne = Convert.ToInt32(reader ["player_oneId"]),
                PlayerTwo = Convert.ToInt32(reader ["player_twoId"]),
                Result = (MatchResult)Convert.ToInt32(reader["result"]),
                Round = Convert.ToInt32(reader ["round"])
            };
        }
        await connection.CloseAsync();
        return match;
    }

    public async Task UpdateRoundMatch(int tournamentId, int newRound, string endTournament)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "UPDATE tournaments SET round = @round, updateDate = @updateDate, status = @status " +
                       " WHERE id = @id";
        await connection.OpenAsync();
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", tournamentId);
        command.Parameters.AddWithValue("@round",newRound );
        command.Parameters.AddWithValue("@status",endTournament );
        command.Parameters.AddWithValue("@updateDate", DateTime.Now);
        await command.ExecuteNonQueryAsync();
        await connection.CloseAsync();

    }
    public async Task<List<Match>> GetTournamentByMatch(int id, int currentRound)
    {
        var matches = new List<Match>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "SELECT * FROM matchs WHERE tournamentId = @id AND round = @round";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@round", currentRound);
        await connection.OpenAsync();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        while (reader.Read())
        {
            var addMatch = new Match
            {
                MatchId = Convert.ToInt32(reader["id"]),
                MatchTournamentId = Convert.ToInt32(reader ["tournamentId"]),
                PlayerOne = Convert.ToInt32(reader ["player_oneId"]),
                PlayerTwo = Convert.ToInt32(reader ["player_twoId"]),
                Result = (MatchResult)Convert.ToInt32(reader["result"]),
                Round = Convert.ToInt32(reader ["round"])
            };
            matches.Add(addMatch);
        }
        await connection.CloseAsync();
        return matches;
    }

    public async Task<List<Scoreboard>> GetScoreboard(int tournamentId, int round)
    {
        List<Scoreboard> results = new List<Scoreboard>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = " SELECT p.id, p.pseudo, COUNT(m.id) AS matchesPlayed, " +
                       "SUM ( CASE WHEN (m.player_oneId = p.id and m.result = 1) " +
                       "OR (m.player_twoId = p.id AND m.result = 2) " +
                       "THEN 1 ELSE 0 END) AS wins, " +
                       "SUM ( CASE WHEN (m.player_oneId = p.id and m.result = 2) " +
                       "OR (m.player_twoId = p.id AND m.result = 1) " +
                       "THEN 1 ELSE 0 END) AS losses, " +
                       "SUM ( CASE WHEN (m.result = 3) " +
                       "THEN 1 ELSE 0 END) as draws, " +
                       "SUM ( CASE WHEN (m.player_oneId = p.id and m.result = 1) " +
                       "OR (m.player_twoId = p.id AND m.result = 2) " +
                       "THEN 1 WHEN m.result = 3 THEN 0.5 ELSE 0 END) AS score " +
                       "FROM players AS p "+
                       "INNER JOIN players_tournaments AS pt ON p.id = pt.playerId " +
                       "INNER JOIN matchs AS m ON pt.tournamentId = m.tournamentId " +
                       "AND (m.player_oneId = p.id OR m.player_twoId = p.id) " +
                       "WHERE pt.tournamentId = @tournamentId " +
                       "AND m.round = @round " +
                       "GROUP BY p.id , p.pseudo ";
        using SqlCommand command = new SqlCommand(query, connection);
        await connection.OpenAsync();
        command.Parameters.AddWithValue("@tournamentId", tournamentId);
        command.Parameters.AddWithValue("@round", round);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(new Scoreboard
            {
                Pseudo = reader["pseudo"].ToString()!,
                MatchesPlayed = Convert.ToInt32(reader["matchesPlayed"]),
                Wins = Convert.ToInt32(reader["wins"]),
                Losses = Convert.ToInt32(reader["losses"]),
                Draws = Convert.ToInt32(reader["draws"]),
                Score = Convert.ToDouble(reader["score"])
            });
        }
        await connection.CloseAsync();
        return results;
    }
}