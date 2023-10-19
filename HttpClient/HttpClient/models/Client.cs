using System;
using System.Collections.Generic;
using System.Linq;


namespace HttpClient
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
