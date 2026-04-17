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
        string query = "INSERT INTO players (pseudo, email, pwd , birthDate, gender, elo)" +
                       " VALUES (@pseudo, @email, @pwd, @birthdate, @gender, @elo)";
        using SqlConnection connection = new SqlConnection(_connectionString);
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            connection.Open();
            command.Parameters.AddWithValue("@pseudo", p.Pseudo);
            command.Parameters.AddWithValue("@email", p.Email);
            command.Parameters.AddWithValue("@pwd", p.Pwd);
            command.Parameters.AddWithValue("@birthdate", p.BirthDate);
            command.Parameters.AddWithValue("@gender", p.Gender);
            command.Parameters.AddWithValue("@elo", p.Elo);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void RemovePlayer(int id)
    {
        throw new NotImplementedException();
    }
    
}