using RoadmapDesigner.Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadmapDesigner.Server.Interfaces
{
    public interface IDirectionTrainingRepository
    {
        // Метод для асинхронного получения всех версий направления обучения
        Task<IEnumerable<VersionsDirectionTrainingDTO>> GetAllAsync();

        // Метод для асинхронного получения деталей версии направления обучения по UUID
        Task<VersionsDirectionTrainingDTO> GetDetailsAsync(Guid uuid);
    }
}