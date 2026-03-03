using LibrarySystem.DAL.Data;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
