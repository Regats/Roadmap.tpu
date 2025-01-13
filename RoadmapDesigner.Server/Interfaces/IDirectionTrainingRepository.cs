using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;

namespace RoadmapDesigner.Server.Interfaces
{
    public interface IDirectionTrainingRepository
    {
        Task<IEnumerable<VersionsDirectionTrainingDTO>> GetAllVersionsDirectionTrainingAsync();
        Task<VersionsDirectionTrainingDTO> GetVersionDirectionTrainingDetailsAsync(Guid uuid);
    }
}
