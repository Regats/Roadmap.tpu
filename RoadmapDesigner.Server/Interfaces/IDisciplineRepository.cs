using RoadmapDesigner.Server.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadmapDesigner.Server.Interfaces
{
    public interface IDisciplineRepository
    {
        // Метод для асинхронного получения списка дисциплин
        Task<List<DisciplineDTO>> GetListDisciplinesAsync();
    }
}