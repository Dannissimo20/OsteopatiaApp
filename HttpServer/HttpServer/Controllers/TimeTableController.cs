using HttpServer.Models;
using OstLib;
using Microsoft.AspNetCore.Mvc;
using OstLib.Repository;
using TimeTableWeekModel = OstLib.TimeTableWeekModelJSON;

namespace HttpServer.Controllers
{
    [Route("TimeTable")]
    [ApiController]
    public class TimeTableController : ControllerBase
    {
        private readonly ITimeTable _timeTable;
        private readonly IClient _client;
        public TimeTableController(ITimeTable timeTable, IClient client)
        {
            _timeTable = timeTable;
            _client = client;
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

        [HttpPost("getWeekTable")]
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

        [HttpPost("addTableLine")]
        public int AddTimeTableLine(TimeTableLineModel tableLineModel)
        {
            try
            {
                Client client = _client.GetByTimeTableFields(tableLineModel);
                TimeTableEntry timeTableLine;
                if (client == null)
                {
                    client = new Client(
                        tableLineModel.Surname,
                        tableLineModel.Name,
                        "",
                        tableLineModel.Phone,
                        "0",
                        "-",
                        "",
                        "",
                        "",
                        "");
                    _client.Add(client);
                }
                timeTableLine = new TimeTableEntry(
                    DateTime.Parse(tableLineModel.Date),
                    client);
                _timeTable.Add(timeTableLine);
                return 200;
            }
            catch(Exception e)
            {
                Console.WriteLine("ОБОШИБКА " + e.Message);
                return 400;
            }
        }
        [HttpPost("removeTableLine")]
        public int RemoveTimeTableLine(TimeTableLineModel tableLineModel)
        {
            try
            {
                var tte = _timeTable.GetTimeTableLineByDate(DateTime.Parse(tableLineModel.Date));
                _timeTable.Remove(tte);
                return 200;
            }
            catch(Exception e)
            {
                Console.WriteLine("ОБОШИБКА " + e.Message);
                return 400;
            }
        }

        [HttpPost("getByDate")]
        public TimeTableEntry GetTimeTableLineByDate(TimeTableDateModel tableDateModel)
        {
            var tte =  _timeTable.GetTimeTableLineByDate(DateTime.Parse(tableDateModel.Date));
            tte.Client.TimeTableLines = null;
            return tte;
        }

        [HttpPost("getBySurname")]
        public IEnumerable<TimeTableEntry> GetTimeTablesBySurname(string surname)
        {
            return _timeTable.FindAllBySurname(surname);
        }
    }
}
