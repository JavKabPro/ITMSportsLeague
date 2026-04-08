using SportsLeague.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SportsLeague.API.DTOs.Request
{
    public class SponsorRequestDTO
    {   
        [Required] public string Name { get; set; } = null!;
        [Required] public string NIT { get; set; } = null!;
        [Required] public SponsorCategory Category { get; set; }
        [EmailAddress] public string ContactEmail { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
