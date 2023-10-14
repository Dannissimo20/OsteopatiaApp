using System.Collections.Generic;

namespace OstLib.Repository
{
    
    public interface ITimeTable
    {
        IEnumerable<TimeTableEntry> FindAll();
        IEnumerable<TimeTableEntry> FindAllForThisWeek(int k);
        void Add(TimeTableEntry timeTableEntry);
    }
}
