using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;

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

        public static List<TimeTableEntry> FindAll()
        {
            return db.TimeTableEntry.ToList();
        }

        public static void Add(TimeTableEntry timeTableLine)
        {
            db.TimeTableEntry.Add(timeTableLine);
            db.SaveChanges();
        }
        public static void Save() => db.SaveChanges();

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
    }
}