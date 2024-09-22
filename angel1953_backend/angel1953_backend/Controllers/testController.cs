using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using angel1953_backend.Models;

namespace angel1953_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class testController : ControllerBase
    {
        private readonly Db _context;

        public testController(Db context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
                string searchTerm = "中";
                var entities = await _context.School
                .Where(e => e.SchoolName.Contains(searchTerm))  // LINQ 查詢條件
                .ToListAsync();

            return Ok(entities);
        }
    }
}
