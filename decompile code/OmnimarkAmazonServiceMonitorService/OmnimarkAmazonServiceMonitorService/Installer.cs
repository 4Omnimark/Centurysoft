using System;
using System.Collections;
using System.ServiceProcess;
using System.ComponentModel;
using System.Configuration.Install;

namespace OmnimarkAmazon.Service
{

    [RunInstaller(true)]
    public sealed class MyServiceInstallerProcess : ServiceProcessInstaller
    {
        public MyServiceInstallerProcess()
        {
            this.Account = ServiceAccount.NetworkService;
        }
    }

    [RunInstaller(true)]
    public sealed class Installer : ServiceInstaller
    {

        public Installer()
        {
            this.Description = "Monitors the OmnimarkAmazonService and Starts it if stopped.";
            this.DisplayName = "Omnimark Amazon Service Monitor Service";
            this.ServiceName = "OmnimarkAmazonServiceMonitorService";
            this.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
        }

        public static void Install(bool undo, string[] args)
        {
            try
            {
                Console.WriteLine(undo ? "uninstalling" : "installing");
                using (AssemblyInstaller inst = new AssemblyInstaller(typeof(Startup).Assembly, args))
                {
                    IDictionary state = new Hashtable();
                    inst.UseNewContext = true;
                    try
                    {
                        if (undo)
                        {
                            inst.Uninstall(state);
                        }
                        else
                        {
                            inst.Install(state);
                            inst.Commit(state);
                        }
                    }
                    catch
                    {
                        try
                        {
                            inst.Rollback(state);
                        }
                        catch { }
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}