namespace SportsLeague.API.DTOs.Response
{
    public class SponsorResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Lo enviamos como string para que sea legible
        public string ContactEmail { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

}
