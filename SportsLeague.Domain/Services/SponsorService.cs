using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Repositories.SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _repository;
        private readonly ITournamentSponsorRepository _tournamentSponsorRepository;
        private readonly ITournamentRepository _tournamentRepository; // Añadido para validar existencia de torneos
        private readonly ILogger<SponsorService> _logger;

        public SponsorService(
            ISponsorRepository sponsorRepository,
            ITournamentSponsorRepository tournamentSponsorRepository,
            ITournamentRepository tournamentRepository, // Inyectamos el repo de torneos
            ILogger<SponsorService> logger)
        {
            _repository = sponsorRepository;
            _tournamentSponsorRepository = tournamentSponsorRepository;
            _tournamentRepository = tournamentRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todos los patrocinadores");
            return await _repository.GetAllAsync();
        }

        public async Task<Sponsor?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Sponsor> CreateAsync(Sponsor sponsor)
        {
            if (await _repository.ExistsByNameAsync(sponsor.Name))
                throw new Exception($"Ya existe un patrocinador con el nombre '{sponsor.Name}'.");

            if (await _repository.ExistsByEmailAsync(sponsor.ContactEmail))
                throw new Exception($"El correo '{sponsor.ContactEmail}' ya está registrado.");

            if (await _repository.ExistsByPhoneAsync(sponsor.Phone!))
                throw new Exception($"El teléfono '{sponsor.Phone}' ya pertenece a otro registro.");

            return await _repository.CreateAsync(sponsor);
        }

        public async Task UpdateAsync(Sponsor sponsor)
        {
            await _repository.UpdateAsync(sponsor);
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await _repository.GetByIdAsync(id);
            if (exists == null)
                throw new Exception("El patrocinador no existe.");

            await _repository.DeleteAsync(id);
        }

        // Método de asociación recibiendo el objeto completo
        public async Task<TournamentSponsor> AssociateTournamentAsync(TournamentSponsor association)
        {
            // 1. Validar si el torneo existe
            var tournament = await _tournamentRepository.GetByIdAsync(association.TournamentId);
            if (tournament == null)
                throw new KeyNotFoundException("El torneo no existe.");

            // 2. Validar si el patrocinador existe
            var sponsor = await _repository.GetByIdAsync(association.SponsorId);
            if (sponsor == null)
                throw new KeyNotFoundException("El patrocinador no existe.");

            // 3. Validación de regla de negocio: Torneo finalizado
            if (tournament.Status == TournamentStatus.Finished)
                throw new InvalidOperationException("No se pueden agregar patrocinadores a un torneo finalizado.");

            // 4. Validar duplicados
            var exists = await _tournamentSponsorRepository.ExistsAsync(association.TournamentId, association.SponsorId);
            if (exists)
                throw new InvalidOperationException("Este patrocinador ya está vinculado a este torneo.");

            return await _tournamentSponsorRepository.CreateAsync(association);
        }

        // Método de asociación por parámetros (Sobrecarga útil para el controlador)
        public async Task<TournamentSponsor> AssociateTournamentAsync(int sponsorId, int tournamentId, decimal amount)
        {
            var association = new TournamentSponsor
            {
                SponsorId = sponsorId,
                TournamentId = tournamentId,
                ContractAmount = amount,
                JoinedAt = DateTime.Now
            };

            return await AssociateTournamentAsync(association);
        }
        public async Task<IEnumerable<Tournament>> GetSponsorTournamentsAsync(int sponsorId)
        {
            //  Buscamos el sponsor incluyendo sus relaciones
            var sponsor = await _repository.GetByIdAsync(sponsorId);

            if (sponsor == null)
                throw new KeyNotFoundException($"No se encontró el patrocinador con ID {sponsorId}");

            var tournaments = sponsor.TournamentSponsors.Select(ts => ts.Tournament);

            return tournaments;
        }
    }
}