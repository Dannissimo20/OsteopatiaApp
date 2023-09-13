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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;
using OstLib;

namespace Osteopatia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Page Clientt;
        private readonly Page TimeTable;
        public static bool isTrue = false;
        public MainWindow()
        {
            new ApplicationContext(ApplicationContext.GetDb());
            /*List<Client> clients = Client.GetAll();
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].City = "Киров";
                Client.Save();
            }*/
            
            Clientt = new ClientPage();
            TimeTable = new TimeTablePage();
            InitializeComponent();
            this.Title = "Клиенты";
            WindowState = WindowState.Maximized;
        }

        private void ClientBox_Click(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Navigate(Clientt);
        }
        
        private void TimeTableBox_Click(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Navigate(TimeTable);
        }
    }
}
