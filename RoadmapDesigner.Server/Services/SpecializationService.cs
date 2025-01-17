using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;

namespace RoadmapDesigner.Server.Services
{
    public class SpecializationService : ISpecializationService
    {
        private readonly ISpecializationRepository _specializationRepository;
        private readonly ILogger<SpecializationService> _logger;
        public SpecializationService(ISpecializationRepository specializationRepository, ILogger<SpecializationService> logger) 
        {
            _logger = logger;
            _specializationRepository = specializationRepository;
        }

        public async Task<List<SpecializationDTO>> GetListSpecializationByDirTrainingID(Guid dirTrainingID)
        {
            var listSpecialization = await _specializationRepository.GetListSpecializationByDirTrainingID(dirTrainingID);
            return listSpecialization;
        }

        public async Task<SpecializationDTO> GetSpecializationByGuid(Guid specID)
        {
            var specialization = await _specializationRepository.GetSpecializationByGuid(specID);
            return specialization;
        }
    }
}
