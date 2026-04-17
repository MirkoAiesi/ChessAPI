namespace API.DTO;

public class GetPlayerResponse
{
    public int PlayerId { get; set; }
    public string Pseudo { get; set; } = "";
    public string Email { get; set; }= "";
    public DateOnly BirthDate { get; set; }
    public string Gender { get; set; }= "";
    public int Elo { get; set; } 
}