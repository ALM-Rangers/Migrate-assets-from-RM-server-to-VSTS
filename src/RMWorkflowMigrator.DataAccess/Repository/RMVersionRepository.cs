// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RMVersionRepository.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the RMVersionRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Repository
{
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Interfaces;

    public class RMVersionRepository : IRMVersionRepository
    {
        private readonly string connectionString;

        public RMVersionRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<string> GetRMVersion()
        {
            string version;
            const string VersionQuery2013 = @"Select v.Number From dbo.Version AS v";
            const string VersionQuery2015 = @"Select v.Number From RM.tbl_Version AS v";

            try
            {
                // This might fail, we don't want to require access to the master database to query table schema
                version = await this.CheckVersion(VersionQuery2015);
            }
            catch (SqlException e) when (e.Number == 208)
            {
                // if this call fails, we want the exception to bubble up normally since it's a legitimate error
                version = await this.CheckVersion(VersionQuery2013);
            }

            return version;
        }

        private async Task<string> CheckVersion(string versionSql)
        {
            using (var sqlConnection = new SqlConnection { ConnectionString = this.connectionString })                
            using (var sqlCommand = new SqlCommand(versionSql, sqlConnection))
            {
                sqlConnection.Open();
                return await sqlCommand.ExecuteScalarAsync() as string;
            }
        }
    }
}