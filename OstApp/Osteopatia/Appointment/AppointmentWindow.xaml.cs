using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Osteopatia.TImeTable;
using OstLib;

namespace Osteopatia
{
    /// <summary>
    /// Логика взаимодействия для AppointmentWindow.xaml
    /// </summary>
    public partial class AppointmentWindow : Window
    {
        Client client;
        public AppointmentWindow(Client client1)
        {
            InitializeComponent();
            this.client = client1;
            this.Title = client.GetName;
            int yearsOld;
            if(Int32.TryParse(client.YearOfBirth, out yearsOld))
                yearsOld = DateTime.Today.Year - int.Parse(client.YearOfBirth);
            else
            {
                MessageBox.Show("Год рождения введён неправильно");
                return;
            }
            FIOBlock.Text = $"{client.GetName} ({yearsOld} л.)";
            AnamnezBlock.Text = "Анамнез:\n" + client.Anamnez;
            GinekologBlock.Text = "Гинекология:\n" + client.Ginekologia;
            OperationBlock.Text = "Операции:\n" + client.Operation;
            InjuryBlock.Text = "Травмы:\n" + client.Injury;
            FillingData();
        }

        private void AppointmentBut_Click(object sender, RoutedEventArgs e)
        {
            AddAppointmentWindow aaw = new AddAppointmentWindow(client,this);
            aaw.Show();
        }

        private void Row_DoubleClick(object sender, RoutedEventArgs e)
        {
            var a = dgAppointment.SelectedItem as Appointment;
            var aaw = new AddAppointmentWindow(a,this);
            if (a != null)
            {
                aaw.NumberBox.Text = a.Number.ToString();
                aaw.TimeBox.Text = a.StartTime.ToString("t");
                aaw.DateBox.Text = a.StartTime.ToString("d");
                aaw.ComplaintBox.Text = a.Complaint;
                aaw.HealBox.Text = a.Heal;
                aaw.Show();
            }
            else
                MessageBox.Show("Сначала выберите запись для подтверждения");
        }

        public void FillingData()
        {
            List<Appointment> list;
            list = Appointment.GetAllForThisClient(client);
            dgAppointment.ItemsSource = list;
        }
        
        private void AddTimeTableLineBut_Click(object sender, RoutedEventArgs e)
        {
            var addTimeTableLineWindow = new AddTimeTableLineWindow(this.client,new TimeTablePage());
            addTimeTableLineWindow.Show();
        }
    }
}
