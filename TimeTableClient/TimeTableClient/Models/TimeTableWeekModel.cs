using System.Collections.Generic;

namespace TimeTableClient.Models;

public class TimeTableWeekModel
{
    public string time { get; set; }
    public string? day1 { get; set; }
    public string? day2 { get; set; }
    public string? day3 { get; set; }
    public string? day4 { get; set; }
    public string? day5 { get; set; }
    public string? day6 { get; set; }
    public string? day7 { get; set; }
    public List<string>? listOfDays { get; set; }
    
    public TimeTableWeekModel(){}

    public TimeTableWeekModel(string time, List<string> listOfDays)
    {
        this.time = time;
        day1 = listOfDays[0];
        day2 = listOfDays[1];
        day3 = listOfDays[2];
        day4 = listOfDays[3];
        day5 = listOfDays[4];
        day6 = listOfDays[5];
        day7 = listOfDays[6];
    }

    public TimeTableWeekModel(string time, string day1,
        string day2,
        string day3,
        string day4,
        string day5,
        string day6,
        string day7
    )
    {
        this.time = time;
        this.day1 = day1;
        this.day2 = day2;
        this.day3 = day3;
        this.day4 = day4;
        this.day5 = day5;
        this.day6 = day6;
        this.day7 = day7;
    }
}