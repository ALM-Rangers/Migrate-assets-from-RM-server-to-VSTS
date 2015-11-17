// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptGeneratorException.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the ScriptGeneratorException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell.Model
{
    using System;
    using System.Collections.Generic;
    using System.Management.Automation;

    [Serializable]
    public class ScriptGeneratorException : Exception
    {
        public ScriptGeneratorException(string message, IDictionary<PSToken, PSParseError> scriptErrors) : base(message)
        {
            this.ScriptErrors = scriptErrors;
        }

        private IDictionary<PSToken, PSParseError> ScriptErrors { get; set; }
    }
}
