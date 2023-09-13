using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Serialization;
using TimeTableClient.Models;

namespace TimeTableClient;

public partial class TimeTablePage : Page
{
    public int WeekNumber = 0;
    public List<TimeTableUdpModel> ListOfTimeTable;
    private static TimeTableUdpModel _udpModel = new TimeTableUdpModel();
    private byte[] _byteData = new byte[1024];
    private Socket _handler;
    private IPEndPoint endPoint;
    private static readonly ManualResetEvent _connectDone = new ManualResetEvent(false);
    public TimeTablePage()
    {
        InitializeComponent();
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        endPoint = new IPEndPoint(ipAddress, 11001);
        Socket sClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        sClient.BeginConnect(endPoint, new AsyncCallback(ConnectCallBack), sClient);
        _connectDone.WaitOne();
        _handler = sClient;
    }
    
    public static void ConnectCallBack(IAsyncResult ar)
    {
        Socket sClient = (Socket)ar.AsyncState;
        sClient.EndConnect(ar);
        MessageBox.Show($"Соединили сокет с {sClient.RemoteEndPoint}");
        _connectDone.Set();
    }

    private void LeftButton_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void RightButton_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void defaultWeekButton_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void AddTimeTableLineButton_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void GetTimeTableButton_OnClick(object sender, RoutedEventArgs e)
    {
        int k = 42;
        _byteData = Encoding.UTF8.GetBytes(k.ToString());
        _handler.Send(_byteData);
        byte[] receiveBytes = new byte[32768];
        _handler.Receive(receiveBytes);
        byte[] newByte = Array.FindAll(receiveBytes, a => a != 0);
        XmlSerializer fileSerializer = new XmlSerializer(typeof(List<TimeTableUdpModel>));
        MemoryStream stream1 = new MemoryStream();
        stream1.Write(newByte,0,newByte.Length);
        stream1.Position = 0;
        List<TimeTableUdpModel> listOfModels = (List<TimeTableUdpModel>)fileSerializer.Deserialize(stream1);
        foreach (var item in listOfModels)
        {
            MessageBox.Show($"{item.TimeTableDateTime}\n" +
                            $"{item.ClientSurname}\n" +
                            $"{item.ClientName}\n" +
                            $"{item.ClientPhoneNumber}");   
        }
    }
}