using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TennisBookings.Merchandise.Api.External.Database;
using TennisBookings.Merchandise.Api.Models.Output;

namespace TennisBookings.Merchandise.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly ICloudDatabase _cloudDatabase;

        public StockController(ICloudDatabase cloudDatabase)
        {
            _cloudDatabase = cloudDatabase;
        }

        [HttpGet]
        [Route("total")]
        public async Task<IActionResult> Get()
        {
            var data = await _cloudDatabase.ScanAsync();
            return Ok(new StockTotalOutputModel { StockItemTotal = data.Sum(x => x.StockCount) });
        }
    }
}
