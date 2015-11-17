// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeployerToolResource.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the DeployerToolResource type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model
{
    public class DeployerToolResource
    {
        public string FileName { get; set; } = string.Empty;

        public byte[] BinaryData { get; set; }
    }
}