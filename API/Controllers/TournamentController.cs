using API.DTO;
using API.DTO.TournamentDTO;
using API.Mapping;
using BLL.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TournamentController : ControllerBase
{
    private readonly ITournamentService _tournamentService;

    public TournamentController(ITournamentService tournamentService)
    {
        _tournamentService = tournamentService;
    }
    [HttpGet]
    public async Task<ActionResult<List<GetTournamentResponse>>> GetAllTournament()
    {
        var tournaments = await _tournamentService.GetAllTournament();
        var response = tournaments.Select(TournamentMapping.ToResponse);
        return Ok(response);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<GetTournamentResponse>> GetTournamentById(int id)
    {
        Tournament tournament = await _tournamentService.GetTournamentById(id);
        if (tournament is null)
        {
            return NotFound();
        }

        return Ok(TournamentMapping.ToResponse(tournament));
    }

    [HttpPost]
    public ActionResult AddTournament(AddTournamentRequest request)
    {
        _tournamentService.AddTournament(TournamentMapping.ToResponse(request));
        return StatusCode(201, "Tournois créé avec succès !");
    }
    [HttpDelete("{id}")]
    public ActionResult RemoveTournament(int id)
    {
        _tournamentService.RemoveTournament(id);
        return NoContent();
    }
    [HttpGet("last-tournament")]
    public async Task<ActionResult<List<GetTournamentResponse>>> GetLastTournament()
    {
        var tournaments = await _tournamentService.GetLastTournament();
        var response = tournaments.Select(TournamentMapping.ToResponse);
        return Ok(response);
    }
    [HttpPost("register-players")]
    public async Task<ActionResult> AddPlayerToTournament(int playerId, int tournamentId)
    {
        await _tournamentService.AddPlayerToTournament(playerId,tournamentId );
        return StatusCode(201, "Tournois créé avec succès !");
    }
    [HttpGet("players-tournaments")]
    public async Task<ActionResult<List<PlayerTournament>>> RegisterPlayer(int playerId, int tournamentId)
    {
        var playersTournaments = await _tournamentService.GetPlayersByTournament(tournamentId);
        return Ok(playersTournaments);
    }
    [HttpPost("register-categories")]
    public async Task<ActionResult> AddCategorieToTournament(int categoryId, int tournamentId)
    {
        await _tournamentService.AddCategorieToTournament(categoryId,tournamentId );
        return StatusCode(201, "Catégorie associé au bon tournois !");
    }

    [HttpDelete("unsubscribe-player")]
    public async Task<ActionResult> RemovePlayerToTournament(int playerId, int tournamentId)
    {
        await _tournamentService.RemovePlayerToTournament(playerId, tournamentId);
        return NoContent();
    }

    [HttpPut("{tournamentId}/start")]
    public async Task<ActionResult> StartTournament(int tournamentId)
    {
        await _tournamentService.StartTournament(tournamentId);
        return Ok("Tournoi démarré");
    }
    [HttpPut("{matchId}/result")]
    public async Task<ActionResult> ResultByMatch(int matchId)
    {
        await _tournamentService.ResultByMatch(matchId);
        return Ok("Résultat du match modifié");
    }

    [HttpPut("passage-round-supérieur")]
    public async Task<ActionResult> UpdateRoundMatch(int tournamentId)
    {
        await _tournamentService.UpdateRoundMatch(tournamentId);
        return Ok("Le tournoi passe à la ronde suivante");
    }
    
}