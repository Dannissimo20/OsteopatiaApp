using HttpServer.Models;
using OstLib;
using Microsoft.AspNetCore.Mvc;

namespace HttpServer.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class TimeTableController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<TimeTableEntryModel> GetTimeTable()
        {
            var listOfItems = TimeTableEntry
            .FindAll()
            .Select(s => new TimeTableEntryModel(
                    s.DateTime, 
                    s.Client.Surname, 
                    s.Client.Name, 
                    s.Client.PhoneNumber));
            return listOfItems;
        }
    }
}
