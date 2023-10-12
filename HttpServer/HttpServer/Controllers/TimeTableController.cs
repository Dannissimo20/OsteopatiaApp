using HttpServer.Models;
using OstLib;
using Microsoft.AspNetCore.Mvc;
using OstLib.Repository;

namespace HttpServer.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class TimeTableController : ControllerBase
    {
        private readonly ITimeTable _timeTable;
        public TimeTableController(ITimeTable timeTable)
        {
            _timeTable = timeTable;
        }
        
        [HttpGet]
        public IEnumerable<TimeTableEntryModel> GetTimeTable()
        {
            var listOfItems = _timeTable
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
