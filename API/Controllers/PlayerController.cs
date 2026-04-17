using API.DTO;
using API.Mapping;
using BLL.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }
    [HttpGet]
    public async Task<ActionResult<List<GetPlayerResponse>>> GetAllPlayer()
    {
        var players = await _playerService.GetAllPlayer();
        var response = players.Select(PlayerMapping.ToResponse);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetPlayerResponse>> GetById(int id)
    {
        Player player = await _playerService.GetById(id);
        if (player is null)
        {
            return NotFound();
        }

        return Ok(PlayerMapping.ToResponse(player));
    }

    [HttpPost]
    public ActionResult AddPlayer(AddPlayerRequest request)
    {
        _playerService.AddPlayer(PlayerMapping.ToResponse(request));
        return StatusCode(201, "Joueur créé avec succès !");
    }
}