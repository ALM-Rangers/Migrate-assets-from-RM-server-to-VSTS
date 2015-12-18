// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RMReleaseTemplateRepository.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the RMReleaseTemplateRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Interfaces;
    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model;

    public class RMReleaseTemplateRepository : IRMReleaseTemplateRepository
    {
        private readonly string connectionString;
        private readonly RMVersion version;

        public RMReleaseTemplateRepository(string connectionString, RMVersion version)
        {
            this.connectionString = connectionString;
            this.version = version;
        }

        public async Task<RMDeploymentSequence> GetDeploymentSequence(string releaseTemplateName, string releaseTemplateStage)
        {
            var tableNamePrefix = this.version == RMVersion.Rm2015 ? "RM.tbl_" : "dbo.";

            var sqlQuery = $@"SELECT avs.Workflow, avs.id [StageId] FROM
                {tableNamePrefix}ApplicationVersion av
                INNER JOIN {tableNamePrefix}ApplicationVersionStage avs
                ON avs.ApplicationVersionId = av.id
                INNER JOIN {tableNamePrefix}Stage s
                ON s.Id = avs.stageid
                INNER JOIN {tableNamePrefix}stagetype st
                ON st.id = s.stagetypeid
                WHERE st.name = @stageName
                AND av.name = @templateName";
                
            using (var sqlConnection = new SqlConnection { ConnectionString = this.connectionString })
            using (var workflowCommand = new SqlCommand(sqlQuery, sqlConnection))
            using (var da = new SqlDataAdapter(workflowCommand))
            using (var results = new DataTable())
            {             
                workflowCommand.Parameters.AddWithValue("@stageName", releaseTemplateStage);
                workflowCommand.Parameters.AddWithValue("@templateName", releaseTemplateName);

                await sqlConnection.OpenAsync();

                da.Fill(results);

                if (results.Rows.Count == 0)
                {
                    throw new ArgumentException($@"Unable to locate a release template named ""{releaseTemplateName}"" with a stage named ""{releaseTemplateStage}"".");
                }

                var sequence = new RMDeploymentSequence
                                    {
                                        WorkflowXaml = (string)results.Rows[0]["Workflow"],
                                        StageId = (int)results.Rows[0]["StageID"]
                                    };

                return sequence;
            }   
        }


        public async Task<IEnumerable<string>> GetReleaseTemplateStages(string releaseTemplateName)
        {

            var tableNamePrefix = this.version == RMVersion.Rm2015 ? "RM.tbl_" : "dbo.";

            var sqlQuery = $@"SELECT distinct st.name [StageName] FROM {tableNamePrefix}ApplicationVersion av
                            inner join {tableNamePrefix}applicationversionstage avs
	                            on avs.ApplicationVersionId = av.id
                            inner join {tableNamePrefix}Stage s
	                            on s.id = avs.stageid
                            inner join {tableNamePrefix}stagetype st 
	                            on st.id = s.stagetypeid
                            where av.name = @templateName";

            var stageNames = new List<string>();
            using (var sqlConnection = new SqlConnection { ConnectionString = this.connectionString })
            using (var stageNamesCommand = new SqlCommand(sqlQuery, sqlConnection))
            {
                stageNamesCommand.Parameters.AddWithValue("@templateName", releaseTemplateName);
                await sqlConnection.OpenAsync();
                
                using (var dataReader = await stageNamesCommand.ExecuteReaderAsync())
                {
                    while (await dataReader.ReadAsync())
                    {
                        var stageName = dataReader.GetString(0); 
                        stageNames.Add(stageName);
                    }
                   
                }
            }
            return stageNames;
        }
    }
}
