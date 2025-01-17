using RoadmapDesigner.Server.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadmapDesigner.Server.Interfaces
{
    public interface IDisciplineService
    {
        // Метод для асинхронного получения списка дисциплин
        Task<List<DisciplineDTO>> GetListDisciplinesAsync();
    }
}