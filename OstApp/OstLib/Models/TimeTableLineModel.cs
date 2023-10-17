namespace OstLib
{
    public class TimeTableLineModel
    {
        public string Date { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public TimeTableLineModel(string date, string surname, string name, string phone)
        {
            Date = date;
            Surname = surname;
            Name = name;
            Phone = phone;
        }
    }
}
