using OstLib;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Osteopatia
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public ClientPage()
        {
            InitializeComponent();
            FillingData();
        }

        private void AddClientClick(object sender, RoutedEventArgs e)
        {
            var acw = new AddClientWindow(null, this);
            acw.Show();
            FillingData();
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            FillingData();
        }
        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var c = dgClient.SelectedItem as Client;
                var acw = new AddClientWindow(c, this);
                if (c != null)
                {
                    acw.PhoneBox.Text = c.PhoneNumber;
                    acw.FIOBox.Text = c.GetName;
                    acw.YearBox.Text = c.YearOfBirth;
                    acw.CityBox.Text = c.City;
                    acw.AnamnezBox.Text = c.Anamnez;
                    acw.GinekologBox.Text = c.Ginekologia;
                    acw.OperationBox.Text = c.Operation;
                    acw.InjuryBox.Text = c.Injury;
                    acw.Show();
                }
                else
                    MessageBox.Show("Сначала выберите запись для подтверждения");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Упс. Что-то пошло не так\n"+ex);
            }
        }

        public void FillingData()
        {
            List<Client> list;
            list = Client.GetAll();
            dgClient.ItemsSource = list;
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Client client = dgClient.SelectedItem as Client;
            AppointmentWindow app = new AppointmentWindow(client);
            app.Show();
        }

        private void ClientSearch_Click(object sender, RoutedEventArgs e)
        {
            List<Client> list;
            string[] FullName = SurnameBox.Text.Split(" ");
            if (FullName.Length == 1)
                list = Client.SearchClient(FullName[0]);
            else if (FullName.Length == 2)
                list = Client.SearchClient(FullName[0], FullName[1]);
            else if (FullName.Length == 3)
                list = Client.SearchClient(FullName[0], FullName[1], FullName[2]);
            else
            {
                MessageBox.Show("Неверно заполненно поле поиска");
                return;
            }
            if(list.Count == 0)
            {
                MessageBox.Show("Ничего не найдено");
                return;
            }
            dgClient.ItemsSource = list;
        }
    }
}
