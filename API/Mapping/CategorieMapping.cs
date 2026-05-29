using API.DTO.CategorieDTO;
using Domain.Entities;

namespace API.Mapping;

public static class CategorieMapping
{
    public static GetCategorieResponse ToResponse(Categorie categorie)
    {
        return new GetCategorieResponse
        {
            CategorieId = categorie.CategorieId,
            Name = categorie.Name,
            MinAge = categorie.MinAge,
            MaxAge = categorie.MaxAge
        };
    }
    public static Categorie ToResponse(GetCategorieResponse request)
    {
        return new Categorie
        {
            Name = request.Name,
            MinAge = request.MinAge,
            MaxAge = request.MaxAge
        };
    }
}