using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services;

public interface IMatchLineupService
{
    Task<IEnumerable<MatchLineup>> GetLineupByMatchAsync(int matchId);
    Task<IEnumerable<MatchLineup>> GetLineupByTeamAsync(int matchId, int teamId);
    Task<MatchLineup> AddPlayerToLineupAsync(MatchLineup lineup);
    Task RemovePlayerFromLineupAsync(int id);
    Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId);
}
