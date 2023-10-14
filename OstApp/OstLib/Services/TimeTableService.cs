using System;
using System.Collections.Generic;
using System.Linq;
using Itenso.TimePeriod;
using System.Runtime.CompilerServices;
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

        public IEnumerable<TimeTableEntry> FindAllForThisWeek(int k)
        {
            Week week = new Week(DateTime.Now + TimeSpan.FromDays(k*7));
            var lastDayOfWeek = week.LastDayOfWeek;
            lastDayOfWeek = lastDayOfWeek.AddHours(23);
            return _context
            .TimeTableEntry
            .Where(t => t.DateTime <= lastDayOfWeek && t.DateTime >= week.FirstDayOfWeek)
            .Include(t=>t.Client);
        }

        public void Add(TimeTableEntry timeTableEntry) => _context.TimeTableEntry.Add(timeTableEntry);
    }
}
