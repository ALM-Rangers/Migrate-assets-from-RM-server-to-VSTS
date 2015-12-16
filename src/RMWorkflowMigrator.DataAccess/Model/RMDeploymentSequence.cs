// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RMDeploymentSequence.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the RMDeploymentSequence type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model
{
    public class RMDeploymentSequence
    {
        public int StageId { get; set; }
        public string ReleaseTemplateName { get; set; }
        public string ReleaseTemplateStageName { get; set; }
        public string WorkflowXaml { get; set; }
    }
}
