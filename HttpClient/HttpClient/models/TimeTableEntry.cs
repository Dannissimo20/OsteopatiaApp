using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;

namespace HttpClient
{
    public class TimeTableEntry
    {
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public virtual Client Client { get; set; }

        public TimeTableEntry(){}

        public TimeTableEntry(DateTime DateTime, Client Client)
        {
            this.DateTime = DateTime;
            this.Client = Client;
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