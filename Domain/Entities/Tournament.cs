using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Tournament
{
    public int TournamentId { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire.")]
    [StringLength(100, ErrorMessage = "La longueur maximum du nom est de 100 caractères")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Le lieu est obligatoire.")]
    [StringLength(255, ErrorMessage = "La longueur maximum du lieu est de 255 caractères")]
    public string Location { get; set; } = "";
    
    [Required(ErrorMessage = "Nombre de joueurs minimum obligatoire.")]
    //[Range(2, int.MaxValue, ErrorMessage = "Il faut au moins 2 joueurs.")]
    public int MinPlayer { get; set; }
    
    [Required(ErrorMessage = "Nombre de joueurs maximum obligatoire.")]
    public int MaxPlayer { get; set; }
    
    public int? MinElo {get; set; }
    public int? MaxElo { get; set; }
    
    [StringLength(50, ErrorMessage = "La longueur maximum du statut est de 50 caractères")]
    public string Status { get; set; } = "En attente de joueurs";
    
    public int Round { get; set; } = 0;
    public bool WomenOnly { get; set; } = false;
    
    [Required(ErrorMessage = "Date de fin des inscriptions obligatoire.")]
    public DateTime EndRegistration { get; set; }
    
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdateDate { get; set; } = DateTime.UtcNow;

    public List<Categorie> Categories { get; set; } = new List<Categorie>();
    public List<Player> Players { get; set; } = new List<Player>();
}