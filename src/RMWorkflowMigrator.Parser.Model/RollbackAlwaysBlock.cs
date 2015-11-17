// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RollbackAlwaysBlock.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the RollbackAlwaysBlock type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Parser.Model
{
    using System.Diagnostics;

    [DebuggerDisplay("{ItemType}_{DisplayName}_{Sequence}")]
    public class RollbackAlwaysBlock : RollbackBlock
    {
    }
}