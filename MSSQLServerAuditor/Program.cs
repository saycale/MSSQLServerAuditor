using System;
using System.Globalization;
using System.Threading;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandLine;
using CommandLine.Text;
using log4net;
using MSSQLServerAuditor.Utils;
using MSSQLServerAuditor.Gui;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.WebServer;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace MSSQLServerAuditor
{
	static class Program
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private static frmMain           _fMain;
		private static MsSqlAuditorModel _model;
		private static WebServerManager  _webServer;

		public static MsSqlAuditorModel Model
		{
			get { return _model; }
		}

		//
		// Define a class to receive parsed values
		//
		class Options
		{
			// [Option('m', "monitor", DefaultValue = -1, Required = false, HelpText = "Monitor id (0 - the first monitor) to open the Main Window")]
			[Option('m', "monitor", Required = false, HelpText = "Monitor id (0 - the first monitor) to open the Main Window")]
			public int intMonitor { get; set; }

			// [Option('x', "posx", DefaultValue = -1, Required = false, HelpText = "Main Window X position")]
			[Option('x', "posx", Required = false, HelpText = "Main Window X position")]
			public int intPosX { get; set; }

			// [Option('y', "posy", DefaultValue = -1, Required = false, HelpText = "Main Window Y position")]
			[Option('y', "posy", Required = false, HelpText = "Main Window Y position")]
			public int intPosY { get; set; }

			// [Option('w', "width", DefaultValue = -1, Required = false, HelpText = "Main Window Width")]
			[Option('w', "width", Required = false, HelpText = "Main Window Width")]
			public int intWidth { get; set; }

			// [Option('h', "height", DefaultValue = -1, Required = false, HelpText = "Main Window Height")]
			[Option('h', "height", Required = false, HelpText = "Main Window Height")]
			public int intHeight { get; set; }

			// [Option('l', "statusline", DefaultValue = false, Required = false, HelpText = "Disable Status Line for the Main Window")]
			[Option('l', "statusline", Required = false, HelpText = "Disable Status Line for the Main Window")]
			public bool isDisableStatusLine { get; set; }

			// [Option('n', "mainmenu", DefaultValue = false, Required = false, HelpText = "Hide Main Menu for the Main Window")]
			[Option('n', "mainmenu", Required = false, HelpText = "Hide Main Menu for the Main Window")]
			public bool isDisableMainMenu { get; set; }

			// [Option('s', "server", DefaultValue = null, Required = false, HelpText = "SQL Server Name")]
			[Option('s', "server", Required = false, HelpText = "SQL Server Name")]
			public string strServerName { get; set; }

			// [Option('t', "template", DefaultValue = null, Required = false, HelpText = "Template File Name")]
			[Option('t', "template", Required = false, HelpText = "Template File Name")]
			public string strTemplate { get; set; }

			// [Option('d', "databasetype", DefaultValue = null, Required = false, HelpText = "SQL Server Type")]
			[Option('d', "databasetype", Required = false, HelpText = "SQL Server Type")]
			public string strDatabaseType { get; set; }

			// [Option('p', "navigationpanel", DefaultValue = false, Required = false, HelpText = "Disable Navigation Panel")]
			[Option('p', "navigationpanel", Required = false, HelpText = "Disable Navigation Panel")]
			public bool isDisableNavigationPanel { get; set; }

			// [ParserState]
			// public IParserState LastParserState { get; set; }

			// [HelpOption]
			// public string GetUsage()
			// {
			// 	// this without using CommandLine.Text or using HelpText.AutoBuild
			// 	var usage = new System.Text.StringBuilder();
			//
			// 	usage.AppendLine("<Program Name>");
			// 	usage.AppendLine("<Program Name>.exe -m=1 -x=100 -y=150 -w=400 -h=200 -l -n -s 'local' -t 'MyTemplateFile.xml' -d 'MSSQL'");
			//
			// 	return usage.ToString();
			// }

			public Options()
			{
				this.isDisableMainMenu        = false;
				this.isDisableNavigationPanel = false;
				this.isDisableStatusLine      = false;
				this.intHeight                = -1;
				this.intMonitor               = -1;
				this.intPosX                  = -1;
				this.intPosY                  = -1;
				this.intWidth                 = -1;
				this.strDatabaseType          = null;
				this.strServerName            = null;
				this.strTemplate              = null;
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
		static void Main(string[] args)
		{
			GlobalContext.Properties["LogFileName"] = CurrentAssembly.ProcessNameBase;
			log4net.Config.XmlConfigurator.Configure();

			log.Debug("Application is starting...");

			Options options = new Options();

			var result = Parser.Default.ParseArguments<Options>(args);

			log.Debug("Command line parameters have been processed.");

			switch (result.Tag)
			{
				case ParserResultType.Parsed:
					log.Debug("Command line options have been successfully parced.");
					break;

				case ParserResultType.NotParsed:
					log.Error("Error by processing command line options.");
					options.intPosX = -1;
					break;

				default:
					log.Error("Unknown status by processing command line options.");
					options.intPosX = -1;
					break;
			}

			if (AppVersionHelper.IsNotDebug())
			{
				AppDomain.CurrentDomain.UnhandledException += frmExceptionBox.CurrentDomainUnhandledException;
				Application.ThreadException += frmExceptionBox.ApplicationOnThreadException;
			}

			Application.EnableVisualStyles();

			Application.SetCompatibleTextRenderingDefault(false);

			TaskScheduler.UnobservedTaskException += OnUnobservedException;

			_fMain = new frmMain(
				options.intMonitor,
				options.intPosX,
				options.intPosY,
				options.intWidth,
				options.intHeight,
				options.isDisableStatusLine,
				options.isDisableMainMenu,
				options.strServerName,
				options.strTemplate,
				options.strDatabaseType,
				options.isDisableNavigationPanel
			);

			_model = new MsSqlAuditorModel();
			_fMain.SetModel(_model);

			_model.Initialize();
			_model.Settings.WarnAboutUnsignedQuery = args.All(a => a != "/!w");

			_webServer = new WebServerManager(_model);

			Thread.CurrentThread.CurrentUICulture = new CultureInfo(Model.Settings.InterfaceLanguage);

			log.Debug("Inner data model has been initialized. Main form is going to be shown...");

			Application.Run(_fMain);

			log.Debug("Application is closed...");
		}

		/// <summary>
		/// Static handler for event that occurs
		/// when faulted task's unobserved exception is about to trigger exception escalation
		/// </summary>
		private static void OnUnobservedException(
			object                           sender,
			UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs
		)
		{
			log.Error(sender, unobservedTaskExceptionEventArgs.Exception);
		}

		/// <summary>
		/// Static handler for Domain.UnhandledException event
		/// </summary>
		/// <param name="sender">Sender object</param>
		/// <param name="e">Event args</param>
		static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e != null && e.ExceptionObject != null && e.ExceptionObject is Exception)
			{
				log.Error(sender, e.ExceptionObject as Exception);

				frmExceptionBox box = new frmExceptionBox(e.ExceptionObject as Exception);

				box.ShowDialog();
			}
			else
			{
				log.Error("UnhandledException");
			}
		}
	}
}
