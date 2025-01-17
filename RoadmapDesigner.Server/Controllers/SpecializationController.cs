using Microsoft.AspNetCore.Mvc;
using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.Entities;

namespace RoadmapDesigner.Server.Controllers
{
    [ApiController]
    [Route("api/spec")]

    public class SpecializationController : ControllerBase
    {
        private readonly ILogger<SpecializationController> _logger;
        private readonly ISpecializationService _specializationService;
        
        public SpecializationController(ILogger<SpecializationController> logger, ISpecializationService specializationService)
        {
            _logger = logger;
            _specializationService = specializationService;
        }


        [HttpGet("listSpec/{dirTrainingID}")]
        public async Task<ActionResult<List<Specialization>>> GetListSpecializationsByDTGuid(Guid dirTrainingID)
        {
            var listSpec = await _specializationService.GetListSpecializationByDirTrainingID(dirTrainingID);
            return Ok(listSpec);
        }

        [HttpGet("spec/{specID}")]
        public async Task<ActionResult<Specialization>> GetSpecializationByGuid(Guid specID)
        {
            var spec = await _specializationService.GetSpecializationByGuid(specID);
            return Ok(spec);
        }

    }
}
