using HttpServer.Models;
using OstLib;
using Microsoft.AspNetCore.Mvc;
using OstLib.Repository;
using TimeTableWeekModel = OstLib.TimeTableWeekModelJSON;

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

        [HttpPost]
        public IEnumerable<TimeTableEntryModel> GetTimeTableForThisWeek(TimeTableWeekModel timeTableWeekModel)
        {
            var listOfItems = _timeTable
            .FindAllForThisWeek(timeTableWeekModel.I)
            .Select(s => new TimeTableEntryModel(
                s.DateTime, 
                s.Client.Surname, 
                s.Client.Name, 
                s.Client.PhoneNumber));
            return listOfItems;
        }

        [HttpPost]
        public int AddTimeTableLine(TimeTableLineModel tableLineModel)
        {
            
        }
    }
}
