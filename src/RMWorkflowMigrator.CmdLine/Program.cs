﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.CmdLine
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CommandLine;

    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model;
    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Repository;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell.Model;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Parser;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Parser.Model;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Extensibility;

    public static class Program
    {
        private const string ApplicationInsightsKey = "8493dd94-b866-47d8-ab6d-f61556fcc31a";

        private static readonly Dictionary<string, bool> SupportedVersions = new Dictionary<string, bool>
        {
            { "12.0.21031.1", false },
            { "12.0.30110.0", false },
            { "12.0.30501.0", false },
            { "12.0.30723.0", false }, 
            
            // Everything above this point is untested. U3 may work. Earlier is unlikely.
            { "12.0.31101.0", true },
            { "14.0.23102.0", true },
            { "14.0.24712.0", true },
            { "14.95.25118.0", true },
            { "14.98.25331.0", true }
        };

        private static readonly Dictionary<string, string> VersionMapping = new Dictionary<string, string>
        {
            { "12.0.21031.1", "2013 RTM" },
            { "12.0.30110.0", "2013 Update 1" },
            { "12.0.30501.0", "2013 Update 2" },
            { "12.0.30723.0", "2013 Update 3" },
            { "12.0.31101.0", "2013 Update 4" },
            { "14.0.23102.0", "2015 RTM" },
            { "14.0.24712.0", "2015 Update 1" },
            { "14.95.25118.0", "2015 Update 2" },
            { "14.98.25331.0", "2015 Update 3" }

        };

        private static Options options;

        private static long? elapsedMilliseconds;

        public static void Main(string[] args)
        {
           
            options = GetOptions(args);

            var telemetryClient = CreateTelemetryClient(ApplicationInsightsKey, options.NoMetrics);
            
            try
            {
                if (!string.IsNullOrEmpty(options.OutputFolder) && options.OutputFolder.Contains("\""))
                {
                    telemetryClient.TrackEvent("DisplayParameters/OutputPath");

                    Console.WriteLine("ERROR: The OutputPath (-o) parameter was provided with a trailing backslash. Please remove the trailing backslash and try again.");
                    DisplayParameters();
                    return;
                }

                if (options.LastParserState != null && options.LastParserState.Errors.Any())
                {
                    telemetryClient.TrackEvent("DisplayParameters/All"); 

                    DisplayParameters();
                    return;
                }

                // Check we have at least been passed a server name
                if (string.IsNullOrEmpty(options.SqlServerName))
                {
                    return;
                }
                var stopwatch = Stopwatch.StartNew();

                var generatorTask = RunGeneratorAsync();

                Task.WaitAll(generatorTask);

                stopwatch.Stop();
                elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

                telemetryClient.TrackEvent("Executed");
                telemetryClient.TrackMetric("Execution Duration", stopwatch.ElapsedMilliseconds);
            }
            finally
            {
                telemetryClient.Flush();
                if (elapsedMilliseconds.HasValue)
                {
                    PrintOnlyIfVerbose($"Total execution time: {elapsedMilliseconds} ms");
                }
            }
        }

        private static Options GetOptions(string[] args)
        {
            var parsedOptions = new Options();
            try
            {
                if (!Parser.Default.ParseArguments(args, parsedOptions))
                {
                    return parsedOptions;
                }
            }
            catch (ParserException ex)
            {
                Console.WriteLine($"{Environment.NewLine}Microsoft.ALMRangers.RMWorkflowMigrator: Parameter error");
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }

            return parsedOptions;
        }

        private static async Task<RMVersion> RetrieveRmVersion()
        {
            var versionRepo = new RMVersionRepository(options.ConnectionString);
            var version = await versionRepo.GetRMVersion();
            if (!VersionMapping.ContainsKey(version))
            {
                throw new UnsupportedReleaseManagementVersionException(GenerateUnsupportedVersionExceptionMessage(version));
            }
            PrintOnlyIfVerbose($"Release Management version detected: {version} ({VersionMapping[version]})");

            if (SupportedVersions[version])
            {
                return version.StartsWith("14.") ? RMVersion.Rm2015 : RMVersion.Rm2013;
            }

            throw new UnsupportedReleaseManagementVersionException(GenerateUnsupportedVersionExceptionMessage(version));
        }

        private static string GenerateUnsupportedVersionExceptionMessage(string version)
        {
            var exceptionMessage = new StringBuilder();
            exceptionMessage.AppendLine(
                !VersionMapping.ContainsKey(version)
                    ? $"Version {version} is unsupported."
                    : $"Version {version} ({VersionMapping[version]}) is unsupported.");
            var supportedVersions = SupportedVersions.Where(srv => srv.Value).Select(srv => $"{VersionMapping[srv.Key]} ({srv.Key})");
            exceptionMessage.AppendLine("Supported versions:");
            exceptionMessage.AppendLine(string.Join($",{Environment.NewLine}", supportedVersions));
            return exceptionMessage.ToString();
        }

        private static async Task RunGeneratorAsync()
        {
            try
            {
                var version = await RetrieveRmVersion();
                if (options.TemplateStage == null)
                {
                    var releaseTemplateRepo = new RMReleaseTemplateRepository(options.ConnectionString, version);
                   
                    var stages = await releaseTemplateRepo.GetReleaseTemplateStages(options.TemplateName);
                    if (!stages.Any())
                    {
                        throw new ArgumentException($@"The release template ""{options.TemplateName}"" has no stages. Check that the release template name is spelled correctly.", nameof(options.TemplateName));
                    }

                    foreach (var stage in stages)
                    {
                        await RetrieveWorkflowAndGenerateScript(version, options.TemplateName, stage);
                    }
                }
                else
                {
                    await RetrieveWorkflowAndGenerateScript(version, options.TemplateName, options.TemplateStage);
                }
            }
            catch (AggregateException ae)
            {
                ae.Handle(x =>
                {
                    if (x is SqlException) // This we know how to handle.
                    {
                        Console.WriteLine($"{Environment.NewLine}Cannot connect to the DB '{0}' on the SQL server '{1}'{Environment.NewLine}", options.DatabaseName, options.SqlServerName); return true;
                    }

                    Console.WriteLine($"{Environment.NewLine}Unexpected error in processing '{0}'{Environment.NewLine}", x.Message);
                    return false;
                });
            }
            // ReSharper disable once CatchAllClause
            catch (Exception ex)
            {
                // final global catch
                Console.WriteLine("A critical error occurred:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static async Task RetrieveWorkflowAndGenerateScript(RMVersion version, string releaseTemplateName, string releaseTemplateStage)
        {
            var workflow = await RetrieveWorkflow(version, releaseTemplateName, releaseTemplateStage);
            if (workflow != null)
            {
                PrintOnlyIfVerbose($"Generating the scripts for the workflow '{releaseTemplateName}' stage '{releaseTemplateStage}' into folder '{options.OutputFolder}'{Environment.NewLine}");
                var scriptGenerator = ConfigureScriptGenerator(version);

                DeploymentSequence sequence;
                PrintOnlyIfVerbose("Parsing release template");
                try
                {
                    sequence = workflow.ToReleaseTemplate();
                }
                catch (UnsupportedReleaseTemplateTypeException)
                {
                    Console.WriteLine(
                        $"Error: {options.TemplateName} is a vNext template. vNext release templates are unsupported. Please refer to the documentation for guidance on migrating vNext release templates.");
                    return;
                }

                PrintOnlyIfVerbose("Done parsing release template");

                PrintOnlyIfVerbose("Generating PowerShell");
                await scriptGenerator.GenerateScriptAsync(sequence, options.OutputFolder);
                PrintOnlyIfVerbose("Done generating PowerShell");

                Console.WriteLine($"{Environment.NewLine}Release workflow generated");

                scriptGenerator.ScriptGenerationNotification -= PrintGeneratorEvents;
                ActionParser.ActionParsed -= PrintActionEvents;
                ContainerParser.ContainerParsed -= PrintContainerEvents;
            }
            else
            {
                Console.WriteLine(
                    $"{Environment.NewLine}No Results returned for TemplateName: '{0}' and StageName: '{1}'\n",
                    options.TemplateName,
                    options.TemplateStage);
            }
        }

        private static ScriptGenerator ConfigureScriptGenerator(RMVersion version)
        {
           

            // make sure we have the output folder
            var fs = new FileSystem();
            fs.CreateDirectory(options.OutputFolder);

            var scriptGenerator = new ScriptGenerator(
                fs,
                new RMComponentRepository(options.ConnectionString, version),
                new RMUserRepository(options.ConnectionString, version),
                new RMDeployerToolRepository(options.ConnectionString, version),
                !options.CreateParameterizedScripts);

            // enable full logging
            scriptGenerator.ScriptGenerationNotification += PrintGeneratorEvents;
            ActionParser.ActionParsed += PrintActionEvents;
            ContainerParser.ContainerParsed += PrintContainerEvents;
            return scriptGenerator;
        }

        private static async Task<RMDeploymentSequence> RetrieveWorkflow(RMVersion version, string releaseTemplateName, string releaseTemplateStage)
        {
            var repository = new RMReleaseTemplateRepository(
                options.ConnectionString,
                version);

            PrintOnlyIfVerbose($"{Environment.NewLine}Connecting to the DB '{options.DatabaseName}' on the SQL server '{options.SqlServerName}'");

            // get the workflow
            var workflow = await repository.GetDeploymentSequence(releaseTemplateName, releaseTemplateStage);
            workflow.ReleaseTemplateName = releaseTemplateName;
            workflow.ReleaseTemplateStageName = releaseTemplateStage;
            return workflow;
        }

        private static void PrintContainerEvents(object sender, ContainerParsedEventArgs args)
        {
            PrintOnlyIfVerbose($"{args.ItemType}: {args.DisplayName}");
        }

        private static void PrintGeneratorEvents(object sender, GenerationEventArgs args)
        {
            PrintOnlyIfVerbose($"{args.GenerationEventType}: {args.Sequence} - {args.BlockType} {args.DisplayName}  IsContainer: {args.IsContainer} IsEnabled: {args.IsEnabled}");
        }

        private static void PrintActionEvents(object sender, ActionParsedEventArgs args)
        {
            PrintOnlyIfVerbose($"{args.ItemType}: {args.DisplayName}");
        }

        private static void PrintOnlyIfVerbose(string eventText)
        {
            if (!options.Verbose)
            {
                return;
            }

            Console.WriteLine(eventText);
            Console.Out.Flush();
        }

        /// <summary>
        /// List the provided parameters
        /// </summary>
        private static void DisplayParameters()
        {
            Console.WriteLine($"{Environment.NewLine}Microsoft.ALMRangers.RMWorkflowMigrator Tool");
            Console.WriteLine("============================================");
            Console.WriteLine($"OutputFolder:\t\t\t{options.OutputFolder}");
            Console.WriteLine($"ConnectionString:\t\t{options.ConnectionString}");
            Console.WriteLine($"TemplateName:\t\t\t{options.TemplateName}");
            Console.WriteLine($"TemplateStage:\t\t\t{options.TemplateStage}");
            Console.WriteLine($"CreateParameterizedScripts:\t{options.CreateParameterizedScripts}{Environment.NewLine}");
            Console.WriteLine($"Verbose:\t\t\t{options.Verbose}{Environment.NewLine}");
            Console.WriteLine();
        }

        private static TelemetryClient CreateTelemetryClient(string instrumentationKey, bool noMetrics)
        {
            var configuration = TelemetryConfiguration.CreateDefault();
            configuration.DisableTelemetry = noMetrics;

            var telemetryClient = new TelemetryClient(configuration) { InstrumentationKey = instrumentationKey };

            return telemetryClient;
        }
    }
}
