using System;
using System.Threading;
using System.Windows.Forms;

namespace MSSQLServerAuditor.Model
{
    public static class MainThread
    {
        private static Thread _object;

        public static Thread Get()
        {
            if (_object == null && Application.OpenForms.Count > 0)
            {
                Action a = () => { _object = Thread.CurrentThread; };
                Application.OpenForms[0].Invoke(a);
            }

            return _object ?? Thread.CurrentThread;
        }

        public static bool ThisIsIt
        {
            get { return Thread.CurrentThread == Get(); }
        }
    }
}