using API.DTO.CategorieDTO;
using Domain.Entities;

namespace API.Mapping;

public static class CategorieMapping
{
    public static GetCategorieResponse ToResponse(Categorie categorie)
    {
        return new GetCategorieResponse
        {
            Name = categorie.Name

        };
    } 
}