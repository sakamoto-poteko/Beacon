using System;
using Microsoft.Extensions.Hosting;

namespace Beacon.Client.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientHost host = new ClientHost();
            host.BuildHost(args);

            host.GenericHost.Run();
        }
    }
}
