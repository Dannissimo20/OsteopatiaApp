using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.Logging.Abstractions;
using OstLib;

namespace Osteopatia.TImeTable;

public partial class AddTimeTableLineWindow : Window
{
    private Client client;
    private TimeTablePage timeTablePage;
    public AddTimeTableLineWindow(Client clientFromAppointment, TimeTablePage ttp)
    {
        InitializeComponent();
        client = clientFromAppointment;
        SurnameBox.Text = client.Surname;
        NameBox.Text = client.Name;
        PhoneBox.Text = client.PhoneNumber;
        SurnameBox.IsEnabled = false;
        NameBox.IsEnabled = false;
        PhoneBox.IsEnabled = false;
        ClientList.IsEnabled = false;
        timeTablePage = ttp;

    }
    public AddTimeTableLineWindow(TimeTablePage ttp)
    {
        InitializeComponent();
        timeTablePage = ttp;
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

        if (TimePicker.SelectedTime.Value.Hour<8 || 
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
            var item = TimeTableEntry.GetTimeTableLineByDate(DateTime.Parse(
                $"{AddCalendar.SelectedDate.Value.ToString("d")}" +
                $" {TimePicker.SelectedTime.Value.ToString("t")}"));

            if (item.Client.Equals(client))
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

        if (client == null && ClientList.SelectedItem==null)
        {
            var localClient = new Client(SurnameBox.Text,
                NameBox.Text,
                "",
                PhoneBox.Text,
                "0",
                "-",
                null,
                null,
                null,
                null);
            client = localClient;
            Client.Add(localClient);
        }
        var dateTime = DateTime.Parse($"{AddCalendar.SelectedDate.Value.ToString("d")} {TimePicker.SelectedTime.Value.ToString("t")}");
        TimeTableEntry tte = new TimeTableEntry(dateTime, client);
        TimeTableEntry.Add(tte);
        timeTablePage.FillingData();
        //Close();
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
        client = s;
    }
}