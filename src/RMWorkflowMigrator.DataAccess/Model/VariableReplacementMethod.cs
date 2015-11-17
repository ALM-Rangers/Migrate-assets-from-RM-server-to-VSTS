// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VariableReplacementMethod.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the VariableReplacementMethod type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model
{
    public enum VariableReplacementMethod
    {
        /// <summary>
        /// Replace Variable only in command
        /// </summary>
        OnlyInCommand = 1,

        /// <summary>
        /// Replace Variable before installation
        /// </summary>
        BeforeInstallation = 2,

        /// <summary>
        /// Replace Variable after installation
        /// </summary>
        AfterInstallation = 3,

        /// <summary>
        /// Replace Variable before and after installation
        /// </summary>
        BeforeAndAfterInstallation = 4
    }
}
