using HttpServer.Models;
using OstLib;
using Microsoft.AspNetCore.Mvc;

namespace HttpServer.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class TimeTableController : ControllerBase
    {
        [HttpGet("get")]
        public IEnumerable<TimeTableEntryModel> GetTimeTable()
        {
            
        }
    }
}
