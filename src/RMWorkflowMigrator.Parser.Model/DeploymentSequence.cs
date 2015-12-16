// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeploymentSequence.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the DeploymentSequence type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Parser.Model
{
    using System.Collections.Generic;

    public class DeploymentSequence
    {
        public string ReleaseTemplateName { get; set; }
        public string ReleaseTemplateStageName { get; set; }
        public int ReleaseTemplateId { get; set; }

        public int ReleaseTemplateStageId { get; set; }

        public string DisplayName { get; set; }

        public IEnumerable<IReleaseActionContainer<IReleaseWorkflowBlock>> Containers { get; set; }
    }
}