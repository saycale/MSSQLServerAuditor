using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MSSQLServerAuditor.Gui.LayoutSettings;

namespace MSSQLServerAuditor.Gui
{
    [XmlRoot]
    public class frmUserSettingsParametersLayout : frmConnectionSelectionSettings, IFormLayoutSettings
    {

    }
}
