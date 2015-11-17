// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManualIntervention.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the ManualIntervention type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Parser.Model
{
    public class ManualIntervention : ReleaseAction, IReleaseWorkflowBlock
    {
        public string Text { get; set; }

        public bool IsGroup { get; set; }

        public int UserId { get; set; }
    }
}
