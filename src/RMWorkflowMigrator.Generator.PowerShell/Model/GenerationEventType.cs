// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenerationEventType.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the GenerationEventType type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell.Model
{
    public enum GenerationEventType
    {
        /// <summary>
        /// Container start event type
        /// </summary>
        ContainerStart,

        /// <summary>
        /// Container end event type
        /// </summary>
        ContainerEnd,

        /// <summary>
        /// Action start event type
        /// </summary>
        ActionStart,

        /// <summary>
        /// Action end event type
        /// </summary>
        ActionEnd,

        /// <summary>
        /// Warning event
        /// </summary>
        Warning,

        /// <summary>
        /// Error event
        /// </summary>
        Error
    }
}