using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Beacon.Client.WPF
{
    internal class Startup : DefaultStartup
    {
        public override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            ConfigureCoreServices(context, services, true);
        }
    }
}
