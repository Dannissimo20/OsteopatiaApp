using System;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        private Client client;
        private ClientPage cp;
        public AddClientWindow(Client client, ClientPage cp)
        {
            InitializeComponent();
            CityBox.Text = "Киров";
            this.client = client;
            if(client!=null)
            {
                NumberBox.IsEnabled = false;
                DateBox.IsEnabled = false;
                TimeBox.IsEnabled = false;
                ComplaintBox.IsEnabled = false;
                HealBox.IsEnabled = false;
            }
            else
            {
                NumberBox.Text = "1";
                DateBox.Text = DateTime.Today.ToShortDateString();
                if (DateTime.Now.Minute < 30)
                    TimeBox.Text = DateTime.Now.Hour + ":00";
                else
                    TimeBox.Text = DateTime.Now.Hour + ":30";
            }
            this.cp = cp;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] fio = GetSplitFullName(FIOBox.Text);
                string f = fio[0];
                string i = fio[1];
                string o = null;
                int yearsOld;

                if (fio.Length == 3)
                    o = fio[2];
                
                if(client != null)
                {
                    client.PhoneNumber = PhoneBox.Text;
                    client.Surname = f;
                    client.Name = i;
                    client.MiddleName = o;
                    client.YearOfBirth = YearBox.Text;
                    client.City = CityBox.Text;
                    client.Anamnez = AnamnezBox.Text;
                    client.Ginekologia = GinekologBox.Text;
                    client.Operation = OperationBox.Text;
                    client.Injury = InjuryBox.Text;
                    Client.Save();
                    cp.FillingData();
                }
                else
                {
                    bool isExist = false;
                    if (YearBox.Text.Length == 4 && Int32.TryParse(YearBox.Text, out yearsOld))
                    {
                        if(CityBox.Text.Length > 0)
                        {
                            List<Client> clients = Client.GetAll();
                            for (int j = 0; j<clients.Count; j++)
                                if (f == clients[j].Surname && i == clients[j].Name && PhoneBox.Text == clients[j].PhoneNumber)
                                    isExist = true;
                            if (!isExist)
                            {
                                Appointment appointment;
                                client = new Client(f, i, o, PhoneBox.Text, YearBox.Text, CityBox.Text, AnamnezBox.Text, GinekologBox.Text, OperationBox.Text, InjuryBox.Text);
                                string NewDate = DateBox.Text + " " + TimeBox.Text;
                                appointment = new Appointment(ComplaintBox.Text, HealBox.Text, DateTime.Parse(NewDate), client, Int32.Parse(NumberBox.Text));
                                Client.Add(client);
                                Appointment.Add(appointment);
                                cp.FillingData();
                                Close();
                            }
                            else
                                MessageBox.Show("Обшибка!\nЕсть уже такой клиент");
                        }
                        else
                            MessageBox.Show("Обшибка!\nНеверно заполнено поле \"Город\"");
                    }
                    else
                    {
                        MessageBox.Show("Обшибка!\nНеверно заполнено поле \"Год рождения\"");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Обшибка!\n" + ex.Message);
            }
        }

        private string[] GetSplitFullName(string fullName)
        {
            fullName = Regex.Replace(fullName, "[ ]+", " ");
            return fullName.Split(' ');
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
