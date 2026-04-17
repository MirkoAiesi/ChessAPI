using API.DTO;
using API.DTO.CategorieDTO;
using API.Mapping;
using BLL.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategorieController : ControllerBase
{
    private readonly ICategorieService _categorieService;

    public CategorieController(ICategorieService categorieService)
    {
        _categorieService = categorieService;
    }
    [HttpGet]
    public async Task<ActionResult<List<GetCategorieResponse>>> GetAllCategorie()
    {
        var categories = await _categorieService.GetAllCategorie();
        var response = categories.Select(CategorieMapping.ToResponse);
        return Ok(response);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<GetCategorieResponse>> GetById(int id)
    {
        Categorie categorie = await _categorieService.GetCategorieById(id);
        if (categorie is null)
        {
            return NotFound();
        }

        return Ok(CategorieMapping.ToResponse(categorie));
    }
}