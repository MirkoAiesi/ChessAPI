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
}