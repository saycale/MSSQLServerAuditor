using System;
using System.Collections.Generic;

namespace MSSQLServerAuditor.Model
{
    /// <summary>
    /// The exception to the license.
    /// </summary>
    public class LicenseStateException: Exception
    {
        /// <summary>
        /// State of the license.
        /// </summary>
        public LicenseState LicenseState { get; private set; }

        /// <summary>
        /// Initializes the object LicenseStateException.
        /// </summary>
        /// <param name="state">State of the license.</param>
        public LicenseStateException(LicenseState state) : base(state.ProblemsText)
        {

        }
    }

    /// <summary>
    /// State of the license.
    /// </summary>
    [Serializable]
    public class LicenseState
    {
        // this class is binnary serialized when saving .msd files so we need to store descrptions separately for compatibility
        [NonSerializedAttribute]
        private readonly List<LicenseProblem> _problems2 = new List<LicenseProblem>();

        List<LicenseProblemType> _problems = new List<LicenseProblemType>();
        private string _instance;

        /// <summary>
        /// List of problems type.
        /// </summary>
        public List<LicenseProblemType> Problems
        {
            get { return _problems; }
            set { _problems = value; }
        }

        /// <summary>
        /// Add problems in list
        /// </summary>
        /// <param name="source">State of the ptoblem.</param>
        public void AddProblems(LicenseState source)
        {
            this._problems.AddRange(source._problems);
            this._problems2.AddRange(source._problems2);
        }

        /// <summary>
        /// Field of instance.
        /// </summary>
        public string Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        /// <summary>
        /// Add problems in list.
        /// </summary>
        /// <param name="problemType">Type of problem.</param>
        /// <param name="description">Description of the problem.</param>
        public void AddProblem(LicenseProblemType problemType, string description)
        {
            _problems.Add(problemType);
            _problems2.Add( new LicenseProblem(problemType, description));
        }

        /// <summary>
        /// Text problems.
        /// </summary>
        public string ProblemsText
        {
            get
            {
                var result = "Instance: "+_instance+Environment.NewLine;

                foreach (var problem in _problems2)
                {
                    result = result + problem.ToLocalizedSting() + Environment.NewLine;
                }

                return result;
            }
        }

        /// <summary>
        /// Correct of problems
        /// </summary>
        public bool IsCorrect
        {
            get { return _problems.Count == 0; }
        }

        /// <summary>
        /// Exception if not correct of problem.
        /// </summary>
        public void ThrowIfNotCorrect()
        {
#if !DEBUG
            if (!IsCorrect)
                throw new LicenseStateException(this);
#endif
        }
    }
}