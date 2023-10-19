using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using OstLib;

namespace HttpClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string url { get; private set; } = null!;
        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName);
            builder.AddJsonFile("appSettings.json");
            var config = builder.Build();
            url = config.GetConnectionString("Server")!;
            base.OnStartup(e);
        }
    }
}
