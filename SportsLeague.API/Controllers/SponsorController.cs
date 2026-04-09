using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SponsorController : ControllerBase
{
    private readonly ISponsorService _sponsorService;
    private readonly IMapper _mapper;

    public SponsorController(ISponsorService sponsorService, IMapper mapper)
    {
        _sponsorService = sponsorService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SponsorResponseDTO>>> GetAll()
    {
        var sponsors = await _sponsorService.GetAllAsync();
        var response = _mapper.Map<IEnumerable<SponsorResponseDTO>>(sponsors);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SponsorResponseDTO>> GetById(int id)
    {
        var sponsor = await _sponsorService.GetByIdAsync(id);
        if (sponsor == null) return NotFound($"No se encontró el patrocinador con ID {id}");

        return Ok(_mapper.Map<SponsorResponseDTO>(sponsor));
    }

    [HttpPost]
    public async Task<ActionResult<SponsorResponseDTO>> Create(SponsorRequestDTO request)
    {
        try
        {
            var sponsor = _mapper.Map<Sponsor>(request);
            var createdSponsor = await _sponsorService.CreateAsync(sponsor);
            var response = _mapper.Map<SponsorResponseDTO>(createdSponsor);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    [HttpPost("{id}/tournaments")]
    public async Task<ActionResult> AssociateTournament(int id, [FromBody] TournamentSponsorRequestDTO request)
    {
        try
        {
            // El 'id' de la URL es el SponsorId
            await _sponsorService.AssociateTournamentAsync(id, request.TournamentId, request.ContractAmount);
            return Ok(new { message = "Patrocinador vinculado exitosamente al torneo." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // Aquí es donde mostrarás el error 400/409 en el video
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno: " + ex.Message });
        }
    }
}