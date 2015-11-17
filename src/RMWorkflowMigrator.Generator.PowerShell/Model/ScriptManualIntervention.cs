// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptManualIntervention.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the ScriptManualIntervention type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell.Model
{
    public class ScriptManualIntervention
    {
        public string DisplayName { get; set; }

        public bool Enabled { get; set; }

        public int Sequence { get; set; }

        public string InterventionText { get; set; }

        public string Target { get; set; }

        public bool IsTargetGroup { get; set; }
    }
}
