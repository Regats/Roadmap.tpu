using RoadmapDesigner.Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadmapDesigner.Server.Interfaces
{
    public interface ISpecializationRepository
    {
        // Метод для асинхронного получения списка специализаций по идентификатору направления подготовки
        Task<List<SpecializationDTO>> GetListSpecializationsByDirTrainingUuid(Guid dirTrainingUuid);

        // Метод для асинхронного получения специализации по UUID
        Task<SpecializationDTO> GetSpecializationByUuid(Guid specUuid);
    }
}