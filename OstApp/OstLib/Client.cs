using System;
using System.Collections.Generic;
using System.Linq;


namespace OstLib
{
    public class Client
    {
        public int ID { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string PhoneNumber { get; set; }
        public string YearOfBirth { get; set; }
        public string City { get; set; }
        public string Anamnez { get; set; }
        public string Ginekologia { get; set; }
        public string Operation { get; set; }
        public string Injury { get; set; }
        public virtual List<Appointment> Appointments { get; set; }
        public virtual List<TimeTableEntry>? TimeTableLines { get; set; }
        private static ApplicationContext db = Context.db;
        public Client() { }
        public Client(string Surname, string Name, string MiddleName, string PhoneNumber, string YearOfBirth, string City, string Anamnez, string Ginekologia, string Operation, string Injury)
        {
            this.Surname = Surname;
            this.Name = Name;
            this.MiddleName = MiddleName;
            this.PhoneNumber = PhoneNumber;
            this.YearOfBirth = YearOfBirth;
            this.City = City;
            this.Anamnez = Anamnez;
            this.Ginekologia = Ginekologia;
            this.Operation = Operation;
            this.Injury = Injury;
        }

        public static Client GetClientById(int id) => db.Client.Find(id);
        public static List<Client> GetAll() => db.Client.OrderBy(c => c.Surname).ToList();
        public static List<Client> SearchClient(string s) => db.Client.Where(c => c.Surname.Contains(s) || c.Name.Contains(s)).ToList();
        public static List<Client> SearchClient(string s, string n) => db.Client.Where(c => c.Surname.Contains(s) && c.Name.Contains(n)).ToList();
        public static List<Client> SearchClient(string s, string n, string m) => db.Client.Where(c => c.Surname.Contains(s) && c.Name.Contains(n) && c.MiddleName.Contains(m)).ToList();

        public static List<Client> SearchBySurname(string s)
        {
            try
            {
                s = s.ToLower();
                var s1 = s.Substring(0, 1);
                s1 = s1.ToUpper();
                s = s.Substring(1);
                s = s1 + s;
                return db.Client.Where(l => l.Surname.Contains(s)).ToList();
            }
            catch
            {
                return new List<Client>();
            }
            
        }
        public static void Add(Client client)
        {
            db.Client.Add(client);
            db.SaveChanges();
        }
        public static void Save() => db.SaveChanges();

        public string GetName
        {
            get
            {
                string tmp = $"{Surname} {Name} ";
                if (MiddleName != null)
                    tmp += $"{MiddleName}";
                return tmp;
            }
        }

        public string GetNameWithoutMiddleName
        {
            get
            {
                string tmp = $"{Surname} {Name}";
                return tmp;
            }
        }

        public string GetNameWithPhone
        {
            get
            {
                string tmp = $"{Surname} {Name}\n{PhoneNumber}";
                return tmp;
            }
        }
    }
}
