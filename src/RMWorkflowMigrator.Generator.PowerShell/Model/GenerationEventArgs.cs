// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenerationEventArgs.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the GenerationEventArgs type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell.Model
{
    using System;

    using Microsoft.ALMRangers.RMWorkflowMigrator.Parser.Model;

    public class GenerationEventArgs : EventArgs
    {
        public BlockType BlockType { get; set; }

        public string DisplayName { get; set; }

        public int Sequence { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsContainer { get; set; }

        public GenerationEventType GenerationEventType { get; set; }
    }
}
