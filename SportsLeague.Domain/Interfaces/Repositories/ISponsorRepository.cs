using SportsLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    namespace SportsLeague.Domain.Interfaces.Repositories
    {
        public interface ISponsorRepository : IGenericRepository<Sponsor>
        {
            //Metodos especificos para validaciones
            Task<bool> ExistsByNameAsync(string name);
            Task<bool> ExistsByEmailAsync(string email);
            Task<bool> ExistsByPhoneAsync(string phone);

        }
    }
}
