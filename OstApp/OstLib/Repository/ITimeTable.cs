using System;
using System.Collections.Generic;

namespace OstLib.Repository
{
    
    public interface ITimeTable
    {
        IEnumerable<TimeTableEntry> FindAll();
        IEnumerable<TimeTableEntry> FindAllForThisWeek(int k);
        DateTime GetLastDateForClient(Client client);
        void Add(TimeTableEntry timeTableEntry);
        void Remove(TimeTableEntry timeTableEntry);
        TimeTableEntry GetTimeTableLineByDate(DateTime date);
        IEnumerable<TimeTableEntry> FindAllBySurname(string surname);
        
    }
}
