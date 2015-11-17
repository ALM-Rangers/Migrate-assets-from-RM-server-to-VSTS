// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRMVersionRepository.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the IRMVersionRepository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Interfaces
{
    using System.Threading.Tasks;

    public interface IRMVersionRepository
    {
        Task<string> GetRMVersion();
    }
}