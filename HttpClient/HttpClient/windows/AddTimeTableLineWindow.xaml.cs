using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Flurl.Http;
using HttpClient.pages;

namespace HttpClient.windows
{
    public partial class AddTimeTableLineWindow
    {
        private Client _client;
        private TimeTablePage _timeTablePage;
        private string _connection = App.url;

        private Task? _task;
        private CancellationTokenSource _cancellation;
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
            var res = $"{_connection}TimeTable/getByDate".PostJsonAsync(new TimeTableDateModel(date1)).Result;
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
            var res = $"{_connection}TimeTable/addTableLine".PostJsonAsync(tableLineModel).Result;
            var code = res.GetJsonAsync<int>().Result;
            return Task.FromResult(code);
        }

        public IEnumerable<Client> GetClientsBySurname(SurnameModel surnameModel)
        {
            var res = $"{_connection}Client/getBySurname".PostJsonAsync(surnameModel).Result;
            var list = res.GetJsonAsync<IEnumerable<Client>>().Result;
            return list;
        }

        public DateTime GetLastDateForClient(TimeTableLineModel tableLineModel)
        {
            var req = $"{_connection}TimeTable/getLastDate".PostJsonAsync(tableLineModel).Result;
            var res = req.GetJsonAsync<DateTime>().Result;
            return res;
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
                TimePicker.SelectedTime.Value.Minute != 0)
            {
                MessageBox.Show("Проверьте правильность заполнения поля \"Время\"\n" +
                                "Выбранное время должно быть в пределах от 9 до 19 часов\n" +
                                "Минуты должны быть равны 00",
                    "Что за тяга к необъяснимому?",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (AddCalendar.SelectedDate.Value < DateTime.Now)
            {
                MessageBox.Show("Нельзя записать человека на дату или время которое уже прошло",
                    "Путешествуем во времени?",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var item = GetTimeTableByDate(DateTime.Parse(
                $"{AddCalendar.SelectedDate.Value.ToString("d")}" + 
                $" {TimePicker.SelectedTime.Value.ToString("t")}")).Result;
            
            var dateTime = DateTime.Parse($"{AddCalendar.SelectedDate.Value.ToString("d")} {TimePicker.SelectedTime.Value.ToString("t")}");
            if (item.DateTime.Equals(dateTime))
            {
                MessageBox.Show($"Данное время уже занято {item.Client.GetNameWithoutMiddleName}\n" +
                                $"Выберите другое время",
                    "Боже мой, какая встреча!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Stop);
                return;
            }

            var item_client = new TimeTableLineModel(null, SurnameBox.Text, NameBox.Text, PhoneBox.Text);
            var dateOfLastLine = GetLastDateForClient(item_client);
            if (AddCalendar.SelectedDate.Value - dateOfLastLine <=TimeSpan.Parse("14") &&
                AddCalendar.SelectedDate.Value > dateOfLastLine)
            {
                var result =  MessageBox.Show($"14 дней с последнего приема еще не прошло\n" +
                                              $"Последний прием был {dateOfLastLine}\n"+
                                              $"Продолжить?",
                    "Доп нагрузка!",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Warning);
                if (result == MessageBoxResult.Cancel)
                    return;

            }
            else if (AddCalendar.SelectedDate.Value - dateOfLastLine >= TimeSpan.Parse("-14") &&
                     AddCalendar.SelectedDate.Value < dateOfLastLine)
            {
                var result =  MessageBox.Show($"14 дней до следующего приема не пройдёт\n" +
                                              $"Следующий прием будет {dateOfLastLine}\n"+
                                              $"Продолжить?",
                    "Доп нагрузка!",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Warning);
                if (result == MessageBoxResult.Cancel)
                    return;
            }

            #endregion
            
            var ttlm = new TimeTableLineModel(dateTime.ToString(), SurnameBox.Text, NameBox.Text, PhoneBox.Text);
            int code = AddTimeTableLine(ttlm).Result;
            if (code == 400)
                new Exception();
            _timeTablePage.FillingData();
            Close();
        }


        private void SurnameBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_task is not null && !_task.IsCompleted)
            {
                _cancellation!.Cancel();
            }
            var surname = SurnameBox.Text;
            _cancellation = new CancellationTokenSource();
            _task = Task.Delay(new TimeSpan(0, 0, 0,1), _cancellation.Token)
            .ContinueWith(_ =>
            {
                var list = GetClientsBySurname(new SurnameModel(surname));
                ClientList.Dispatcher.Invoke(() => ClientList.ItemsSource = list);
            }, _cancellation.Token);
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
