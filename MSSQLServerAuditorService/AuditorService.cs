using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using log4net;
using MSSQLServerAuditor.Utils;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace MSSQLServerAuditorService
{
    public partial class AuditorService : ServiceBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private MSSQLServerAuditorWrapper _mssqlServerAuditor;
        private Timer _serviceUpdateDataTimer=null;
        private Timer _serviceRunJobsTimer=null;
        private Timer _modelInitializer = null;
        private bool timers = false;
        
        public AuditorService()
        {
            InitializeComponent();
            GlobalContext.Properties["LogFileName"] = CurrentAssembly.ProcessNameBase;
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// On service start
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
            this._modelInitializer = new Timer();
            this._modelInitializer.Interval = 500;
            this._modelInitializer.Elapsed += new System.Timers.ElapsedEventHandler(this.ModelInintializerElapsed);
            this._modelInitializer.Enabled = true;
            log.DebugFormat(@"Model initializer enabled");
        }
        
        /// <summary>
        /// On service stop
        /// </summary>
        protected override void OnStop()
        {
            if(this._modelInitializer.Enabled==true)
            {
                this._modelInitializer.Enabled = false;
            }
            if (this._serviceRunJobsTimer.Enabled == true)
            {
                this._serviceRunJobsTimer.Enabled = false;
            }
            if (this._serviceUpdateDataTimer.Enabled == true)
            {
                this._serviceUpdateDataTimer.Enabled = false;
            }
        }

        /// <summary>
        /// Model initialize timer elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModelInintializerElapsed(object sender, ElapsedEventArgs e)
        {
            if(_mssqlServerAuditor==null)
            {
                log.DebugFormat(
                    @"Initialize storage..."
                );
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
        private void EnableUpdateTimers()
        {
            if (!timers)
            {
                timers = true;
                this._modelInitializer.Enabled = false;

                this._serviceUpdateDataTimer = new Timer();
                this._serviceUpdateDataTimer.Interval = TimeSpan.FromSeconds(_mssqlServerAuditor.ServiceDataUpdateTimeout).TotalMilliseconds;
                this._serviceUpdateDataTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.ServiceUpdateDataTimerCallback);
                this._serviceUpdateDataTimer.Enabled = true;
                
                this._serviceRunJobsTimer = new Timer();
                this._serviceRunJobsTimer.Interval = TimeSpan.FromSeconds(_mssqlServerAuditor.ServiceRunJobsTimeout).TotalMilliseconds;
                this._serviceRunJobsTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.ServiceRunJobsTimerCallback);
                this._serviceRunJobsTimer.Enabled = true;
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
        private void ServiceUpdateDataTimerCallback(object sender, ElapsedEventArgs e)
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
        private void ServiceRunJobsTimerCallback(object sender, ElapsedEventArgs e)
        {
            if (_mssqlServerAuditor != null)
            {
                _mssqlServerAuditor.CheckScheduledJobs();
            }
        }
    }
}
