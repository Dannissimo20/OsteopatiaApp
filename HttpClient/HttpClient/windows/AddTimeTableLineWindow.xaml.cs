using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Flurl.Http;
using HttpClient.pages;
using OstLib;

namespace HttpClient.windows
{
    public partial class AddTimeTableLineWindow
    {
        private Client _client;
        private TimeTablePage _timeTablePage;
        public AddTimeTableLineWindow(Client clientFromAppointment, TimeTablePage ttp)
        {
            InitializeComponent();
            _client = clientFromAppointment;
            SurnameBox.Text = _client.Surname;
            NameBox.Text = _client.Name;
            PhoneBox.Text = _client.PhoneNumber;
            SurnameBox.IsEnabled = false;
            NameBox.IsEnabled = false;
            PhoneBox.IsEnabled = false;
            ClientList.IsEnabled = false;
            _timeTablePage = ttp;

        }
        public AddTimeTableLineWindow(TimeTablePage ttp)
        {
            InitializeComponent();
            _timeTablePage = ttp;
        }

        public Task<TimeTableEntry> GetTimeTableByDate(DateTime date)
        {
            var date1 = "";
            if (date.Hour < 10)
                date1 = $"{date.Year}-{date.Month}-{date.Day}T0{date.Hour}:00:00+03:00";
            else
                date1 = $"{date.Year}-{date.Month}-{date.Day}T{date.Hour}:00:00+03:00";
            var res = "http://localhost:8759/TimeTable/getByDate".PostJsonAsync(new TimeTableDateModel(date1)).Result;
            var list = res.GetJsonAsync<TimeTableEntry>().Result;
            return Task.FromResult(list);
        }

        public Task<int> AddTimeTableLine(TimeTableLineModel tableLineModel)
        {
            var date = DateTime.Parse(tableLineModel.Date);
            var date1 = "";
            if (date.Hour < 10)
                date1 = $"{date.Year}-{date.Month}-{date.Day}T0{date.Hour}:00:00+03:00";
            else
                date1 = $"{date.Year}-{date.Month}-{date.Day}T{date.Hour}:00:00+03:00";
            tableLineModel.Date = date1;
            var res = "http://localhost:8759/TimeTable/addTableLine".PostJsonAsync(tableLineModel).Result;
            var code = res.GetJsonAsync<int>().Result;
            return Task.FromResult(code);
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            #region Проверка на правильность заполнения полей и на уже существующую запись

            if (!AddCalendar.SelectedDate.HasValue)
            {
                MessageBox.Show("Выберите дату, на которую нужно записать клиента",
                    "Последнее предупреждение!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (!TimePicker.SelectedTime.HasValue)
            {
                MessageBox.Show("Выберите время на которое нужно записать клиента",
                    "Последнее китайское предупреждение!!!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (TimePicker.SelectedTime.Value.Hour < 8 ||
                TimePicker.SelectedTime.Value.Hour > 19 ||
                TimePicker.SelectedTime.Value.Minute%30 != 0)
            {
                MessageBox.Show("Проверьте правильность заполнения поля \"Время\"\n" +
                                "Выбранное время должно быть в пределах от 9 до 19 часов\n" +
                                "Минуты должны быть равны 00 или 30",
                    "Что за тяга к необъяснимому?",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (AddCalendar.SelectedDate.Value.Day < DateTime.Now.Day ||
                AddCalendar.SelectedDate.Value.Month < DateTime.Now.Month ||
                AddCalendar.SelectedDate.Value.Year < DateTime.Now.Year
               )
            {
                MessageBox.Show("Нельзя записать человека на дату или время которое уже прошло",
                    "Путешествуем во времени?",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (TimeTableEntry.isTimeTableEntryExists(DateTime.Parse($"{AddCalendar.SelectedDate.Value.ToString("d")}" +
                                                                     $" {TimePicker.SelectedTime.Value.ToString("t")}")))
            {

                var item = GetTimeTableByDate(DateTime.Parse(
                    $"{AddCalendar.SelectedDate.Value.ToString("d")}" +
                    $" {TimePicker.SelectedTime.Value.ToString("t")}")).Result;

                if (item.Client.Equals(_client))
                {
                    MessageBox.Show($"Занимаешь место на которое этот клиент уже записан? Странно\n",
                        "Боже мой, какая встреча... Кажется я вас уже где-то видел!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Stop);
                    return;
                }
                MessageBox.Show($"Данное время уже занято {item.Client.GetNameWithoutMiddleName}\n" +
                                $"Выберите другое время",
                    "Боже мой, какая встреча!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Stop);
                return;
            }

            #endregion
            
            var dateTime = DateTime.Parse($"{AddCalendar.SelectedDate.Value.ToString("d")} {TimePicker.SelectedTime.Value.ToString("t")}");
            var ttlm = new TimeTableLineModel(dateTime.ToString(), SurnameBox.Text, NameBox.Text, PhoneBox.Text);
            int code = AddTimeTableLine(ttlm).Result;
            if (code == 400)
                new Exception();
            _timeTablePage.FillingData();
            Close();
        }


        private void SurnameBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var list = Client.SearchBySurname(SurnameBox.Text);
            ClientList.ItemsSource = list;
        }

        private void ClientList_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var s = (Client) ClientList.SelectedItem;
            SurnameBox.Text = s.Surname;
            NameBox.Text = s.Name;
            PhoneBox.Text = s.PhoneNumber;
            _client = s;
        }
    }
}
