namespace RoadmapDesigner.Server.Models.DTO
{
    public class SpecializationDTO
    {
        public Guid Uuid { get; set; }

        public string Name { get; set; } = null!;

        public string? RoadmapJson { get; set; }
    }
}
