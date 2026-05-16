using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;
[ApiController]
[Route("api/match/{matchId}/lineup")] 
public class MatchLineupController : ControllerBase
{
    private readonly IMatchLineupService _lineupService;
    private readonly IMapper _mapper;

    public MatchLineupController(IMatchLineupService lineupService, IMapper mapper)
    {
        _lineupService = lineupService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<MatchLineupResponseDTO>> AddPlayer(int matchId, MatchLineupRequestDTO dto)
    {
        try
        {
            var lineup = _mapper.Map<MatchLineup>(dto);
            lineup.MatchId = matchId; 

            var created = await _lineupService.AddPlayerToLineupAsync(lineup);

            var fullLineup = await _lineupService.GetLineupByMatchAsync(matchId);
            var createdPlayer = fullLineup.First(x => x.Id == created.Id);

            return CreatedAtAction(nameof(GetMatchLineup), new { matchId }, _mapper.Map<MatchLineupResponseDTO>(createdPlayer));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message }); 
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>> GetMatchLineup(int matchId)
    {
        var lineup = await _lineupService.GetLineupByMatchAsync(matchId);
        return Ok(_mapper.Map<IEnumerable<MatchLineupResponseDTO>>(lineup));
    }

    [HttpGet("team/{teamId}")]
    public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>> GetMatchLineupByTeam(int matchId, int teamId)
    {
        try
        {
            var lineup = await _lineupService.GetByMatchAndTeamAsync(matchId, teamId);
            return Ok(_mapper.Map<IEnumerable<MatchLineupResponseDTO>>(lineup));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemovePlayer(int id)
    {
        try
        {
            await _lineupService.RemovePlayerFromLineupAsync(id);
            return NoContent(); 
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message }); 
        }
    }
}
