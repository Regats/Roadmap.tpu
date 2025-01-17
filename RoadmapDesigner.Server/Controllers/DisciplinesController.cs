using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;

namespace RoadmapDesigner.Server.Controllers
{
    [ApiController]
    [Route("api/discipline")]
    public class DisciplinesController : ControllerBase
    {
        private readonly ILogger<DirectionTrainingController> _logger;
        private readonly IDisciplineService _disciplineService;

        public DisciplinesController(ILogger<DirectionTrainingController> logger, IDisciplineService disciplineService)
        {
            _logger = logger;
            _disciplineService = disciplineService;
        }

        [HttpGet("listDiscip")]
        public async Task<ActionResult<List<DisciplineDTO>>> GetListDisciplineAsyns()
        {
            var listDiscip = await _disciplineService.GetListDisciplineAsync();
            return Ok(listDiscip);
        }
    }
}
