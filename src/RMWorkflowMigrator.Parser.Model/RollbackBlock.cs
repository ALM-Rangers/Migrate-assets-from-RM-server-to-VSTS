// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RollbackBlock.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the RollbackBlock type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Parser.Model
{
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("{ItemType}_{DisplayName}_{Sequence}")]
    public class RollbackBlock : ReleaseAction, IReleaseActionContainer<IReleaseWorkflowBlock>
    {
        public RollbackBlock()
        {
            this.DisplayNameIsMeaningful = false;
        }

        public string ValidFileName => this.DisplayName;

        public IEnumerable<IReleaseWorkflowBlock> SubItems { get; set; }
    }
}