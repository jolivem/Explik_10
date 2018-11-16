using Roadkill.Core.Configuration;
using Roadkill.Core.DI;
using Roadkill.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the settings from the web.config
            FullTrustConfigReaderWriter configReader = new FullTrustConfigReaderWriter("");
            ApplicationSettings applicationSettings = configReader.GetApplicationSettings();

            // Configure StructureMap dependencies
            DependencyManager iocSetup = new DependencyManager(applicationSettings);

            iocSetup.Configure();
            iocSetup.ConfigureMvc();

            // Logging
            Log.ConfigureLogging(applicationSettings);

        }
    }
}
