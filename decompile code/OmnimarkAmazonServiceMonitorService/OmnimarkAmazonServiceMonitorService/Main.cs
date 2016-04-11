using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace OmnimarkAmazon.Service
{
    class Startup
    {
        static int Main(string[] args)
        {
            bool install = false, uninstall = false, console = false, rethrow = false;
            
            try
            {
                foreach (string arg in args)
                {
                    switch (arg)
                    {
                        case "-i":
                        case "-install":
                            install = true; break;
                        case "-u":
                        case "-uninstall":
                            uninstall = true; break;
                        case "-c":
                        case "-console":
                            console = true; break;
                        default:
                            Console.Error.WriteLine("Argument not expected: " + arg);
                            break;
                    }
                }

                if (uninstall)
                {
                    Installer.Install(true, args);
                }
                if (install)
                {
                    Installer.Install(false, args);
                }
                if (console)
                {
                    Service s = new Service(true);
                    Console.WriteLine("Starting...");
                    s.Startup();
                    Console.WriteLine("System running; press any key to stop");
                    Console.ReadKey(true);
                    s.Shutdown();
                    Console.WriteLine("System stopped");
                }
                else if (!(install || uninstall))
                {
                    rethrow = true; // so that windows sees error...
                    ServiceBase[] services = { new Service(false) };
                    ServiceBase.Run(services);
                    rethrow = false;
                }
                return 0;
            }
            catch (Exception ex)
            {
                if (rethrow) throw;
                Console.Error.WriteLine(ex.Message);
                return -1;
            }
        }
    }
}