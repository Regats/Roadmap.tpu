using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;

namespace RoadmapDesigner.Server.Controllers
{
    [ApiController]
    [Route("api/DT")]
    public class DirectionTrainingController : ControllerBase
    {
        private readonly IDirectionTrainingService _directionTrainingService;
        private readonly ILogger<DirectionTrainingController> _logger;

        public DirectionTrainingController(IDirectionTrainingService directionTrainingService, ILogger<DirectionTrainingController> logger)
        {
            _logger = logger;
            _directionTrainingService = directionTrainingService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TrainingArea>>> GetAllDirectionTraining()
        {
            var directionTraining = await _directionTrainingService.SortAllDirectionTrainingToTrainingAreas();

            return Ok(directionTraining);
        }

        [HttpGet("DirectionTrainingDetails/{uuid}")]
        public async Task<ActionResult<VersionsDirectionTrainingDTO>> GetDirectionTrainingDetails(Guid uuid)
        {
            var detailsDirectionTraining = await _directionTrainingService.GetDirectionTrainingDetails(uuid);
            return Ok(detailsDirectionTraining);
        }
    }
}
