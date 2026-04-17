using API.DTO;
using Domain.Entities;

namespace API.Mapping;

public static class PlayerMapping
{
    public static GetPlayerResponse ToResponse(Player player)
    {
        return new GetPlayerResponse
        {
            PlayerId = player.PlayerId,
            Pseudo = player.Pseudo,
            Email = player.Email,
            BirthDate = player.BirthDate,
            Gender = player.Gender,
            Elo = player.Elo

        };
    }

    public static Player ToResponse(AddPlayerRequest request)
    {
        return new Player
        {
            Pseudo = request.Pseudo,
            Email = request.Email,
            Pwd = request.Pwd,
            BirthDate = request.BirthDate,
            Gender = request.Gender,
            Elo = request.Elo
        };
    }
}