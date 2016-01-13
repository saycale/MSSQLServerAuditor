using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.Model.Connections;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Utils.Network;

namespace MSSQLServerAuditor.Gui
{
	public partial class NetworkInformationConnectionDialog : LocalizableForm, IConnectionStringDialog
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public NetworkInformationConnectionDialog()
		{
			InitializeComponent();

			cmbProtocol.DataSource = Enum.GetValues(typeof(ProtocolType))
				.Cast<Enum>()
				.Select(value => new
				{
					((DescriptionAttribute) Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()),
						typeof(DescriptionAttribute))).Description,
					value
				})
				.OrderBy(item => item.value)
				.ToList();

			cmbProtocol.DisplayMember            = "Description";
			cmbProtocol.ValueMember              = "Value";
			cmbProtocol.SelectionChangeCommitted += ProtocolSelectionChangeCommitted;
			cmbProtocol.SelectedIndex            = 0;

			UpdateControlsState();
		}

		private void UpdateControlsState()
		{
			bool valid =
				this.cmbProtocol.SelectedItem != null &&
				!string.IsNullOrWhiteSpace(this.txtHost.Text);

			this.btnOk.Enabled = valid;

			ProtocolType protocol = (ProtocolType) cmbProtocol.SelectedValue;

			switch (protocol)
			{
				case ProtocolType.Icmp:
					nupPort.Enabled = false;
					break;
				default:
					nupPort.Enabled = true;
					break;
			}
		}

		private void ProtocolSelectionChangeCommitted(object sender, EventArgs eventArgs)
		{
			ProtocolType protocol = (ProtocolType)cmbProtocol.SelectedValue;

			switch (protocol)
			{
				case ProtocolType.Icmp:
					nupTimeout.Value = HostPinger.DefaultTimeout;
					break;
				case ProtocolType.Tcp:
					nupTimeout.Value = TcpPinger.DefaultTimeout;
					break;
				case ProtocolType.Udp:
					nupTimeout.Value = UdpPinger.DefaultTimeout;
					break;
			}

			UpdateControlsState();
		}

		public string ConnectionString
		{
			get
			{
				NetworkInformationConnection connection = new NetworkInformationConnection(
					txtHost.Text,
					Convert.ToInt32(nupPort.Value),
					(ProtocolType) cmbProtocol.SelectedValue,
					Convert.ToInt32(nupTimeout.Value)
				);

				return connection.ConnectionString;
			}
		}

		public string ConnectionName
		{
			get { return this.ConnectionString; }
		}

		public bool IsOdbc
		{
			get { return false; }
		}

		private void nupPort_ValueChanged(object sender, EventArgs e)
		{
			UpdateControlsState();
		}

		private void txtHost_TextChanged(object sender, EventArgs e)
		{
			UpdateControlsState();
		}

		private void nupTimeout_ValueChanged(object sender, EventArgs e)
		{
			UpdateControlsState();
		}
	}
}
