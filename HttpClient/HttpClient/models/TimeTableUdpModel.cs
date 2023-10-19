using System;

namespace HttpClient;

[Serializable]
public class TimeTableUdpModel
{
    public DateTime TimeTableDateTime { get; set; }
    public string ClientSurname { get; set; }
    public string ClientName { get; set; }
    public string ClientPhoneNumber { get; set; }
    //public int UDPMODELSIZE { get; set; }

    public TimeTableUdpModel(){}

    public TimeTableUdpModel(DateTime dateTime, string surname, string name, string phone)
    {
        TimeTableDateTime = dateTime;
        ClientSurname = surname;
        ClientName = name;
        ClientPhoneNumber = phone;
    }
}