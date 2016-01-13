using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

using MSSQLServerAuditor.Properties;

namespace MSSQLServerAuditor.Model
{
    using Resources = MSSQLServerAuditor.Properties.Resources;

    /// <summary>
    /// Type of problems with the license.
    /// </summary>
    [Serializable]
    public enum LicenseProblemType
    {
        /// <summary>
        /// Type problem: cant connect.
        /// </summary>
        [EnumMember] CantConnect,
        /// <summary>
        /// Type problem: expired of end.
        /// </summary>
        [EnumMember] Expired,
        /// <summary>
        /// Type problem: wrong host.
        /// </summary>
        [EnumMember] WrongHost,
        /// <summary>
        /// Type problem: wrong user.
        /// </summary>
        [EnumMember] WrongUser,
        /// <summary>
        /// Type problem: license not defined.
        /// </summary>
        [EnumMember] LicenseNotDefined,
        /// <summary>
        /// Type problem: wrong signature.
        /// </summary>
        [EnumMember] WrongSignature,
        /// <summary>
        /// Build Expiry Date invalid.
        /// </summary>
        [EnumMember]
        BuildExpiryDateNotValid
    }

    /// <summary>
    /// Problems with the license.
    /// </summary>
    [Serializable]
    public struct LicenseProblem
    {
        /// <summary>
        /// Type of problem.
        /// </summary>
        public readonly LicenseProblemType ProblemType;
        /// <summary>
        /// Description of the problem.
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// Initializes the object LicenseProblem.
        /// </summary>
        /// <param name="problemType">Type of problem.</param>
        /// <param name="description">Description of the problem.</param>
        public LicenseProblem(LicenseProblemType problemType, string description)
        {
            ProblemType = problemType;
            Description = description;
        }

        /// <summary>
        /// To localized string licenseProblem.
        /// </summary>
        /// <returns>Description of the ptoblem.</returns>
        public string ToLocalizedSting()
        {
			string licenseProblemString;

			switch (ProblemType)
			{
				case LicenseProblemType.CantConnect:
					licenseProblemString = GetProblemText("CantConnect");;
					break;

				case LicenseProblemType.Expired:
					licenseProblemString = GetProblemText("Expired");
					break;

				case LicenseProblemType.LicenseNotDefined:
					licenseProblemString = GetProblemText("LicenseNotDefined");
					break;

				case LicenseProblemType.WrongHost:
					licenseProblemString = GetProblemText("WrongHost");
					break;

				case LicenseProblemType.WrongSignature:
					licenseProblemString = GetProblemText("WrongSignature");
					break;

				case LicenseProblemType.WrongUser:
					licenseProblemString = GetProblemText("WrongUser");
					break;

                case LicenseProblemType.BuildExpiryDateNotValid:
                    licenseProblemString = GetProblemText("ExpiredBuildDate");
                    break;

					default: throw new Exception(Resources.NotExecutedCodeError);
			}


			if (string.IsNullOrWhiteSpace(Description))
			{
				licenseProblemString += ": " + Description;
			}

            return licenseProblemString;
        }

        string GetProblemText(string problemName)
        {
        	return Program.Model.LocaleManager.GetLocalizedText("licenseProblems", problemName);
        }
    }
}