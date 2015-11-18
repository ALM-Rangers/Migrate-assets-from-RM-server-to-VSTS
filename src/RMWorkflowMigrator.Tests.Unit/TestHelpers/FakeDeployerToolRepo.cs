// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeDeployerToolRepo.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Tests.Unit.TestHelpers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Interfaces;

    [ExcludeFromCodeCoverage]
    public class FakeDeployerToolRepo : IRMDeployerToolRepository
    {
        public Task WriteToolToDiskAsync(int deployerToolId, string diskPath)
        {
            return Task.FromResult(0);
        }
    }
}