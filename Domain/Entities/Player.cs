using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Player
{
    public int PlayerId { get; set; }
    
    [Required(ErrorMessage = "Le pseudo est obligatoire.")]
    [StringLength(50, ErrorMessage = "La longueur maximum du pseudo est de 50 caractères")]
    public string Pseudo { get; set; }
    
    [Required(ErrorMessage = "L'email est obligatoire.")]
    [EmailAddress(ErrorMessage = "Format de l'email incorrect")]
    [StringLength(100, ErrorMessage = "La longueur maximum de l'email est de 100 caractères")]
    public string Email { get; set; }
    
    [Required]
    public string Pwd { get; set; } // A VOIR COMMENT FAIRE PR LE HASH
    
    [Required(ErrorMessage = "La date de naissance est obligatoire.")]
    public DateOnly BirthDate { get; set; }
    
    public string Gender { get; set; }
    public int Elo { get; set; } = 1200; // est ce que je dois l'initaliser ici ? je pense que non
}