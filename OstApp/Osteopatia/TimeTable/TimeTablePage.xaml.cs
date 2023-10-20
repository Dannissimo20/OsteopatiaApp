using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Flurl.Http;
using Osteopatia.TimeTable;
using Osteopatia.TImeTable;
using OstLib;
using OstLib.Models;

namespace Osteopatia
{
    public partial class TimeTablePage
    {
        // Параметр weekNumber показывает какую неделю нужно вывести в DataGrid
        // 0 - текущая неделя
        // 1 - следующая неделя
        // -2 - неделя которая была 2 недели назад (да тавтология, и что такого?)
        private int _weekNumber;
        public TimeTablePage()
        {
            InitializeComponent();
            FillingData();
        }

        public IEnumerable<TimeTableUdpModel> GetTimeTablesForThisWeek(int weekNumberJson)
        {
            var res = "http://localhost:5000/TimeTable/getWeekTable".PostJsonAsync(new TimeTableWeekModelJSON(weekNumberJson)).Result;
            var list = res.GetJsonAsync<IEnumerable<TimeTableUdpModel>>().Result;
            return list;
        }

        public void FillingData()
        {
            /*
             * Задумка проста - в зависимости от дня недели переменная dayOfWeekSubtract будет вычитаться
             * из DateTime каждого названия столбца и каждого DateTime в поиске записей
             * Таким образом первый столбец в таблице всегда будет понедельником, второй вторником и т.д.
             */
            
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
                dataGridColumns[i].Header = $"{DateTime.Today.AddDays(i + dayOfWeekSubtract + _weekNumber*7).ToString("d.MM.yy ddd")}";

            List<TimeTableWeekModel> listOfRows = new List<TimeTableWeekModel>();

            var list1 = GetTimeTablesForThisWeek(_weekNumber);

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
            _weekNumber--;
            FillingData();
        }
        private void RightButton_OnClick(object sender, RoutedEventArgs e)
        {
            _weekNumber++;
            FillingData();
        }

        private void DefaultWeekButton_OnClick(object sender, RoutedEventArgs e)
        {
            _weekNumber = 0;
            FillingData();
        }

        private void AddTimeTableLineButton_OnClick(object sender, RoutedEventArgs e)
        {
            var attWindow = new AddTimeTableLineWindow(this);
            attWindow.Show();
        }
        
        private void RemoveTimeTableButton_OnClick(object sender, RoutedEventArgs e)
        {
            var ttsWindow = new TimeTableStateWindow(this);
            ttsWindow.Show();
        }
    }
}
