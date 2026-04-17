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
        string query = "SELECT name FROM categories";
        using SqlCommand command = new SqlCommand(query, connection);
        connection.Open();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        while (reader.Read())
        {
            Categorie addCat = new Categorie
            {
                Name = reader["name"].ToString() ?? ""
            };
            categories.Add(addCat);
        }

        return categories;
    }

    public async Task<Categorie> GetCategorieById(int id)
    {
        Categorie? categorie = null;
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "SELECT name FROM categories WHERE id = @id";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        connection.Open();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (reader.Read())
        {
            categorie = new Categorie
            {
                Name = reader["name"].ToString() ?? ""
            };
        }

        return categorie;
    }

    public void AddCategorie(Categorie c)
    {
        throw new NotImplementedException();
    }

    public void UpdateCategorie(int id, Categorie c)
    {
        throw new NotImplementedException();
    }

    public void RemoveCategorie(int id)
    {
        throw new NotImplementedException();
    }
}