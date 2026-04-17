using Domain.Entities;

namespace DAL.Interfaces;

public interface IPlayerRepository
{
    Task<List<Player>> GetAllPlayer();
    Task<Player> GetById(int id);
    void AddPlayer(Player p);
    void RemovePlayer(int id);
}