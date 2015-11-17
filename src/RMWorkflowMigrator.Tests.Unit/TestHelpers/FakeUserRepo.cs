// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeUserRepo.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Tests.Unit.TestHelpers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Interfaces;
    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model;

    [ExcludeFromCodeCoverage]
    public class FakeUserRepo : IRMUserRepository
    {
        public HashSet<RMGroup> Groups { get; set; }

        public HashSet<RMUser> Users { get; set; }

        public Task<RMGroup> GetGroupAsync(int groupId)
        {
            return Task.FromResult(this.Groups.First(u => u.Id == groupId));
        }

        public Task<RMUser> GetUserAsync(int userId)
        {
            return Task.FromResult(this.Users.First(u => u.Id == userId));
        }
    }
}