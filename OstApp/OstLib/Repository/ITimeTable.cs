using System.Collections.Generic;

namespace OstLib.Repository
{
    
    public interface ITimeTable
    {
        IEnumerable<TimeTableEntry> FindAll();
    }
}
