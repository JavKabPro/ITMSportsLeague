using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsLeague.Domain.Entities;
public class MatchLineup : AuditBase
{
    public int MatchId { get; set; }
    public int PlayerId { get; set; }
    public bool IsStarter { get; set; } // true = Titular, false = Suplente [2, 7]
    public string Position { get; set; } = string.Empty; // Ej: "GK", "CB" [2, 8]

    // Propiedades de navegación
    public Match Match { get; set; } = null!;
    public Player Player { get; set; } = null!;
}
