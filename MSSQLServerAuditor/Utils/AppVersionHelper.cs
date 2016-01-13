// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppVersionHelper.cs" company="">
//
// </copyright>
// <summary>
//   The app version.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MSSQLServerAuditor.Utils
{
    using MSSQLServerAuditor.Entities;

    /// <summary>
    /// The app version helper.
    /// </summary>
    public static class AppVersionHelper
    {
        /// <summary>
        /// Initializes static members of the <see cref="AppVersionHelper"/> class.
        /// </summary>
        static AppVersionHelper()
        {
#if DEBUG
            CurrentAppVersionType = AppVersionType.Debug;
#endif

#if RELEASE
            CurrentAppVersionType = AppVersionType.Release;
#endif

#if TRIAL
            CurrentAppVersionType = AppVersionType.Trial;
#endif
        }

        /// <summary>
        /// Gets the current app version.
        /// </summary>
        public static AppVersionType CurrentAppVersionType { get; private set; }

        /// <summary>
        /// The is debug.
        /// </summary>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        public static bool IsDebug()
        {
            return CurrentAppVersionType == AppVersionType.Debug;
        }

        /// <summary>
        /// The is not debug.
        /// </summary>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        public static bool IsNotDebug()
        {
            return CurrentAppVersionType != AppVersionType.Debug;
        }

        /// <summary>
        /// The is trial.
        /// </summary>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        public static bool IsTrial()
        {
            return CurrentAppVersionType == AppVersionType.Trial;
        }

        /// <summary>
        /// The is not trial.
        /// </summary>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        public static bool IsNotTrial()
        {
            return CurrentAppVersionType != AppVersionType.Trial;
        }

        /// <summary>
        /// The is release.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsRelease()
        {
            return CurrentAppVersionType == AppVersionType.Release;
        }

        /// <summary>
        /// The is not release.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsNotRelease()
        {
            return CurrentAppVersionType != AppVersionType.Release;
        }
    }
}