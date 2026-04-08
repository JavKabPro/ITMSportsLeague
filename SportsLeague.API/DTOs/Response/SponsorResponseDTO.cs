namespace SportsLeague.API.DTOs.Response
{
    public class SponsorResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string NIT { get; set; } = null!;
        public string Category { get; set; } = null!; // Lo enviamos como string para que sea legible
        public string ContactEmail { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }

}
