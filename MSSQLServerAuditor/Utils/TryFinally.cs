using System;
using System.Diagnostics;
using log4net;

namespace MSSQLServerAuditor.Utils
{
    public class TryFinally : IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string _startLocation;
        private bool            _disposed;
        private readonly Action _doAfter;

        public static IDisposable Nothing()
        {
            return new TryFinally(null, null);
        }

        public TryFinally(Action doBefore, Action doAfter)
        {
            _disposed = false;

#if DEBUG
            _startLocation = new StackTrace(1).ToString();
#else
            _startLocation = ""; // to prevent compile-time warning
#endif

            if (doBefore != null)
            {
                try
                {
                    doBefore();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            _doAfter = doAfter;
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
            _disposed = true;

            if (_doAfter != null)
            {
                try
                {
                    _doAfter();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }
        #endregion

        ~TryFinally()
        {
            if(!_disposed)
            {
                try
                {
                    log.Error("MSSQLServerAuditor.Utils.TryFinally:Dispose method was not called");

                    string strMessage = String.Format(
                        "For {0} (doAfter : {1}) Dispose method was not called. Source: {3}",
                        this,
                        _doAfter,
                        _startLocation
                    );

                    log.Error(strMessage);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }
    }
}
