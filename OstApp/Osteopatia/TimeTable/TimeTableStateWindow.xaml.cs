using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OstLib;

namespace Osteopatia.TimeTable
{
    public partial class TimeTableStateWindow : Window
    {
        private TimeTablePage _tablePage;
        public TimeTableStateWindow(TimeTablePage ttp)
        {
            InitializeComponent();
            _tablePage = ttp;
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
            TimeTableList.ItemsSource = TimeTableEntry.GetTimeTablesBySurname(s.Surname);
        }
        private void RemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < TimeTableList.SelectedItems.Count; i++)
            {
                var tt = (TimeTableEntry) TimeTableList.SelectedItems[i];
                TimeTableEntry.Remove(tt);
            }
            _tablePage.FillingData();
            Close();
        }
    }
}

