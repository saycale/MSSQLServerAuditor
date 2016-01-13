using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using log4net;
using MSSQLServerAuditor.Utils;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.SQLite.Tables;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace MSSQLServerAuditorServiceTestApp
{
	class Program
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static MSSQLServerAuditorWrapper _mssqlServerAuditor;
        private static Timer _serviceUpdateDataTimer = null;
        private static Timer _serviceRunJobsTimer = null;
        private static Timer _modelInitializer = null;
        private static bool timers = false;

		static void Main(string[] args)
		{
			GlobalContext.Properties["LogFileName"] = CurrentAssembly.ProcessNameBase;
			log4net.Config.XmlConfigurator.Configure();


            _modelInitializer = new Timer();
            _modelInitializer.Interval = 500;
            _modelInitializer.Elapsed += new System.Timers.ElapsedEventHandler(ModelInintializerElapsed);
            _modelInitializer.Enabled = true;
            log.DebugFormat(
                    @"Initialize storage..."
                );

			Console.ReadKey();
		}

        private static void ModelInintializerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_mssqlServerAuditor == null)
            {
                _mssqlServerAuditor = MSSQLServerAuditorWrapper.Instance;
            }

            if (_mssqlServerAuditor.IsInitialized())
            {
                EnableUpdateTimers();
            }
        }


        /// <summary>
        /// Set update timers
        /// </summary>
        private static void EnableUpdateTimers()
        {
            if (!timers)
            {
                timers = true;
                _modelInitializer.Enabled = false;

                _serviceUpdateDataTimer = new Timer();
                _serviceUpdateDataTimer.Interval = TimeSpan.FromSeconds(_mssqlServerAuditor.ServiceDataUpdateTimeout).TotalMilliseconds;
                _serviceUpdateDataTimer.Elapsed += new System.Timers.ElapsedEventHandler(ServiceUpdateDataTimerCallback);
                _serviceUpdateDataTimer.Enabled = true;

                _serviceRunJobsTimer = new Timer();
                _serviceRunJobsTimer.Interval = TimeSpan.FromSeconds(_mssqlServerAuditor.ServiceRunJobsTimeout).TotalMilliseconds;
                _serviceRunJobsTimer.Elapsed += new System.Timers.ElapsedEventHandler(ServiceRunJobsTimerCallback);
                _serviceRunJobsTimer.Enabled = true;
                log.DebugFormat(
                    @"Storage initialized, timers enabled"
                );
                if (_mssqlServerAuditor != null)
                {
                    _mssqlServerAuditor.CheckScheduledJobs();
                }
            }
        }

        /// <summary>
        /// Update schedules timer elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ServiceUpdateDataTimerCallback(object sender, ElapsedEventArgs e)
        {
            if (_mssqlServerAuditor != null)
            {
                _mssqlServerAuditor.CheckUpdates();
            }
        }

        /// <summary>
        /// Run jobs timer elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ServiceRunJobsTimerCallback(object sender, ElapsedEventArgs e)
        {
            if (_mssqlServerAuditor != null)
            {
                _mssqlServerAuditor.CheckScheduledJobs();
            }
        }
	}
}
