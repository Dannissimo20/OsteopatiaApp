namespace HttpServer.Models
{
    public class TimeTableEntryModel
    {
        public DateTime TimeTableDateTime { get; set; }
        public string ClientSurname { get; set; }
        public string ClientName { get; set; }
        public string ClientPhoneNumber { get; set; }

        public TimeTableEntryModel(){}

        public TimeTableEntryModel(DateTime dateTime, string surname, string name, string phone)
        {
            TimeTableDateTime = dateTime;
            ClientSurname = surname;
            ClientName = name;
            ClientPhoneNumber = phone;
        }
    }
}
