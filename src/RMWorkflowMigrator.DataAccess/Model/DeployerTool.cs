// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeployerTool.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the DeployerTool type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model
{
    using System.Collections.Generic;

    public class DeployerTool
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public IEnumerable<DeployerToolResource> Resources { get; set; }
    }
}
