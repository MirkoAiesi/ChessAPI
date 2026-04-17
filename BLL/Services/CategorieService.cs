using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class CategorieService : ICategorieService
{
    private readonly ICategorieRepository _categorieService;

    public CategorieService(ICategorieRepository categorieService)
    {
        _categorieService = categorieService;
    }
    
    public async Task<List<Categorie>> GetAllCategorie()
    {
        return await _categorieService.GetAllCategorie();
    }

    public async Task<Categorie> GetCategorieById(int id)
    {
        return await _categorieService.GetCategorieById(id);
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