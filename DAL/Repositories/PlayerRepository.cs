using DAL.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly string _connectionString;

    public PlayerRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }
    public async Task<List<Player>> GetAllPlayer()
    {
        var player = new List<Player>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "SELECT * FROM players";
        using SqlCommand command = new SqlCommand(query, connection);
        
        connection.Open();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        while (reader.Read())
        {
            var addPlayer = new Player
            {
                PlayerId = Convert.ToInt32(reader["id"]),
                Pseudo = reader["pseudo"].ToString() ?? "",
                Email = reader["email"].ToString() ?? "",
                BirthDate = DateOnly.FromDateTime((DateTime)reader["birthDate"]),
                Gender = reader["gender"].ToString() ??"",
                Elo = Convert.ToInt32(reader["elo"])
            };

            player.Add(addPlayer);
        }

        return player;
    }

    public async Task<Player?> GetById(int id)
    {
        Player? player = null;
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "SELECT * FROM players WHERE Id = @Id";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        connection.Open();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (reader.Read())
        {
            player = new Player
            {
                PlayerId = Convert.ToInt32(reader["id"]),
                Pseudo = reader["pseudo"].ToString() ?? "",
                Email = reader["email"].ToString() ?? "",
                BirthDate = DateOnly.FromDateTime((DateTime)reader["birthDate"]),
                Gender = reader["gender"].ToString() ??"",
                Elo = Convert.ToInt32(reader["elo"])
            };
        }
        return player;
    }

    public void AddPlayer(Player p)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string queryVerif = "SELECT COUNT(*) FROM players WHERE pseudo = @pseudo OR email = @email";
        using SqlCommand commandVerif = new SqlCommand(queryVerif, connection)
        {
            Parameters =
            {
                new SqlParameter("@pseudo", p.Pseudo),
                new SqlParameter("@email", p.Email)
            }
        };
        connection.Open();
        int count = (int)commandVerif.ExecuteScalar();
        if (count > 0)
        {
            throw new InvalidOperationException("Pseudo ou email est déjà utilisé");
        }
        string query = "INSERT INTO players (pseudo, email, pwd , birthDate, gender, elo)" +
                       " VALUES (@pseudo, @email, @pwd, @birthdate, @gender, @elo)";
        using SqlCommand command = new SqlCommand(query, connection)
        {
            Parameters =
            {
                new SqlParameter("@pseudo", p.Pseudo),
                new SqlParameter("@email", p.Email),
                new SqlParameter("@pwd", p.Pwd),
                new SqlParameter("@birthdate", p.BirthDate),
                new SqlParameter("@gender", p.Gender),
                new SqlParameter("@elo", p.Elo)
            }
        };
        command.ExecuteNonQuery();
        connection.Close();
    }

    public void RemovePlayer(int id)
    {
        throw new NotImplementedException();
    }
    
}