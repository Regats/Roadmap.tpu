using RoadmapDesigner.Server.Models.Entities;

namespace RoadmapDesigner.Server.Models.DTO
{
    public class TrainingArea
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<VersionsDirectionTrainingDTO> TrainingDirections { get; set; } = new List<VersionsDirectionTrainingDTO>();
    }
}
