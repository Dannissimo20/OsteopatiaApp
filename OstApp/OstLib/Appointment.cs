using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstLib
{
    public class Appointment
    {
        public int ID { get; set; }
        public string Complaint { get; set; }
        public string Heal { get; set; }
        public DateTime StartTime { get; set; }
        public virtual Client Client { get; set; }
        public int Number { get; set; }
        private static ApplicationContext db = Context.db;
        public Appointment() { }
        public Appointment(string Complaint, string Heal, DateTime StartTime, Client Client, int Number)
        {
            this.Complaint = Complaint;
            this.Heal = Heal;
            this.StartTime = StartTime;
            this.Client = Client;
            this.Number = Number;
        }

        public static List<Appointment> GetAllForThisClient(Client client) => db.Appointment.Where(id => id.Client.ID==client.ID).OrderBy(a => a.Number).ToList();
        public static Appointment GetLastAppointment(Client client)
        {
            try
            {
                return db.Appointment.Where(id => id.Client.ID == client.ID).OrderBy(a => a.Number).Last();
            }
            catch(Exception ex)
            {
                Appointment ap;
                return ap = new Appointment("-","-",DateTime.Now,client,0);
            }    
        }
        public static void Add(Appointment appointment)
        {
            db.Appointment.Add(appointment);
            db.SaveChanges();
        }
        public static void Save() => db.SaveChanges();

        public string GetDate
        {
            get
            {
                return StartTime.ToString("f");
            }
        }
    }
}
