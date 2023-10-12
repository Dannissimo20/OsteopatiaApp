using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OstLib.Repository;

namespace OstLib.Services
{
    public class TimeTableService : ITimeTable
    {
        private readonly ApplicationContext _context;
        public TimeTableService(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<TimeTableEntry> FindAll()
        {
            /*
             * AsNoTracking() - позволяет не отслеживать изменения в бд
             * Ускоряет работу EntityFramework
             *
             * Include() - подгружает вложенный объект указанный в лямюде
             * Делает join при обращении к бд
             */
            return _context.TimeTableEntry.AsNoTracking().Include(t => t.Client);
        }
    }
}
