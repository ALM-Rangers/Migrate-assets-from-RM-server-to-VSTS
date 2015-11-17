// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RMComponent.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the RMDeploymentSequence type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model
{
    using System;
    using System.Collections.Generic;

    public class RMComponent
    {
        public int Id { get; set; }

        public int DeployerToolId { get; set; }

        public Guid WorkflowActivityId { get; set; }

        public string FileExtensionFilter { get; set; } = string.Empty;

        public string Command { get; set; } = string.Empty;

        public string Arguments { get; set; } = string.Empty;

        public VariableReplacementMethod VariableReplacementMethod { get; set; }

        public IEnumerable<ConfigurationVariable> ConfigurationVariables { get; set; } = new List<ConfigurationVariable>();
    }
}
