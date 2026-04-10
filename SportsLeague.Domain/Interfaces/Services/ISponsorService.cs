using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface ISponsorService
    {
        Task<IEnumerable<Sponsor>> GetAllAsync();
        Task<Sponsor?> GetByIdAsync(int id);
        Task<Sponsor> CreateAsync(Sponsor sponsor);
        Task UpdateAsync(Sponsor sponsor);
        Task DeleteAsync(int id);

        Task<TournamentSponsor> AssociateTournamentAsync(TournamentSponsor association);
        Task<TournamentSponsor> AssociateTournamentAsync(int sponsorId, int tournamentId, decimal amount);
        Task<IEnumerable<Tournament>> GetSponsorTournamentsAsync(int sponsorId);
    }
}
