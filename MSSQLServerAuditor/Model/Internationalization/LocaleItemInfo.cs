namespace MSSQLServerAuditor.Model.Internationalization
{
    /// <summary>
    /// Localization item
    /// </summary>
    internal struct LocaleItemInfo
    {
        private readonly string _language;
        private readonly string _value;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="language">Language</param>
        /// <param name="value">Text</param>
        public LocaleItemInfo(string language, string value)
        {
            _language = language;
            _value = value;
        }

        /// <summary>
        /// Language
        /// </summary>
        public string Language
        {
            get { return _language; }
        }

        /// <summary>
        /// Text
        /// </summary>
        public string Value
        {
            get { return _value; }
        }
    }
}