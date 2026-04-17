using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class AddPlayerRequest
{
    [Required(ErrorMessage = "Le pseudo est obligatoire.")]
    [StringLength(50, ErrorMessage = "La longueur maximum du pseudo est de 50 caractères")]
    public string Pseudo { get; set; }
    
    [Required(ErrorMessage = "L'email est obligatoire.")]
    [EmailAddress(ErrorMessage = "Format de l'email incorrect")]
    [StringLength(100, ErrorMessage = "La longueur maximum de l'email est de 100 caractères")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
    [RegularExpression(@"^(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$", 
            ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères, une lettre majuscule, une lettre minuscule et un caractère spécial.")]
    public string Pwd { get; set; }
    
    [Required(ErrorMessage = "La date de naissance est obligatoire.")]
    public DateOnly BirthDate { get; set; }
    
    public string Gender { get; set; }
    public int Elo { get; set; } = 1200; 
}