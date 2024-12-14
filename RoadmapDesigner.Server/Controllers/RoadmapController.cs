using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadmapDesigner.Server.Models.Entity;

namespace RoadmapDesigner.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoadmapController : ControllerBase
    {

        private readonly RoadmapDesignerContext _context;

        public RoadmapController(RoadmapDesignerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetListUsers()
        {
            var users = _context.Users.ToListAsync();
            return await users;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            // ���������� ����������� ���� index.html, React ����� ������������ ��� ��� �������� �����
            return File("~/index.html", "text/html");
        }

    }
}
