using Microsoft.AspNetCore.Mvc;
using Khabarov_Artem_AS2304.Data;

namespace Khabarov_Artem_AS2304.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabController : ControllerBase
    {
        private readonly LabContext _context;

        public LabController(LabContext context)
        {
            _context = context;
        }

    }
}