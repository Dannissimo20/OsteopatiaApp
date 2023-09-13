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
using OstLib;

namespace Osteopatia
{
    /// <summary>
    /// Логика взаимодействия для AddAppointmentWindow.xaml
    /// </summary>
    public partial class AddAppointmentWindow : Window
    {
        Client client;
        private Appointment appointment;
        private AppointmentWindow aw;
        public AddAppointmentWindow(Client client, AppointmentWindow aw)
        {
            InitializeComponent();
            Appointment lastA = Appointment.GetLastAppointment(client);
            NumberBox.Text = $"{lastA.Number+1}";
            DateBox.Text = DateTime.Today.ToShortDateString();
            if (DateTime.Now.Minute < 30)
                TimeBox.Text = DateTime.Now.Hour + ":00";
            else
                TimeBox.Text = DateTime.Now.Hour + ":30";
                
            this.client = client;
            this.aw = aw;
        }
        public AddAppointmentWindow(Appointment appointment, AppointmentWindow aw)
        {
            InitializeComponent();
            this.appointment = appointment;
            this.aw = aw;
        }

        private void AddAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (NumberBox.Text == "")
            { 
                MessageBox.Show("Не заполнен номер приёма");
                return;
            }
            try
            {
                if(client!=null)
                {
                    string NewDate = DateBox.Text + " " + TimeBox.Text;
                    Appointment app = new Appointment(ComplaintBox.Text, HealBox.Text, DateTime.Parse(NewDate), client, Int32.Parse(NumberBox.Text));
                    Appointment.Add(app);
                    aw.FillingData();
                    Close();
                }
                else
                {
                    string NewDate = (DateBox.Text + " " + TimeBox.Text);
                    appointment.Number = Int32.Parse(NumberBox.Text);
                    appointment.StartTime = DateTime.Parse(NewDate);
                    appointment.Complaint = ComplaintBox.Text;
                    appointment.Heal = HealBox.Text;
                    Appointment.Save();
                    aw.FillingData();
                    Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Проверьте заполнение полей");
            }
        }

        private void TimeBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string time = TimeBox.Text;
            int val;

            if (time.Length == 2)
            {
                TimeBox.Text = time + ":";
                TimeBox.SelectionStart = TimeBox.Text.Length; //коретка в конец строки
            }
            if (time.Length >= 5)
            {
                e.Handled = true; // отклоняем ввод
            }
            if (!Int32.TryParse(e.Text, out val))
            {
                e.Handled = true; // отклоняем ввод
            }
        }
        private void TimeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string time = TimeBox.Text;

            if (time.Length == 1)
            {
                if (Convert.ToInt32(time) > 2)
                {
                    TimeBox.Text = "0" + time;
                    TimeBox.SelectionStart = TimeBox.Text.Length;
                }
            }
            if (time.Length == 2)
            {
                if (Convert.ToInt32(time.Substring(0, 2)) > 23)
                {
                    TimeBox.Text = time.Remove(1, 1);
                    TimeBox.SelectionStart = TimeBox.Text.Length;
                }
            }
            if (time.Length == 4)
            {
                if (Convert.ToInt32(time.Substring(3, 1)) > 5)
                {
                    TimeBox.Text = time.Substring(0, 3) + "0" + time.Substring(3, 1);
                    TimeBox.SelectionStart = TimeBox.Text.Length;
                }
            }
        }
        private void TimeBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true; // если пробел, отклоняем ввод
            }
        }
    }
}
