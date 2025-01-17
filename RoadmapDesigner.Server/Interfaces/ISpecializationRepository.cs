using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;

namespace RoadmapDesigner.Server.Interfaces
{
    public interface ISpecializationRepository
    {
        Task<List<SpecializationDTO>> GetListSpecializationByDirTrainingID(Guid dirTrainingID);
        Task<SpecializationDTO> GetSpecializationByGuid(Guid specID);
    }
}
