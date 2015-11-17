// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RMComponentRepository.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the RMComponentRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading.Tasks;

    using Interfaces;
    using Model;

    public class RMComponentRepository : IRMComponentRepository
    {
        private readonly string connectionString;        
        private readonly string tableNamePrefix;

        public RMComponentRepository(string connectionString, RMVersion version)
        {
            this.connectionString = connectionString;            
            this.tableNamePrefix = version == RMVersion.Rm2015 ? "RM.tbl_" : "dbo.";
        }

        /// <summary>
        /// Get workflow Component by ID
        /// </summary>
        /// <param name="workflowGuid">Workflow GUID</param>
        /// <param name="stageId">Stage ID</param>
        /// <returns><see cref="RMComponent"/></returns>
        /// <exception cref="IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
        /// <exception cref="IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
        /// <exception cref="ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
        /// <exception cref="SqlException">SQL Server returned an error while executing the command text. A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
        /// <exception cref="InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
        /// <exception cref="DbException">An error occurred while executing the command text.</exception>
        /// <exception cref="ArgumentException">Component with specified ID can't be found.</exception>
        /// <exception cref="IndexOutOfRangeException">Trying to read a column that does not exist.</exception>
        /// <exception cref="InvalidOperationException">The connection drops or is closed during the data retrieval.The <see cref="T:System.Data.SqlClient.SqlDataReader" /> is closed during the data retrieval.There is no data ready to be read (for example, the first <see cref="M:System.Data.SqlClient.SqlDataReader.Read" /> hasn't been called, or returned false).Tried to read a previously-read column in sequential mode.There was an asynchronous operation in progress. This applies to all Get* methods when running in sequential mode, as they could be called while reading a stream.</exception>
        /// <exception cref="SqlNullValueException">The value of the column was null (<see cref="M:System.Data.SqlClient.SqlDataReader.IsDBNull(System.Int32)" /> == true), retrieving a non-SQL type.</exception>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        public async Task<RMComponent> GetComponentByIdAsync(Guid workflowGuid, int stageId)
        {
            var sqlQuery = $@"SELECT  c.Id, c.DeployerToolId, c.FileExtensionFilter, c.Command, c.Arguments, c.VariableReplacementModeId
                FROM {this.tableNamePrefix}Component c
                inner join {this.tableNamePrefix}ApplicationVersionStageActivity avsa
				ON c.Id = avsa.ComponentId
                WHERE avsa.ApplicationVersionStageId = @StageId
                AND avsa.WorkflowActivityId = @WorkflowActivityId";

            using (var sqlConnection = new SqlConnection { ConnectionString = this.connectionString })      
            using (var componentCommand = new SqlCommand(sqlQuery, sqlConnection))
            {
                componentCommand.Parameters.AddWithValue("@stageId", stageId);
                componentCommand.Parameters.AddWithValue("@WorkflowActivityId", workflowGuid);
                await sqlConnection.OpenAsync();

                using (var sqlReader = await componentCommand.ExecuteReaderAsync())
                {
                    if (!await sqlReader.ReadAsync())
                    {
                        throw new ArgumentException($"Could not retrieve a component with the ID {workflowGuid}.");
                    }

                    return new RMComponent
                                {
                                    Id = sqlReader.GetFieldValue<int>(0),
                                    DeployerToolId = sqlReader.GetFieldValue<int>(1),
                                    WorkflowActivityId = workflowGuid,
                                    FileExtensionFilter = sqlReader.GetFieldValue<string>(2),
                                    Command = sqlReader.GetFieldValue<string>(3),
                                    Arguments = sqlReader.GetFieldValue<string>(4),
                                    VariableReplacementMethod = (VariableReplacementMethod)sqlReader.GetFieldValue<int>(5),
                                    ConfigurationVariables = await this.GetComponentConfigurationVariablesAsync(workflowGuid, stageId)
                                };
                }
            }
        }

        /// <summary>
        /// Gets configuration variables for component
        /// </summary>
        /// <param name="workflowGuid">Workflow GUID</param>
        /// <param name="stageId">Component ID</param>
        /// <returns>Collection of configuration variables</returns>
        /// <exception cref="InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
        /// <exception cref="SqlException">SQL Server returned an error while executing the command text.A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
        /// <exception cref="IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
        /// <exception cref="DbException">An error occurred while executing the command text.</exception>
        /// <exception cref="SqlNullValueException">The value of the column was null (<see cref="M:System.Data.SqlClient.SqlDataReader.IsDBNull(System.Int32)" /> == true), retrieving a non-SQL type.</exception>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        public async Task<IEnumerable<ConfigurationVariable>> GetComponentConfigurationVariablesAsync(Guid workflowGuid, int stageId)
        {
            var sqlQuery = 
                $@"SELECT cv.Name, cvv.Value, cv.IsParameter, cv.TypeId                                            
                FROM {this.tableNamePrefix}ConfigurationVariable cv	                                        
	            inner join {this.tableNamePrefix}ConfigurationVariableValue cvv
		        ON cv.Id = cvv.ConfigurationVariableId		
			    inner join {this.tableNamePrefix}ApplicationVersionStageActivity avsa
				ON cvv.ApplicationVersionStageActivityId = avsa.Id		                                        
                WHERE avsa.ApplicationVersionStageId = @StageId
                AND avsa.WorkflowActivityId = @WorkflowActivityId";

            using (var sqlConnection = new SqlConnection { ConnectionString = this.connectionString })     
            using (var command = new SqlCommand(sqlQuery, sqlConnection))
            {
                command.Parameters.AddWithValue("@stageId", stageId);
                command.Parameters.AddWithValue("@WorkflowActivityId", workflowGuid);
                await sqlConnection.OpenAsync();

                var configVars = new List<ConfigurationVariable>();
                using (var sqlReader = await command.ExecuteReaderAsync())
                {
                    while (await sqlReader.ReadAsync())
                    {
                        var cv = new ConfigurationVariable
                                        {
                                            OriginalName = sqlReader.GetFieldValue<string>(0),
                                            Value = sqlReader.GetFieldValue<string>(1),
                                            IsParameter = sqlReader.GetFieldValue<bool>(2),
                                            Encrypted = sqlReader.GetFieldValue<int>(3) == 2
                                        };
                        configVars.Add(cv);
                    }

                    return configVars;
                }
             }
        }
    }
}
