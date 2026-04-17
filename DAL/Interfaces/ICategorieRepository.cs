using Domain.Entities;

namespace DAL.Interfaces;

public interface ICategorieRepository
{
    Task<List<Categorie>> GetAllCategorie();
    Task<Categorie> GetCategorieById(int id);
    void AddCategorie(Categorie c);
    void UpdateCategorie(int id, Categorie c);
    void RemoveCategorie(int id);
}