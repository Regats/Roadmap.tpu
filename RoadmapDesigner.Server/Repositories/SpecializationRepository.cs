using Microsoft.EntityFrameworkCore;
using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;

namespace RoadmapDesigner.Server.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly RoadmapContext _context;
        private readonly ILogger<SpecializationRepository> _logger;

        public SpecializationRepository(RoadmapContext context, ILogger<SpecializationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<List<SpecializationDTO>> GetListSpecializationByDirTrainingID(Guid dirTrainingID)
        {
            if (dirTrainingID == Guid.Empty)
            {
                _logger.LogWarning("Идентификатор направления подготовки пустой");
            }

            try
            {
                var listSpec = await _context.Specializations
                    .Where(s => s.VersionsDirectionTrainningUuid == dirTrainingID)
                    .ToListAsync();

                var listSpecializationDTO = listSpec.Select(s => new SpecializationDTO
                {
                    Uuid = s.Uuid,
                    Name = s.Name,
                    RoadmapJson = s.RoadmapJson,

                    // Добавьте другие свойства по мере необходимости
                }).ToList();

                return listSpecializationDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении специализаций");
                return null;
            }
        }
        public async Task<SpecializationDTO> GetSpecializationByGuid(Guid specID)
        {
            try
            {
                var specialization = await _context.Specializations.FindAsync(specID);


                return new SpecializationDTO
                {
                    Uuid = specialization.Uuid,
                    Name = specialization.Name,
                    RoadmapJson = specialization.RoadmapJson,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении специализации");
                return null;
            }
        }

    }
}
