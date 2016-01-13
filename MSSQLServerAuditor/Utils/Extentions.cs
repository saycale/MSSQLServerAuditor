using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MSSQLServerAuditor.Utils
{
    public static class Extentions
    {
        public static IEnumerable<TreeNode> AsEnumerable(this TreeNodeCollection nodes, bool hierarchically = false)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                yield return nodes[i];

                if (hierarchically)
                    foreach (var n in nodes[i].Nodes.AsEnumerable(true))
                    {
                        yield return n;
                    }
            }
        }

        public static void EnableControls(bool value, params Control[] controls)
        {
            foreach (var c in controls)
            {
                c.Enabled = value;
            }
        }

        public static DateTime FitToBouds(this DateTime value, DateTime min, DateTime max)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }
    }
}