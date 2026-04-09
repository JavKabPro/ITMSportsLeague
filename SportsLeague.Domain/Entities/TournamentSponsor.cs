using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsLeague.Domain.Entities
{
    public class TournamentSponsor : AuditBase
    {
        public int TournamentId { get; set; }
        public int SponsorId { get; set; }

        // Campo específico requerido por el parcial
        public decimal ContractAmount { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.Now;

        // Propiedades de Navegación
        public virtual Tournament Tournament { get; set; } = null!;
        public virtual Sponsor Sponsor { get; set; } = null!;
    }
}
