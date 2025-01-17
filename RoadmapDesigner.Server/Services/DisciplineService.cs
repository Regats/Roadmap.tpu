using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;

namespace RoadmapDesigner.Server.Services
{
    public class DisciplineService : IDisciplineService
    {
        private readonly IDisciplineRepository _disciplineRepository;
        private readonly ILogger<DisciplineService> _logger;

        public DisciplineService(IDisciplineRepository disciplineRepository, ILogger<DisciplineService> logger)
        {
            _disciplineRepository = disciplineRepository;
            _logger = logger;
        }

        public async Task<List<DisciplineDTO>> GetListDisciplineAsync()
        {
            var listDiscipline = await _disciplineRepository.GetListDisciplineAsync();
            return listDiscipline;
        }
    }
}
