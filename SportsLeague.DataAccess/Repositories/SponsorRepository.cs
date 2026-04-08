using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories.SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class SponsorRepository : GenericRepository<Sponsor>, ISponsorRepository
    {
        public SponsorRepository(LeagueDbContext context) : base(context)
        {
        }
        //Validaciones de datos para que no se repitan
        public async Task<bool> ExistsByNameAsync(string name)
        {
            // Usamos ToLower para que la validación no falle por una mayúscula dado el caso
            return await _context.Sponsors
                .AnyAsync(s => s.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Sponsors.AnyAsync(s => s.ContactEmail.ToLower() == email.ToLower());
        }

        public async Task<bool> ExistsByPhoneAsync(string phone)
        {
            return await _context.Sponsors.AnyAsync(s => s.Phone == phone);
        }
    }
}
