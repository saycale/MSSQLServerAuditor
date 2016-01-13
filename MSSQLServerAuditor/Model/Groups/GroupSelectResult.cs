using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace MSSQLServerAuditor.Model.Groups
{
    public struct GroupSelectResult
    {
        public GroupSelectResult(DataTable[] tables) : this()
        {
            Tables = tables;
        }

        public DataTable[] Tables { get; private set; }
    }
}