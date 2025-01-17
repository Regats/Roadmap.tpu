using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;

namespace RoadmapDesigner.Server.Interfaces
{
    public interface IDisciplineRepository
    {
        Task<List<DisciplineDTO>> GetListDisciplineAsync();
    }
}
