// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationVariable.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the ConfigurationVariable type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model
{
    public class ConfigurationVariable
    {
        /// <summary>
        /// Gets or sets the original name of the RM configuration variable.
        /// </summary>
        /// <value>
        /// The name of the configuration variable as it originated from the database.
        /// </value>
        public string OriginalName { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the remapped name of the RM configuration variable.
        /// </summary>
        /// <value>
        /// The remapped name of the configuration variable, after it has been disambiguated and made unique
        /// </value>
        public string RemappedName { get; set; }

        public string Value { get; set; } = string.Empty;

        public bool IsParameter { get; set; }

        public bool Encrypted { get; set; }
    }
}
