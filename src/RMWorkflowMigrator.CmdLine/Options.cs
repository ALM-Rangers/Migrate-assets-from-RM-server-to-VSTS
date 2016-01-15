// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Options.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the Options type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Microsoft.ALMRangers.RMWorkflowMigrator.CmdLine
{
    using System;
    using System.Data.SqlClient;

    using CommandLine;
    using CommandLine.Text;

    internal class Options
    {
        [Option('n', "SqlServerName", Required = true, HelpText = "The name of the SQL server hosting the Release Management database.")]
        public string SqlServerName { get; set; }

        [Option('d', "DatabaseName", Required = false, DefaultValue="ReleaseManagement", HelpText = "The name of the Release Management database.")]
        public string DatabaseName { get; set; }

        [Option("ConnectTimeout", Required = false, DefaultValue = 15, HelpText = "The length of time (in seconds) to wait for a connection to the SQL server before terminating the attempt and generating an error. By default timeout is 15 seconds.")]
        public int ConnectTimeout { get; set; }

        [Option('l', "NetworkLibrary", Required = false, HelpText = "The name of the network library used to establish a connection to the SQL Server. Supported values include dbnmpntw (Named Pipes), dbmsrpcn (Multiprotocol), dbmsadsn (AppleTalk), dbmsgnet (VIA), dbmslpcn (Shared Memory) and dbmsspxn (IPX/SPX), and dbmssocn (TCP/IP). The corresponding network DLL must be installed on the system to which you connect. If you do not specify a network and you use a local server (for example, \".\" or \"(local)\"), Shared Memory is used.")]
        public string NetworkLibrary { get; set; }

        [Option('t', "TemplateName", Required = true, HelpText = "Name of the release template to export. If the name contains spaces use \"the name\"")]
        public string TemplateName { get; set; }

        [Option('s', "TemplateStage", Required = false, DefaultValue = null, HelpText = "Stage of the template to export. If the name contains spaces use \"the name\". If omitted, the tool will extract scripts for all stages.")]
        public string TemplateStage { get; set; }

        [Option('o', "OutputFolder", DefaultValue = "Output", HelpText = @"The folder in which the tool output will be created. Can be relative or absolute. If this parameter is being enclosed in quotation marks, do not include a trailing backslash. Ex: ""C:\Output"", not ""C:\Output\""")]
        public string OutputFolder { get; set; }

        [Option('c', "CreateParameterizedScripts", DefaultValue = false, HelpText = "Create scripts with parameter blocks instead of separate initialization scripts")]
        public bool CreateParameterizedScripts { get; set; }

        [Option('v', "Verbose", DefaultValue = false, HelpText = "Prints the detailed messages to standard output.")]
        public bool Verbose { get; set; }

        [Option('m', "NoMetrics", DefaultValue = false, HelpText = "Stops all metrics from being captured.")]
        public bool NoMetrics { get; set; }

        public string ConnectionString
        {
            get
            {
                var builder = new SqlConnectionStringBuilder { IntegratedSecurity = true, ConnectTimeout = this.ConnectTimeout };

                if (!string.IsNullOrEmpty(this.SqlServerName))
                {
                    builder.DataSource = this.SqlServerName;
                }

                if (!string.IsNullOrEmpty(this.DatabaseName))
                {
                    builder.InitialCatalog = this.DatabaseName;
                }

                if (!string.IsNullOrEmpty(this.NetworkLibrary))
                {
                    builder.NetworkLibrary = this.NetworkLibrary;
                }

                return builder.ConnectionString;
            }
        }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
