// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RMUserRepository.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the RMUserRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Repository
{
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Interfaces;
    using Model;

    public class RMUserRepository : IRMUserRepository
    {
        private readonly string connectionString;
        private readonly string tableNamePrefix;

        public RMUserRepository(string connectionString, RMVersion version)
        {
            this.connectionString = connectionString;
            this.tableNamePrefix = version == RMVersion.Rm2015 ? "RM.tbl_" : "dbo.";
        }

        /// <summary>
        /// Gets Group from repository by ID
        /// </summary>
        /// <param name="groupId">Group ID</param>
        /// <returns><see cref="Task{RMGroup}"/></returns>
        /// <exception cref="DbException">An error occurred while executing the command text.</exception>
        public async Task<RMGroup> GetGroupAsync(int groupId)
        {
            string groupSql = $@"SELECT sg.Name FROM {this.tableNamePrefix}SecurityGroup sg	
                            LEFT OUTER JOIN {this.tableNamePrefix}SecurityGroup_TfsGroup sgtg
	                            ON sgtg.SecurityGroupId = sg.id
                            LEFT JOIN  {this.tableNamePrefix}tfsgroup tg
	                            ON tg.id = sgtg.TfsGroupId
                            LEFT OUTER JOIN {this.tableNamePrefix}SecurityGroup_AdGroup sgag
	                            ON sgag.SecurityGroupId = sg.id
                            LEFT OUTER JOIN {this.tableNamePrefix}adgroup ag
	                            ON ag.id = sgag.AdGroupId
                            INNER JOIN {this.tableNamePrefix}SecurityGroup_User sgu
	                            ON sgu.SecurityGroupId = sg.id
                            WHERE isdeleted = 0
                            AND sg.ID = @GroupId";

            using (var sqlConnection = new SqlConnection { ConnectionString = this.connectionString }) 
            using (var groupCommand = new SqlCommand(groupSql, sqlConnection))
            {
                groupCommand.Parameters.AddWithValue("@GroupID", groupId);
                await sqlConnection.OpenAsync();
                return new RMGroup { Id = groupId, Name = await groupCommand.ExecuteScalarAsync() as string };
            }    
        }

        /// <summary>
        /// Gets User by ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns><see cref="Task{RMUser}"/></returns>
        /// <exception cref="DbException">An error occurred while executing the command text.</exception>
        public async Task<RMUser> GetUserAsync(int userId)
        {
            string userSql = $@"SELECT DisplayName, UserName, Email FROM {this.tableNamePrefix}User WHERE isdeleted = 0 AND id = @UserId";

            using (var sqlConnection = new SqlConnection { ConnectionString = this.connectionString })    
            using (var userCommand = new SqlCommand(userSql, sqlConnection))
            {
                userCommand.Parameters.AddWithValue("@UserID", userId);
                await sqlConnection.OpenAsync();
                return new RMUser() { Id = userId, Name = await userCommand.ExecuteScalarAsync() as string };
            }
        }
    }
}
