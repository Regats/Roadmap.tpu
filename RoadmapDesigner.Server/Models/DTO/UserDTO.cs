namespace RoadmapDesigner.Server.Models.DTO
{
    public class UserDTO
    {
        public Guid UserId { get; set; }

        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string Login { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Role { get; set; }
    }
}
