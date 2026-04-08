using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories.SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _repository;

        // Inyectamos el repositorio específico que ya tiene los métodos Exists
        public SponsorService(ISponsorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Sponsor?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Sponsor> CreateAsync(Sponsor sponsor)
        {
            // 1. Validación de Negocio: Nombre Único
            if (await _repository.ExistsByNameAsync(sponsor.Name))
                throw new Exception($"Regla de Negocio: Ya existe un patrocinador con el nombre '{sponsor.Name}'.");

            // 2. Validación de Negocio: Email Único
            if (await _repository.ExistsByEmailAsync(sponsor.ContactEmail))
                throw new Exception($"Regla de Negocio: El correo '{sponsor.ContactEmail}' ya está registrado por otra empresa.");

            // 3. Validación de Negocio: Teléfono Único
            if (await _repository.ExistsByPhoneAsync(sponsor.Phone!))
                throw new Exception($"Regla de Negocio: El teléfono '{sponsor.Phone}' ya pertenece a otro registro.");

            // Si pasa todas las validaciones, procedemos a guardar
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
                throw new Exception("El patrocinador que intenta eliminar no existe.");

            await _repository.DeleteAsync(id);
        }
    }
}
