using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Response
{
    public class UpdateStatusDTO
    {
        public TournamentStatus Status { get; set; }
    }

}
