using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;

namespace RoadmapDesigner.Server.Interfaces
{
    public interface IDisciplineService
    {
        Task<List<DisciplineDTO>> GetListDisciplineAsync();
    }
}
