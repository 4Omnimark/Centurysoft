using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.IO;
using OmnimarkAmazon.Models;


namespace OmnimarkAmazon.Service
{
    public partial class Service : ServiceBase
    {
        Timer Timer;
        bool IsConsole;

        public Service(bool IsConsole)
        {
            this.IsConsole = IsConsole;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Startup();
        }

        public void Startup()
        {
            Timer = new Timer(DoCheck, null, 0, 60000);
        }

        protected override void OnStop()
        {
            Shutdown();
        }

        public void Shutdown()
        {
            Timer.Dispose();
        }

        public void DoCheck(Object state)
        {
            ServiceController sc = new ServiceController("OmnimarkAmazonService");

            ServiceControllerStatus Status;
            
            try
            {
                Status = sc.Status;

                if (Status == System.ServiceProcess.ServiceControllerStatus.Stopped)
                {
                    sc.Start();
                }
            }
            catch (Exception Ex)
            {
                string cs = "OmnimarkAmazonServiceMonitorService";
                EventLog elog = new EventLog();
                if (!EventLog.SourceExists(cs))
                {
                    EventLog.CreateEventSource(cs, "Application");
                }
                elog.Source = cs;
                elog.EnableRaisingEvents = true;

                string Msg = "";

                while (Ex != null)
                {
                    Msg += Ex.Message + "\n" + Ex.StackTrace + "\n\n";
                    Ex = Ex.InnerException;
                }

                elog.WriteEntry(Msg);
            }


        }
    }

}
