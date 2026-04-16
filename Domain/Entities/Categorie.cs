using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Categorie
{
    public int CategorieId { get; set; }

    [StringLength(255, ErrorMessage = "La longueur maximum du nom de la catégorie est de 255 caractères")]
    public string Name { get; set; } = "";
}