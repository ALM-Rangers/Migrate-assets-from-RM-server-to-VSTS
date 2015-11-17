// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RMDeployerToolRepository.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the RMDeployerToolRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Repository
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Security;
    using System.Threading.Tasks;

    using Interfaces;
    using Model;

    public class RMDeployerToolRepository : IRMDeployerToolRepository
    {
        private readonly string connectionString;
        private readonly string tableNamePrefix;

        public RMDeployerToolRepository(string connectionString, RMVersion version)
        {
            this.connectionString = connectionString;
            this.tableNamePrefix = version == RMVersion.Rm2015 ? "RM.tbl_" : "dbo.";
        }

        /// <summary>
        /// Export deployer tool to disk
        /// </summary>
        /// <param name="deployerToolId">Tool ID</param>
        /// <param name="diskPath">Disk path where to export</param>
        /// <returns><see cref="Task"/></returns>
        /// <exception cref="InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
        /// <exception cref="SqlException">SQL Server returned an error while executing the command text.A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
        /// <exception cref="IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
        /// <exception cref="DbException">An error occurred while executing the command text.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="FileNotFoundException">The file cannot be found, such as when mode is FileMode. Truncate or FileMode.Open, and the file specified by path does not exist. The file must already exist in these modes. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public async Task WriteToolToDiskAsync(int deployerToolId, string diskPath)
        {
            if (File.Exists(diskPath))
            {
                return;
            }

            var toolQuery =
                $@"SELECT dt.Name [ToolName], r.Name [FileName], r.BinaryData [Data] FROM {this.tableNamePrefix}DeployerTool dt 
                    INNER JOIN {this.tableNamePrefix}deployertoolresource dtr
	                    ON dtr.DeployerToolId = dt.id
                    INNER JOIN {this.tableNamePrefix}Resource r
	                    ON r.id = dtr.resourceid
                    WHERE dt.id = @DeployerToolId";

            using (var sqlConnection = new SqlConnection { ConnectionString = this.connectionString })
            using (var command = new SqlCommand(toolQuery, sqlConnection))
            {
                command.Parameters.AddWithValue("@DeployerToolId", deployerToolId);
                await sqlConnection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess))
                {
                    while (await reader.ReadAsync())
                    {
                        var fileName = await reader.GetFieldValueAsync<string>(1);
                        var outputPath = Path.Combine(diskPath, fileName);

                        using (var fs = new FileStream(outputPath, FileMode.Create))                        
                        using (var bw = new BinaryWriter(fs))
                        {
                            bw.Write(await reader.GetFieldValueAsync<byte[]>(2));
                            bw.Flush();
                        }
                    }
                }
            }
        }
    }
}
