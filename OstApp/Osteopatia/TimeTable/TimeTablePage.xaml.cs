using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Xml.Serialization;
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
        private byte[] buffer = new byte[1024];
        private Socket sListener;
        public TimeTablePage()
        {
            IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
            IPEndPoint localend = new IPEndPoint(ipAdr, 11001);
            if(!MainWindow.isTrue){
            Thread workThread = new Thread(() =>
            {
                while (true)
                {
                    sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    sListener.Bind(localend);
                    sListener.Listen(10);
                    try
                    {
                        Socket handler = sListener.Accept();
                        while (true)
                        {
                            int bytesRec = handler.Receive(buffer);
                            List<TimeTableUdpModel> listOfModels = new List<TimeTableUdpModel>();
                            List<TimeTableEntry> listOfEntries = TimeTableEntry.FindAll();
                            foreach (var item in listOfEntries)
                            {
                                var tmp = new TimeTableUdpModel(item.DateTime,
                                    item.Client.Surname,
                                    item.Client.Name,
                                    item.Client.PhoneNumber);
                                listOfModels.Add(tmp);
                            }
                            XmlSerializer fileSerializer = new XmlSerializer(typeof(List<TimeTableUdpModel>));
                            MemoryStream stream = new MemoryStream();
                            fileSerializer.Serialize(stream, listOfModels);
                            stream.Position = 0;
                            byte[] bytesSend = new byte[stream.Length];
                            stream.Read(bytesSend, 0, Convert.ToInt32(stream.Length));
                            handler.Send(bytesSend);
                            stream.Close();
                            int k = int.Parse(Encoding.ASCII.GetString(buffer, 0, bytesRec));
                        }
                    }
                    catch(SocketException e)
                    {
                        //MessageBox.Show("Опа, кто-то выключил клиента");
                        sListener.Close();
                    }
                }
            });
            workThread.Start();
            MainWindow.isTrue = true;
            }
            InitializeComponent();
            FillingData();
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
                dataGridColumns[i].Header = $"{DateTime.Today.AddDays(i+dayOfWeekSubtract+weekNumber*7).ToString("d.MM.yy ddd")}";
            
            List<TimeTableWeekModel> listOfRows = new List<TimeTableWeekModel>();
            
            for (int i = 9; i <= 19; i++)
            {
                List<string> listOfDays = new List<string>();
                for (int k = 0; k < dataGridColumns.Count; k++)
                {
                    string cell = TimeTableEntry.GetTimeTableLineByDate(
                                  DateTime.Today.AddDays(k+dayOfWeekSubtract+weekNumber*7).AddHours(i))
                                  .Client.GetNameWithoutMiddleName;
                    cell += "\n"+TimeTableEntry.GetTimeTableLineByDate(
                            DateTime.Today.AddDays(k + dayOfWeekSubtract + weekNumber * 7).AddHours(i))
                            .Client.PhoneNumber;
                    listOfDays.Add(cell);
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