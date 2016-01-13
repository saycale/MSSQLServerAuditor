//#define STOPWATCH
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MSSQLServerAuditor.Graph;

namespace TestApp
{
	public partial class DemoForm : Form
	{
		public DemoForm()
		{
			InitializeComponent();

			GraphConfiguration configuration = new GraphConfiguration();
			configuration.AxisXConfiguration.MajorGrid.LineColor = Color.FromArgb(200, 111, 135);
			configuration.Palette.Add(new PaletteItem(){Color = Color.Firebrick, HatchStyle = ChartHatchStyle.BackwardDiagonal});
			configuration.Palette.Add(new PaletteItem() { Color = Color.Black, HatchStyle = ChartHatchStyle.SolidDiamond });
			configuration.GraphSource = new XmlFileGraphSource(){DateTag = "aaa"};
			configuration.SaveToXml("qwe.xml");

			Stopwatch sw = new Stopwatch();
			sw.Start();
			GraphControl gc = graphControl;// new GraphControl();

			gc.SetConfiguration("graphConfig.xml", Size, null);
			sw.Stop();
#if STOPWATCH
			MessageBox.Show("Time spent for data extraction: " + sw.ElapsedMilliseconds);
#endif
			sw.Reset();
			sw.Start();
			gc.SaveImage("temp.jpg", ChartImageFormat.Jpeg);
			sw.Stop();
#if STOPWATCH
			MessageBox.Show("Time spent for data render by Chart control: " + sw.ElapsedMilliseconds);
#endif
		}
	}
}
