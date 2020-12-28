using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Hosting;

namespace Beacon.Client.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ClientHost _clientHost;

        public App()
        {
            var commands = Environment.GetCommandLineArgs();
            _clientHost = new ClientHost();
            _clientHost.BuildHost<Startup>(commands);
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await _clientHost.GenericHost.StartAsync();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            await _clientHost.GenericHost.StopAsync();
            _clientHost.Dispose();
        }
    }
}
