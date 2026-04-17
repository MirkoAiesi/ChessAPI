using Domain.Entities;

namespace BLL.Interfaces;

public interface IPlayerService
{
    Task<List<Player>> GetAllPlayer();
    Task<Player> GetById(int id);
    void AddPlayer(Player p);
}