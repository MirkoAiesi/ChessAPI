namespace API.DTO.CategorieDTO;

public class GetCategorieResponse
{
    public int CategorieId { get; set; }
    public string Name { get; set; } = "";
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
}
