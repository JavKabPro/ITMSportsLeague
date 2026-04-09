using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentSponsorRepository : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
    {
        public TournamentSponsorRepository(LeagueDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsAsync(int tournamentId, int sponsorId)
        {
            return await _context.TournamentSponsors
                .AnyAsync(ts => ts.TournamentId == tournamentId && ts.SponsorId == sponsorId);
        }
    }
}
