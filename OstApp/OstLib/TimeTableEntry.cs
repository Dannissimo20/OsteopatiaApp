using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Itenso.TimePeriod;

namespace OstLib
{
    public class TimeTableEntry
    {
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public virtual Client Client { get; set; }
        private static ApplicationContext db = Context.db;
        
        public TimeTableEntry(){}

        public TimeTableEntry(DateTime DateTime, Client Client)
        {
            this.DateTime = DateTime;
            this.Client = Client;
        }

        public static bool isTimeTableEntryExists(DateTime dateTime)
        {
            var timeTableEntry = db.TimeTableEntry.FirstOrDefault(tte => tte.DateTime.Equals(dateTime));
            if (timeTableEntry != null)
                return true;
            else
                return false;
        }

        public static DateTime GetLastDateForClient(Client client)
        {
            var date = db.TimeTableEntry.Where(tte => tte.Client == client).OrderBy(tt=>tt.ID).LastOrDefault();
            if(date == null)
                return DateTime.MinValue;
            return date.DateTime.AddHours(-date.DateTime.Hour);
        }
        
        public static IEnumerable<TimeTableEntry> FindAllForThisWeek(int k)
        {
            Week week = new Week(DateTime.Now + TimeSpan.FromDays(k*7));
            var lastDayOfWeek = week.LastDayOfWeek;
            lastDayOfWeek = lastDayOfWeek.AddHours(23);
            return db
                .TimeTableEntry
                .Where(t => t.DateTime <= lastDayOfWeek && t.DateTime >= week.FirstDayOfWeek);
        }

        public static List<TimeTableEntry> FindAll()
        {
            return db.TimeTableEntry.ToList();
        }
        
        public static IEnumerable<TimeTableEntry> FindAllie()
        {
            return db.TimeTableEntry.ToList();
        }

        public static void Add(TimeTableEntry timeTableLine)
        {
            db.TimeTableEntry.Add(timeTableLine);
            db.SaveChanges();
        }
        public static void Save() => db.SaveChanges();
        public static void Remove(TimeTableEntry tt)
        {
            db.TimeTableEntry.Remove(tt);
            db.SaveChanges();
        }

        public static List<TimeTableEntry> GetTimeTableLineByDate(List<DateTime> date)
        {
            List<TimeTableEntry> list = new List<TimeTableEntry>();
            foreach (var item in date)
            {
                list.AddRange(db.TimeTableEntry.Where(tt => tt.DateTime.Year == item.Year && 
                                                            tt.DateTime.Month == item.Month && 
                                                            tt.DateTime.Day == item.Day && 
                                                            tt.DateTime.Hour == item.Hour));
            }
            if(list.IsNullOrEmpty())
                list.Add(new TimeTableEntry(DateTime.Today, new Client()));
            return list;
        }

        public static TimeTableEntry GetTimeTableLineByDate(DateTime date)
        {
            TimeTableEntry? tte = db.TimeTableEntry.FirstOrDefault(tt => tt.DateTime.Year == date.Year &&
                                                          tt.DateTime.Month == date.Month &&
                                                          tt.DateTime.Day == date.Day &&
                                                          tt.DateTime.Hour == date.Hour);
            if (tte == null)
                tte = new TimeTableEntry(DateTime.Today, new Client());
            return tte;
        }

        public static List<TimeTableEntry> GetTimeTablesBySurname(string surname)
        {
            return db.TimeTableEntry.Where(tt => tt.Client.Surname == surname).OrderBy(t=>t.DateTime).ToList();
        }

        public string GetDate
        {
            get
            {
                return DateTime.ToString("f");
            }
        }
        
        public string GetTime
        {
            get
            {
                return DateTime.ToString("t");
            }
        }

        public string GetInfoForListView
        {
            get
            {
                return $"-  {DateTime.ToString("d")} в {GetTime}";
            }
        }
    }
}