// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeComponentRepo.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Tests.Unit.TestHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Interfaces;
    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model;

    [ExcludeFromCodeCoverage]
    internal class FakeComponentRepo : IRMComponentRepository
    {
        public FakeComponentRepo()
        {
            this.Components = new List<RMComponent>();
        }

        public List<RMComponent> Components { get; set; }

        public Task<RMComponent> GetComponentByIdAsync(Guid workflowActivityId, int stageId)
        {
            return Task.FromResult(this.Components.First(c => c.WorkflowActivityId == workflowActivityId));
        }

        public Task<IEnumerable<ConfigurationVariable>> GetComponentConfigurationVariablesAsync(
            Guid workflowGuid, 
            int stageId)
        {
            throw new NotImplementedException();
        }
    }
}