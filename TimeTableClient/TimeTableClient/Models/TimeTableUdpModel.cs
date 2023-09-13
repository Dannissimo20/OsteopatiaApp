using System;

namespace TimeTableClient.Models;

[Serializable]
public class TimeTableUdpModel
{
    public DateTime TimeTableDateTime { get; set; }
    public string ClientSurname { get; set; }
    public string ClientName { get; set; }
    public string ClientPhoneNumber { get; set; }
}