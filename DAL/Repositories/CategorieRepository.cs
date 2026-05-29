using DAL.Interfaces;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories;

public class CategorieRepository : ICategorieRepository
{
    private readonly string _connectionString;

    public CategorieRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }
    public async Task<List<Categorie>> GetAllCategorie()
    {
        List<Categorie> categories = new List<Categorie>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "SELECT * FROM categories";
        using SqlCommand command = new SqlCommand(query, connection);
        connection.Open();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        while (reader.Read())
        {
            Categorie addCat = new Categorie
            {
                CategorieId = Convert.ToInt32(reader["id"]),
                Name = reader["name"].ToString() ?? "",
                MinAge = Convert.ToInt32(reader["minAge"]),
                MaxAge = Convert.ToInt32(reader["maxAge"])
            };
            categories.Add(addCat);
        }

        return categories;
    }

    public async Task<Categorie> GetCategorieById(int id)
    {
        Categorie? categorie = null;
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "SELECT * FROM categories WHERE id = @id";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        connection.Open();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (reader.Read())
        {
            categorie = new Categorie
            {
                CategorieId = Convert.ToInt32(reader["id"]),
                Name = reader["name"].ToString() ?? "",
                MinAge = Convert.ToInt32(reader["minAge"]),
                MaxAge = Convert.ToInt32(reader["maxAge"])
            };
        }

        return categorie;
    }

    public void AddCategorie(Categorie c)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "INSERT INTO categories(name, minAge, maxAge) VALUES (@name, @minAge, @maxAge)";
        using SqlCommand command = new SqlCommand(query, connection)
        {
            Parameters =
            {
                new SqlParameter("@name", c.Name),
                new SqlParameter("@minAge", c.MinAge),
                new SqlParameter("@maxAge", c.MaxAge)
            }
        };
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
    }

    public void UpdateCategorie(int id, Categorie c)
    {
        throw new NotImplementedException();
    }

    public void RemoveCategorie(int id)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        using SqlTransaction transaction = connection.BeginTransaction();
        try
        {
            // 1. Supprimer les liaisons dans la table de jointure
            using (SqlCommand unlink = new SqlCommand(
                       "DELETE FROM tournament_category WHERE categoryId = @id", connection, transaction))
            {
                unlink.Parameters.AddWithValue("@id", id);
                unlink.ExecuteNonQuery();
            }

            // 2. Supprimer la catégorie
            using (SqlCommand delete = new SqlCommand(
                       "DELETE FROM categories WHERE id = @id", connection, transaction))
            {
                delete.Parameters.AddWithValue("@id", id);
                delete.ExecuteNonQuery();
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
    
}