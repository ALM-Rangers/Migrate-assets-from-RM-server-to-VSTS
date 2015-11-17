// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReleaseAction.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the ReleaseAction type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Parser.Model
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    [Serializable]
    [DebuggerDisplay("{ItemType}_{DisplayName}_{Sequence}")]
    public class ReleaseAction : WorkflowBlockBase, IReleaseWorkflowBlock
    {
        public bool Enabled { get; set; }  = true;

        public int ComponentId { get; set; }

        public bool IsComponent => this.ItemType == BlockType.Component;

        public IEnumerable<string> RollbackScripts { get; set; }
    }
}