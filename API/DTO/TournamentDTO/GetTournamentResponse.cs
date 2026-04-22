using Domain.Entities;

namespace API.DTO.TournamentDTO;

public class GetTournamentResponse
{
    public int TournamentId { get; set; }
    public string Name { get; set; } = "";
    public string Location { get; set; } = "";
    public int MinPlayer { get; set; }
    public int MaxPlayer { get; set; }
    public int? MinElo {get; set; }
    public int? MaxElo { get; set; }
    public string Status { get; set; } = "En attente de joueurs";
    public int Round { get; set; } = 0;
    public bool WomenOnly { get; set; } = false;
    public DateTime EndRegistration { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdateDate { get; set; } = DateTime.UtcNow;
    public List<Categorie> Categories { get; set; } = new List<Categorie>();
    public List<Player> Players { get; set; } = new List<Player>();
}