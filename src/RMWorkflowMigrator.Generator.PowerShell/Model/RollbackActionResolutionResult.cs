// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RollbackActionResolutionResult.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell.Model
{
    using System.Collections.Generic;

    public class RollbackActionResolutionResult
    {
        public IEnumerable<ScriptAction> Actions { get; set; }
        public IEnumerable<ScriptManualIntervention> ManualInterventions { get; set; }
    }
}
