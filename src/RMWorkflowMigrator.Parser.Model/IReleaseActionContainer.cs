// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReleaseActionContainer.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the IReleaseActionContainer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Parser.Model
{
    using System.Collections.Generic;

    public interface IReleaseActionContainer<out T> : IReleaseWorkflowBlock
    {
        string ValidFileName { get; }

        IEnumerable<T> SubItems { get; }
    }
}