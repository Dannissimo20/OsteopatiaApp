using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Xml.Serialization;
using Flurl.Http;
using MaterialDesignThemes.Wpf;
using Osteopatia.TImeTable;
using OstLib;
using DataGridTextColumn = System.Windows.Controls.DataGridTextColumn;

namespace Osteopatia
{
    public partial class TimeTablePage : Page
    {
        // Параметр weekNumber показывает какую неделю нужно вывести в DataGrid
        // 0 - текущая неделя
        // 1 - следующая неделя
        // -2 - неделя которая была 2 недели назад (да тавтология, и что такого?)
        public int weekNumber = 0;
        public TimeTablePage()
        {
            InitializeComponent();
            FillingData();
        }

        /*public async Task<IEnumerable<TimeTableUdpModel>> GetTimeTables()
        {
            var list = await "http://localhost:8759/TimeTable".GetJsonAsync<IEnumerable<TimeTableUdpModel>>();
            return list;
        }*/

        public async Task<IEnumerable<TimeTableUdpModel>> GetTimeTablesForThisWeek(int weekNumberJson)
        {
            var res = "http://localhost:8759/TimeTable".PostJsonAsync(new TimeTableWeekModelJSON(weekNumberJson)).Result;
            var list = res.GetJsonAsync<IEnumerable<TimeTableUdpModel>>().Result;
            return list;
        }

        public void FillingData()
        {
            // Задумка проста - в зависимости от дня недели переменная dayOfWeekSubtract будет вычитаться
            // из DateTime каждого названия столбца и каждого DateTime в поиске записей
            // Таким образом первый столбец в таблице всегда будет понедельником, второй вторником и т.д.
            int dayOfWeekSubtract = 0;
            DayOfWeek dayOfWeek = DateTime.Today.DayOfWeek;
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    dayOfWeekSubtract = 0;
                    break;
                case DayOfWeek.Tuesday:
                    dayOfWeekSubtract = -1;
                    break;
                case DayOfWeek.Wednesday:
                    dayOfWeekSubtract = -2;
                    break;
                case DayOfWeek.Thursday:
                    dayOfWeekSubtract = -3;
                    break;
                case DayOfWeek.Friday:
                    dayOfWeekSubtract = -4;
                    break;
                case DayOfWeek.Saturday:
                    dayOfWeekSubtract = -5;
                    break;
                case DayOfWeek.Sunday:
                    dayOfWeekSubtract = -6;
                    break;
            }

            List<DataGridColumn> dataGridColumns = new List<DataGridColumn> {
                day1Col,
                day2Col,
                day3Col,
                day4Col,
                day5Col,
                day6Col,
                day7Col,
            };

            // Формирование названий столбцов
            for (int i = 0; i < dataGridColumns.Count; i++)
                dataGridColumns[i].Header = $"{DateTime.Today.AddDays(i + dayOfWeekSubtract + weekNumber*7).ToString("d.MM.yy ddd")}";

            List<TimeTableWeekModel> listOfRows = new List<TimeTableWeekModel>();

            var list1 = GetTimeTablesForThisWeek(weekNumber).Result;

            for (int i = 9; i <= 19; i++)
            {
                List<string> listOfDays = new List<string>();
                for (int k = 1; k < 8; k++)
                {
                    int weekNum = k;
                    if (k == 7)
                        weekNum = 0;
                    var cell = list1.FirstOrDefault(l => l.TimeTableDateTime.Hour == i &&
                                                         (int) l.TimeTableDateTime.DayOfWeek == weekNum);
                    string cellStr;
                    if (cell == null)
                        cellStr = "";
                    else
                        cellStr = cell.ClientSurname + " " + 
                                  cell.ClientName + "\n" + 
                                  cell.ClientPhoneNumber;
                    listOfDays.Add(cellStr);
                }
                TimeTableWeekModel row = new TimeTableWeekModel($"{i}:00", listOfDays);
                listOfRows.Add(row);
            }

            dgTimeTable.ItemsSource = listOfRows;
        }

        private void LeftButton_OnClick(object sender, RoutedEventArgs e)
        {
            weekNumber--;
            FillingData();
        }
        private void RightButton_OnClick(object sender, RoutedEventArgs e)
        {
            weekNumber++;
            FillingData();
        }

        private void defaultWeekButton_OnClick(object sender, RoutedEventArgs e)
        {
            weekNumber = 0;
            FillingData();
        }

        private void AddTimeTableLineButton_OnClick(object sender, RoutedEventArgs e)
        {
            var attWindow = new AddTimeTableLineWindow(this);
            attWindow.Show();
        }
    }
}
