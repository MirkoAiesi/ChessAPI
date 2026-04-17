using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }
    public async Task<List<Player>> GetAllPlayer()
    {
        return await _playerRepository.GetAllPlayer();
    }

    public async Task<Player> GetById(int id)
    {
        return await _playerRepository.GetById(id);
    }

    public void AddPlayer(Player p)
    {
        if (string.IsNullOrEmpty(p.Pwd))
        {
            throw new ArgumentException("Le mot de passe ne peut pas être vide ou null.", nameof(p.Pwd));
        }
        string hashedPwd = BCrypt.Net.BCrypt.HashPassword(p.Pwd);
        p.Pwd = hashedPwd;
        _playerRepository.AddPlayer(p);
    }
}