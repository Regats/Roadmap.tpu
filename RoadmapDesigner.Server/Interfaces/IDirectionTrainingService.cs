using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;

namespace RoadmapDesigner.Server.Interfaces
{
    public interface IDirectionTrainingService
    {
        Task<List<TrainingArea>> SortAllDirectionTrainingToTrainingAreas();

        Task<VersionsDirectionTrainingDTO> GetDirectionTrainingDetails(Guid uuid);
    }
}
