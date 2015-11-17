// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkflowBlockBase.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the WorkflowBlockBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Parser.Model
{
    using System;

    public abstract class WorkflowBlockBase : IReleaseWorkflowBlock
    {
        public int Sequence { get; set; }

        public string DisplayName { get; set; }

        public bool DisplayNameIsMeaningful { get; set; }

        public BlockType ItemType { get; set; }

        public Guid WorkflowActivityId { get; set; }
    }
}