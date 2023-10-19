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
using HttpClient.pages;
using OstLib;

namespace HttpClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Page TimeTable;
        public MainWindow()
        {
            TimeTable = new TimeTablePage();
            InitializeComponent();
            this.Title = "Клиенты";
            WindowState = WindowState.Maximized;
        }

        private void TimeTableBox_Click(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Navigate(TimeTable);
        }
    }
}
