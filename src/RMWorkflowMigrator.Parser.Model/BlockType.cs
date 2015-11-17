// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockType.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the BlockType type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Parser.Model
{
    public enum BlockType
    {
        /// <summary>
        /// Type is undefined
        /// </summary>
        Undefined,

        /// <summary>
        /// Action
        /// </summary>
        Action,

        /// <summary>
        /// Component
        /// </summary>
        Component,

        /// <summary>
        /// Manual intervention
        /// </summary>
        ManualIntervention,

        /// <summary>
        /// Parallel block
        /// </summary>
        Parallel,

        /// <summary>
        /// Sequence
        /// </summary>
        Sequence,

        /// <summary>
        /// server
        /// </summary>
        Server,
        
        /// <summary>
        /// ServerTag
        /// </summary>
        ServerTag,

        /// <summary>
        /// Rollback
        /// </summary>
        Rollback,

        /// <summary>
        /// Rollback Always
        /// </summary>
        RollbackAlways
    }
}
