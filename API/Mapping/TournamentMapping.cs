using API.DTO.TournamentDTO;
using Domain.Entities;

namespace API.Mapping;

public static class TournamentMapping
{
    public static GetTournamentResponse ToResponse(Tournament tournament)
    {
        return new GetTournamentResponse
        {
            TournamentId = tournament.TournamentId,
            Name = tournament.Name,
            Location = tournament.Location,
            MinPlayer = tournament.MinPlayer,
            MaxPlayer = tournament.MaxPlayer,
            MinElo = tournament.MinElo,
            MaxElo = tournament.MaxElo,
            Status = tournament.Status,
            Round = tournament.Round,
            WomenOnly = tournament.WomenOnly,
            EndRegistration = tournament.EndRegistration,
            CreateDate = tournament.CreateDate,
            UpdateDate = tournament.UpdateDate,
            Categories = tournament.Categories,
            Players = tournament.Players
            
        };
    }
    public static Tournament ToResponse(AddTournamentRequest tournament)
    {
        return new Tournament
        {
            Name = tournament.Name,
            Location = tournament.Location,
            MinPlayer = tournament.MinPlayer,
            MaxPlayer = tournament.MaxPlayer,
            MinElo = tournament.MinElo,
            MaxElo = tournament.MaxElo,
            Status = tournament.Status,
            Round = tournament.Round,
            WomenOnly = tournament.WomenOnly,
            EndRegistration = tournament.EndRegistration,
            CreateDate = tournament.CreateDate,
            UpdateDate = tournament.UpdateDate,
            Categories = tournament.Categories
        };
    }
}