using Domain.Entities;

namespace BLL.Interfaces;

public interface ICategorieService
{
    Task<List<Categorie>> GetAllCategorie();
    Task<Categorie> GetCategorieById(int id);
    void AddCategorie(Categorie c);
    void UpdateCategorie(int id, Categorie c);
    void RemoveCategorie(int id);
}