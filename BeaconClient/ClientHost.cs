using System;
using Microsoft.Extensions.Hosting;

namespace Beacon.Client
{
    public class ClientHost
    {
        public IHost GenericHost { get; private set; }

        public void BuildHost<T>(string[] args, Action<IHostBuilder> configureHost = null) where T : DefaultStartup
        {
            if (GenericHost != null)
            {
                throw new InvalidOperationException("The host is already configured and built");
            }

            var builder = Host.CreateDefaultBuilder(args);

            configureHost?.Invoke(builder);

            var startup = (T)Activator.CreateInstance(typeof(T));
            builder.ConfigureServices(startup.ConfigureServices);

            startup.ConfigureHostBuilder(builder);

            GenericHost = builder.Build();
        }

        public void BuildHost(string[] args, Action<IHostBuilder> configureHost = null)
        {
            BuildHost<DefaultStartup>(args, configureHost);
        }
    }
}
