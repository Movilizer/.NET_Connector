using System;
using System.ServiceProcess;
using System.Timers;

using MWS.Data;
using MWS.Log;

namespace MWS.WindowsService
{
    public partial class MovilizerWindowsService : ServiceBase
    {
        // common usage vars
        private Manager _manager;

        public MovilizerWindowsService(Manager manager)
        {
            _manager = manager;

            InitializeComponent();

            serviceTimer.Elapsed += ServiceTimerElapsed;
        }

        protected override void OnStart(string[] args)
        {
            serviceTimer.Interval = 5000; // 5 seconds to go
            serviceTimer.Enabled = true;
            serviceTimer.Start();

            LogFactory.WriteInfo("Movilizer Windows Service (MWS) started");
        }

        protected void ServiceTimerElapsed(object sender, ElapsedEventArgs args)
        {
            //LogFactory.WebServiceLog.WriteInfo("Movilizer Windows Service (MWS) working...");

            try
            {
                _manager.RunServiceCycle();
            }
            catch(Exception e)
            {
                LogFactory.WriteError(e.ToString());
            }

            // restart timer
            serviceTimer.Interval = Configuration.GetServiceTimeInterval();
            serviceTimer.Start();
        }

        protected override void OnStop()
        {
            serviceTimer.Stop();
            serviceTimer.Enabled = false;

            LogFactory.WriteInfo("Movilizer Windows Service (MWS) stopped");
        }
    }
}
