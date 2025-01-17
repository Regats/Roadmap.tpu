using RoadmapDesigner.Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadmapDesigner.Server.Interfaces
{
    public interface IDirectionTrainingService
    {
        // Метод для получения всех областей обучения с направлениями обучения
        Task<List<TrainingArea>> GetAllTrainingAreas();

        // Метод для асинхронного получения деталей версии направления обучения по UUID
        Task<VersionsDirectionTrainingDTO> GetVersionDirectionTrainingDetails(Guid uuid);
    }
}