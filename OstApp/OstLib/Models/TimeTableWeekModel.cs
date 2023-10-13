#nullable enable
using System.Collections.Generic;

namespace OstLib.Models;

public class TimeTableWeekModel
{
    public string Time { get; set; }
    public string? Day1 { get; set; }
    public string? Day2 { get; set; }
    public string? Day3 { get; set; }
    public string? Day4 { get; set; }
    public string? Day5 { get; set; }
    public string? Day6 { get; set; }
    public string? Day7 { get; set; }
    public List<string>? ListOfDays { get; set; }
    
    public TimeTableWeekModel(){}

    public TimeTableWeekModel(string time, List<string> listOfDays)
    {
        this.Time = time;
        Day1 = listOfDays[0];
        Day2 = listOfDays[1];
        Day3 = listOfDays[2];
        Day4 = listOfDays[3];
        Day5 = listOfDays[4];
        Day6 = listOfDays[5];
        Day7 = listOfDays[6];
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
        this.Time = time;
        this.Day1 = day1;
        this.Day2 = day2;
        this.Day3 = day3;
        this.Day4 = day4;
        this.Day5 = day5;
        this.Day6 = day6;
        this.Day7 = day7;
    }
}