using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Flurl.Http;
using HttpClient.pages;

namespace HttpClient.windows
{
    public partial class TimeTableStateWindow : Window
    {
        private TimeTablePage _tablePage;
        private string _connection = App.url;
        public TimeTableStateWindow(TimeTablePage ttp)
        {
            InitializeComponent();
            _tablePage = ttp;
        }
        
        public IEnumerable<Client> GetClientsBySurname(SurnameModel surnameModel)
        {
            var res = $"{_connection}Client/getBySurname".PostJsonAsync(surnameModel).Result;
            var list = res.GetJsonAsync<IEnumerable<Client>>().Result;
            return list;
        }
        
        private void SurnameBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var list = GetClientsBySurname(new SurnameModel(SurnameBox.Text));
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

