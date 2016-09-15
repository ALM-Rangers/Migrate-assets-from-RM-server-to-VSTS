// --------------------------------------------------------------------------------------------------------------------
// <copyright file="When_Generating_Scripts.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Management.Automation;
    using System.Threading.Tasks;

    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell.Model;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Parser;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Tests.Unit.TestHelpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class When_Generating_Scripts
    {
        [TestMethod]
        public async Task
            An_Backslash_Escaped_Quotation_Mark_In_A_Variable_Is_Replaced_With_A_Powershell_Escape_Sequence()
        {
            var fakeFs = new FakeFileSystem();
            var componentRepo = new FakeComponentRepo();
            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4010, 
                        WorkflowActivityId = Guid.Parse("d25617e8-7fac-4637-a7cb-2ad4da996156"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"-DropWebSite -sn ""__SiteName__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                @"\""TestValue\""", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4016, 
                        WorkflowActivityId = Guid.Parse("00152F01-0651-4221-9835-3394F1739A2F"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"*.* ""__Installation Path__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "Installation Path", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 3981, 
                        WorkflowActivityId = Guid.Parse("E7783400-9B05-4970-8777-6AB92C78D155"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"""__SourceFileFolder__"" ""__DestinationFileFolder__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SourceFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\Backup\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "DestinationFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4009, 
                        WorkflowActivityId = Guid.Parse("A657D65D-F4D6-42F8-AA1F-7AC638496557"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments =
                            @"-CreateWebSite -sn ""__SiteName__"" -port __PortNumber__ -pd ""__PhysicalPath__"" -ap ""__AppPoolName__"" -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "AppPoolName", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsPreloadEnabled", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsAutoStart", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                @"FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PortNumber", 
                                            Value = @"8000", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PhysicalPath", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4009, 
                        WorkflowActivityId = Guid.Parse("51C262E8-D1FE-4FF8-ADD6-879C2718616B"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments =
                            @"-CreateWebSite -sn ""__SiteName__"" -port __PortNumber__ -pd ""__PhysicalPath__"" -ap ""__AppPoolName__"" -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "AppPoolName", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsPreloadEnabled", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsAutoStart", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                @"FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PortNumber", 
                                            Value = @"8000", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PhysicalPath", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 3981, 
                        WorkflowActivityId = Guid.Parse("8160F49B-34C4-4860-AEC4-B9FC4CF81E9B"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"""__SourceFileFolder__"" ""__DestinationFileFolder__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SourceFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "DestinationFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\Backup\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            var fakeUserRepo = new FakeUserRepo();
            var fakeDeployerToolRepo = new FakeDeployerToolRepo();

            var scriptGenerator = new ScriptGenerator(fakeFs, componentRepo, fakeUserRepo, fakeDeployerToolRepo);
            var path = @"C:\RMWorkflow";
            var releaseTemplate =
                @"<DeploymentSequenceActivity xmlns=""clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow"" xmlns:mtrdm=""clr-namespace:Microsoft.TeamFoundation.Release.Data.Model;assembly=Microsoft.TeamFoundation.Release.Data"" xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities"" xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation"" xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" DisplayName=""Deployment Sequence"" sap:VirtualizedContainerService.HintSize=""399,1234"" mva:VisualBasic.Settings=""Assembly references and imported namespaces serialized as XML namespaces""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><ServerActivity sap:VirtualizedContainerService.HintSize=""377,1110"" InstanceSequenceNumber=""1"" ServerId=""1"" ServerName=""VSALM""><ActionActivity DisplayName=""Remove Web Site"" sap:VirtualizedContainerService.HintSize=""355,79"" IsSkipped=""False"" WorkflowActivityId=""d25617e8-7fac-4637-a7cb-2ad4da996156""><ActionActivity.StageActivity><mtrdm:StageActivity ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4010"" HasPendingSecurityChanges=""False"" Id=""26"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""d25617e8-7fac-4637-a7cb-2ad4da996156""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""-DropWebSite -sn &quot;__SiteName__&quot;"" CategoryId=""2005"" CategoryName=""IIS"" Command=""IISConfig.exe"" DeployerToolId=""2014"" DeploymentMethod=""2013"" Description=""Remove a web site (IIS 6.0+)"" HasPendingSecurityChanges=""False"" Id=""4010"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Remove Web Site"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID0"" Id=""10022"" Name=""SiteName"" Value="""" /></mtrdm:Component.ConfigurationVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID1"" Id=""10022"" Name=""SiteName"" Value=""FabrikamDev"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID1</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""355,97"" IsSkipped=""False"" WorkflowActivityId=""8160f49b-34c4-4860-aec4-b9fc4cf81e9b""><ActionActivity.StageActivity><mtrdm:StageActivity ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""27"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""8160f49b-34c4-4860-aec4-b9fc4cf81e9b""><mtrdm:StageActivity.Component><mtrdm:Component x:Name=""__ReferenceID20"" ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""&quot;__SourceFileFolder__&quot; &quot;__DestinationFileFolder__&quot;"" CategoryId=""1998"" CategoryName=""Windows OS"" Command=""irxcopy.cmd"" DeployerToolId=""12"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID2"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID3"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:Component.ConfigurationVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID4"" Id=""9950"" Name=""SourceFileFolder"" Value=""c:\FabrikamRM\WebSite\DEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID5"" Id=""9951"" Name=""DestinationFileFolder"" Value=""c:\FabrikamRM\Backup\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID4</x:Reference><x:Reference>__ReferenceID5</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ComponentActivity DisplayName=""Fabrikam Call Center"" sap:VirtualizedContainerService.HintSize=""355,51"" IsSkipped=""False"" WorkflowActivityId=""00152f01-0651-4221-9835-3394f1739a2f""><ComponentActivity.StageActivity><mtrdm:StageActivity ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4016"" HasPendingSecurityChanges=""False"" Id=""-416602"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""00152f01-0651-4221-9835-3394f1739a2f""><mtrdm:StageActivity.Component><mtrdm:ApplicationVersionComponent Id=""4016"" InMemoryStatus=""Existing"" Name=""Fabrikam Call Center"" OverrideRelativePathToPackageLocation=""""><mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID6"" Id=""10034"" Name=""Installation Path"" Value="""" /></mtrdm:ApplicationVersionComponent.ConfigurationVariables></mtrdm:ApplicationVersionComponent></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID7"" Id=""10034"" Name=""Installation Path"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ComponentActivity.StageActivity><ComponentActivity.StageActivityVariables><x:Reference>__ReferenceID7</x:Reference></ComponentActivity.StageActivityVariables></ComponentActivity><ActionActivity DisplayName=""Create Web Site"" sap:VirtualizedContainerService.HintSize=""355,169"" IsSkipped=""False"" WorkflowActivityId=""51c262e8-d1fe-4ff8-add6-879c2718616b""><ActionActivity.StageActivity><mtrdm:StageActivity ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4009"" HasPendingSecurityChanges=""False"" Id=""28"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""51c262e8-d1fe-4ff8-add6-879c2718616b""><mtrdm:StageActivity.Component><mtrdm:Component x:Name=""__ReferenceID23"" ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""-CreateWebSite -sn &quot;__SiteName__&quot; -port __PortNumber__ -pd &quot;__PhysicalPath__&quot; -ap &quot;__AppPoolName__&quot; -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__"" CategoryId=""2005"" CategoryName=""IIS"" Command=""IISConfig.exe"" DeployerToolId=""2014"" DeploymentMethod=""2013"" Description=""Create a web site (IIS 6.0+)"" HasPendingSecurityChanges=""False"" Id=""4009"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Create Web Site"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID8"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID9"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID10"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID11"" Id=""10019"" Name=""SiteName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID12"" Id=""10020"" Name=""PortNumber"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID13"" Id=""10021"" Name=""PhysicalPath"" Value="""" /></mtrdm:Component.ConfigurationVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID14"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID15"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID16"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID17"" Id=""10019"" Name=""SiteName"" Value=""FabrikamDEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID18"" Id=""10020"" Name=""PortNumber"" Value=""8000"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID19"" Id=""10021"" Name=""PhysicalPath"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID14</x:Reference><x:Reference>__ReferenceID15</x:Reference><x:Reference>__ReferenceID16</x:Reference><x:Reference>__ReferenceID17</x:Reference><x:Reference>__ReferenceID18</x:Reference><x:Reference>__ReferenceID19</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><RollbackActivity DisplayName=""Rollback"" sap:VirtualizedContainerService.HintSize=""355,430""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""333,97"" IsSkipped=""False"" WorkflowActivityId=""e7783400-9b05-4970-8777-6ab92c78d155""><ActionActivity.StageActivity><mtrdm:StageActivity Component=""{x:Reference __ReferenceID20}"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""29"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""0"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""e7783400-9b05-4970-8777-6ab92c78d155""><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID21"" Id=""9950"" Name=""SourceFileFolder"" Value=""c:\FabrikamRM\Backup\DEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID22"" Id=""9951"" Name=""DestinationFileFolder"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID21</x:Reference><x:Reference>__ReferenceID22</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ActionActivity DisplayName=""Create Web Site"" sap:VirtualizedContainerService.HintSize=""333,169"" IsSkipped=""False"" WorkflowActivityId=""a657d65d-f4d6-42f8-aa1f-7ac638496557""><ActionActivity.StageActivity><mtrdm:StageActivity Component=""{x:Reference __ReferenceID23}"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4009"" HasPendingSecurityChanges=""False"" Id=""30"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""0"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""a657d65d-f4d6-42f8-aa1f-7ac638496557""><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID24"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID25"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID26"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID27"" Id=""10019"" Name=""SiteName"" Value=""FabrikamDEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID28"" Id=""10020"" Name=""PortNumber"" Value=""8000"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID29"" Id=""10021"" Name=""PhysicalPath"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID24</x:Reference><x:Reference>__ReferenceID25</x:Reference><x:Reference>__ReferenceID26</x:Reference><x:Reference>__ReferenceID27</x:Reference><x:Reference>__ReferenceID28</x:Reference><x:Reference>__ReferenceID29</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity></RollbackActivity></ServerActivity></DeploymentSequenceActivity>";

            var parsedTemplate =
                (new RMDeploymentSequence { StageId = 100, WorkflowXaml = releaseTemplate }).ToReleaseTemplate();
            scriptGenerator.ScriptGenerationNotification += PrintEvents;
            await scriptGenerator.GenerateScriptAsync(parsedTemplate, path);

            Collection<PSParseError> errors;
            PSParser.Tokenize(fakeFs.Files["C:\\RMWorkflow\\01_Server_VSALM\\InitializationScript.ps1"], out errors);
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public async Task FabrikamFiber_From_BKVM_Component_NoTools()
        {
            var fakeFs = new FakeFileSystem();
            var componentRepo = new FakeComponentRepo();
            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4010, 
                        WorkflowActivityId = Guid.Parse("d25617e8-7fac-4637-a7cb-2ad4da996156"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"-DropWebSite -sn ""__SiteName__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                "FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4016, 
                        WorkflowActivityId = Guid.Parse("00152F01-0651-4221-9835-3394F1739A2F"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"*.* ""__Installation Path__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "Installation Path", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4020, 
                        WorkflowActivityId = Guid.Parse("3C8D22D2-37F3-469E-9038-26AA438FB34C"), 
                        Command = "placeholder.exe", 
                        DeployerToolId = 0, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"__Bar__", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "Bar", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 3981, 
                        WorkflowActivityId = Guid.Parse("E7783400-9B05-4970-8777-6AB92C78D155"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"""__SourceFileFolder__"" ""__DestinationFileFolder__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SourceFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\Backup\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "DestinationFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4009, 
                        WorkflowActivityId = Guid.Parse("A657D65D-F4D6-42F8-AA1F-7AC638496557"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments =
                            @"-CreateWebSite -sn ""__SiteName__"" -port __PortNumber__ -pd ""__PhysicalPath__"" -ap ""__AppPoolName__"" -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "AppPoolName", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsPreloadEnabled", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsAutoStart", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                @"FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PortNumber", 
                                            Value = @"8000", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PhysicalPath", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4009, 
                        WorkflowActivityId = Guid.Parse("51C262E8-D1FE-4FF8-ADD6-879C2718616B"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments =
                            @"-CreateWebSite -sn ""__SiteName__"" -port __PortNumber__ -pd ""__PhysicalPath__"" -ap ""__AppPoolName__"" -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "AppPoolName", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsPreloadEnabled", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsAutoStart", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                @"FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PortNumber", 
                                            Value = @"8000", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PhysicalPath", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 3981, 
                        WorkflowActivityId = Guid.Parse("8160F49B-34C4-4860-AEC4-B9FC4CF81E9B"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"""__SourceFileFolder__"" ""__DestinationFileFolder__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SourceFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "DestinationFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\Backup\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            var fakeUserRepo = new FakeUserRepo();
            var fakeDeployerToolRepo = new FakeDeployerToolRepo();

            var scriptGenerator = new ScriptGenerator(fakeFs, componentRepo, fakeUserRepo, fakeDeployerToolRepo);
            var path = @"C:\RMWorkflow";
            var releaseTemplate =
                @"<DeploymentSequenceActivity xmlns=""clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow"" xmlns:mtrdm=""clr-namespace:Microsoft.TeamFoundation.Release.Data.Model;assembly=Microsoft.TeamFoundation.Release.Data"" xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities"" xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation"" xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" DisplayName=""Deployment Sequence"" sap:VirtualizedContainerService.HintSize=""401.6,1374.4"" mva:VisualBasic.Settings=""Assembly references and imported namespaces serialized as XML namespaces""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><ServerActivity sap:VirtualizedContainerService.HintSize=""379.2,1249.6"" InstanceSequenceNumber=""1"" ServerId=""1"" ServerName=""VSALM""><ActionActivity DisplayName=""Remove Web Site"" sap:VirtualizedContainerService.HintSize=""356.8,81.6"" IsSkipped=""False"" WorkflowActivityId=""d25617e8-7fac-4637-a7cb-2ad4da996156""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Remove Web Site"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4010"" HasPendingSecurityChanges=""False"" Id=""137"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""d25617e8-7fac-4637-a7cb-2ad4da996156""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""-DropWebSite -sn &quot;__SiteName__&quot;"" CategoryId=""2005"" CategoryName=""IIS"" Command=""IISConfig.exe"" DeployerToolId=""2014"" DeploymentMethod=""2013"" Description=""Remove a web site (IIS 6.0+)"" HasPendingSecurityChanges=""False"" Id=""4010"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Remove Web Site"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID0"" Id=""10022"" Name=""SiteName"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID1"" Id=""10022"" Name=""SiteName"" Value=""FabrikamDev"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID1</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""356.8,99.2"" IsSkipped=""False"" WorkflowActivityId=""8160f49b-34c4-4860-aec4-b9fc4cf81e9b""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""138"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""8160f49b-34c4-4860-aec4-b9fc4cf81e9b""><mtrdm:StageActivity.Component><mtrdm:Component x:Name=""__ReferenceID20"" ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""&quot;__SourceFileFolder__&quot; &quot;__DestinationFileFolder__&quot;"" CategoryId=""1998"" CategoryName=""Windows OS"" Command=""irxcopy.cmd"" DeployerToolId=""12"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID2"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID3"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID4"" Id=""9950"" Name=""SourceFileFolder"" Value=""c:\FabrikamRM\WebSite\DEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID5"" Id=""9951"" Name=""DestinationFileFolder"" Value=""c:\FabrikamRM\Backup\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID4</x:Reference><x:Reference>__ReferenceID5</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ComponentActivity DisplayName=""Fabrikam Call Center"" sap:VirtualizedContainerService.HintSize=""356.8,52.8"" IsSkipped=""False"" WorkflowActivityId=""00152f01-0651-4221-9835-3394f1739a2f""><ComponentActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Fabrikam Call Center"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4016"" HasPendingSecurityChanges=""False"" Id=""142"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""00152f01-0651-4221-9835-3394f1739a2f""><mtrdm:StageActivity.Component><mtrdm:ApplicationVersionComponent Id=""4016"" InMemoryStatus=""Existing"" Name=""Fabrikam Call Center"" OverrideRelativePathToPackageLocation=""""><mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID6"" Id=""10034"" Name=""Installation Path"" Value="""" /></mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ApplicationVersionComponent.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:ApplicationVersionComponent.PropertyBagVariables></mtrdm:ApplicationVersionComponent></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID7"" Id=""10034"" Name=""Installation Path"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ComponentActivity.StageActivity><ComponentActivity.StageActivityVariables><x:Reference>__ReferenceID7</x:Reference></ComponentActivity.StageActivityVariables></ComponentActivity><ActionActivity DisplayName=""Create Web Site"" sap:VirtualizedContainerService.HintSize=""356.8,172.8"" IsSkipped=""False"" WorkflowActivityId=""51c262e8-d1fe-4ff8-add6-879c2718616b""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Create Web Site"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4009"" HasPendingSecurityChanges=""False"" Id=""139"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""51c262e8-d1fe-4ff8-add6-879c2718616b""><mtrdm:StageActivity.Component><mtrdm:Component x:Name=""__ReferenceID23"" ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""-CreateWebSite -sn &quot;__SiteName__&quot; -port __PortNumber__ -pd &quot;__PhysicalPath__&quot; -ap &quot;__AppPoolName__&quot; -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__"" CategoryId=""2005"" CategoryName=""IIS"" Command=""IISConfig.exe"" DeployerToolId=""2014"" DeploymentMethod=""2013"" Description=""Create a web site (IIS 6.0+)"" HasPendingSecurityChanges=""False"" Id=""4009"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Create Web Site"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID8"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID9"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID10"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID11"" Id=""10019"" Name=""SiteName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID12"" Id=""10020"" Name=""PortNumber"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID13"" Id=""10021"" Name=""PhysicalPath"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID14"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID15"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID16"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID17"" Id=""10019"" Name=""SiteName"" Value=""FabrikamDEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID18"" Id=""10020"" Name=""PortNumber"" Value=""8000"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID19"" Id=""10021"" Name=""PhysicalPath"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID14</x:Reference><x:Reference>__ReferenceID15</x:Reference><x:Reference>__ReferenceID16</x:Reference><x:Reference>__ReferenceID17</x:Reference><x:Reference>__ReferenceID18</x:Reference><x:Reference>__ReferenceID19</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><RollbackActivity DisplayName=""Rollback"" sap:VirtualizedContainerService.HintSize=""356.8,436.8""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""334.4,99.2"" IsSkipped=""False"" WorkflowActivityId=""e7783400-9b05-4970-8777-6ab92c78d155""><ActionActivity.StageActivity><mtrdm:StageActivity Component=""{x:Reference __ReferenceID20}"" ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""140"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""0"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""e7783400-9b05-4970-8777-6ab92c78d155""><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID21"" Id=""9950"" Name=""SourceFileFolder"" Value=""c:\FabrikamRM\Backup\DEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID22"" Id=""9951"" Name=""DestinationFileFolder"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID21</x:Reference><x:Reference>__ReferenceID22</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ActionActivity DisplayName=""Create Web Site"" sap:VirtualizedContainerService.HintSize=""334.4,172.8"" IsSkipped=""False"" WorkflowActivityId=""a657d65d-f4d6-42f8-aa1f-7ac638496557""><ActionActivity.StageActivity><mtrdm:StageActivity Component=""{x:Reference __ReferenceID23}"" ActivityDisplayName=""Create Web Site"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4009"" HasPendingSecurityChanges=""False"" Id=""141"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""0"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""a657d65d-f4d6-42f8-aa1f-7ac638496557""><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID24"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID25"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID26"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID27"" Id=""10019"" Name=""SiteName"" Value=""FabrikamDEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID28"" Id=""10020"" Name=""PortNumber"" Value=""8000"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID29"" Id=""10021"" Name=""PhysicalPath"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID24</x:Reference><x:Reference>__ReferenceID25</x:Reference><x:Reference>__ReferenceID26</x:Reference><x:Reference>__ReferenceID27</x:Reference><x:Reference>__ReferenceID28</x:Reference><x:Reference>__ReferenceID29</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity></RollbackActivity><ComponentActivity DisplayName=""Fabrikam Call Center - No tool"" sap:VirtualizedContainerService.HintSize=""356.8,81.6"" IsSkipped=""False"" WorkflowActivityId=""3c8d22d2-37f3-469e-9038-26aa438fb34c""><ComponentActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Fabrikam Call Center - No tool"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4020"" HasPendingSecurityChanges=""False"" Id=""144"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""3c8d22d2-37f3-469e-9038-26aa438fb34c""><mtrdm:StageActivity.Component><mtrdm:ApplicationVersionComponent Id=""4020"" InMemoryStatus=""Existing"" Name=""Fabrikam Call Center - No tool"" OverrideRelativePathToPackageLocation=""""><mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID30"" Id=""10044"" Name=""Bar"" Value="""" /></mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ApplicationVersionComponent.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:ApplicationVersionComponent.PropertyBagVariables></mtrdm:ApplicationVersionComponent></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID31"" Id=""10044"" Name=""Bar"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ComponentActivity.StageActivity><ComponentActivity.StageActivityVariables><x:Reference>__ReferenceID31</x:Reference></ComponentActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ComponentActivity></ServerActivity></DeploymentSequenceActivity>";

            var parsedTemplate =
                (new RMDeploymentSequence { StageId = 100, WorkflowXaml = releaseTemplate }).ToReleaseTemplate();
            scriptGenerator.ScriptGenerationNotification += PrintEvents;
            await scriptGenerator.GenerateScriptAsync(parsedTemplate, path);
        }

        [TestMethod]
        public async Task FabrikamFiber_From_BKVM_Proof_Of_Concept()
        {
            var fakeFs = new FakeFileSystem();
            var componentRepo = new FakeComponentRepo();
            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4010, 
                        WorkflowActivityId = Guid.Parse("d25617e8-7fac-4637-a7cb-2ad4da996156"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"-DropWebSite -sn ""__SiteName__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                "FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4016, 
                        WorkflowActivityId = Guid.Parse("00152F01-0651-4221-9835-3394F1739A2F"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"*.* ""__Installation Path__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "Installation Path", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 3981, 
                        WorkflowActivityId = Guid.Parse("E7783400-9B05-4970-8777-6AB92C78D155"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"""__SourceFileFolder__"" ""__DestinationFileFolder__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SourceFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\Backup\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "DestinationFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4009, 
                        WorkflowActivityId = Guid.Parse("A657D65D-F4D6-42F8-AA1F-7AC638496557"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments =
                            @"-CreateWebSite -sn ""__SiteName__"" -port __PortNumber__ -pd ""__PhysicalPath__"" -ap ""__AppPoolName__"" -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "AppPoolName", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsPreloadEnabled", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsAutoStart", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                @"FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PortNumber", 
                                            Value = @"8000", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PhysicalPath", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4009, 
                        WorkflowActivityId = Guid.Parse("51C262E8-D1FE-4FF8-ADD6-879C2718616B"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments =
                            @"-CreateWebSite -sn ""__SiteName__"" -port __PortNumber__ -pd ""__PhysicalPath__"" -ap ""__AppPoolName__"" -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "AppPoolName", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsPreloadEnabled", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsAutoStart", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                @"FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PortNumber", 
                                            Value = @"8000", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PhysicalPath", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 3981, 
                        WorkflowActivityId = Guid.Parse("8160F49B-34C4-4860-AEC4-B9FC4CF81E9B"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"""__SourceFileFolder__"" ""__DestinationFileFolder__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SourceFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "DestinationFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\Backup\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            var fakeUserRepo = new FakeUserRepo();
            var fakeDeployerToolRepo = new FakeDeployerToolRepo();

            var scriptGenerator = new ScriptGenerator(fakeFs, componentRepo, fakeUserRepo, fakeDeployerToolRepo);
            var path = @"C:\RMWorkflow";
            var releaseTemplate =
                @"<DeploymentSequenceActivity xmlns=""clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow"" xmlns:mtrdm=""clr-namespace:Microsoft.TeamFoundation.Release.Data.Model;assembly=Microsoft.TeamFoundation.Release.Data"" xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities"" xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation"" xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" DisplayName=""Deployment Sequence"" sap:VirtualizedContainerService.HintSize=""399,1234"" mva:VisualBasic.Settings=""Assembly references and imported namespaces serialized as XML namespaces""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><ServerActivity sap:VirtualizedContainerService.HintSize=""377,1110"" InstanceSequenceNumber=""1"" ServerId=""1"" ServerName=""VSALM""><ActionActivity DisplayName=""Remove Web Site"" sap:VirtualizedContainerService.HintSize=""355,79"" IsSkipped=""False"" WorkflowActivityId=""d25617e8-7fac-4637-a7cb-2ad4da996156""><ActionActivity.StageActivity><mtrdm:StageActivity ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4010"" HasPendingSecurityChanges=""False"" Id=""26"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""d25617e8-7fac-4637-a7cb-2ad4da996156""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""-DropWebSite -sn &quot;__SiteName__&quot;"" CategoryId=""2005"" CategoryName=""IIS"" Command=""IISConfig.exe"" DeployerToolId=""2014"" DeploymentMethod=""2013"" Description=""Remove a web site (IIS 6.0+)"" HasPendingSecurityChanges=""False"" Id=""4010"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Remove Web Site"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID0"" Id=""10022"" Name=""SiteName"" Value="""" /></mtrdm:Component.ConfigurationVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID1"" Id=""10022"" Name=""SiteName"" Value=""FabrikamDev"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID1</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""355,97"" IsSkipped=""False"" WorkflowActivityId=""8160f49b-34c4-4860-aec4-b9fc4cf81e9b""><ActionActivity.StageActivity><mtrdm:StageActivity ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""27"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""8160f49b-34c4-4860-aec4-b9fc4cf81e9b""><mtrdm:StageActivity.Component><mtrdm:Component x:Name=""__ReferenceID20"" ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""&quot;__SourceFileFolder__&quot; &quot;__DestinationFileFolder__&quot;"" CategoryId=""1998"" CategoryName=""Windows OS"" Command=""irxcopy.cmd"" DeployerToolId=""12"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID2"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID3"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:Component.ConfigurationVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID4"" Id=""9950"" Name=""SourceFileFolder"" Value=""c:\FabrikamRM\WebSite\DEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID5"" Id=""9951"" Name=""DestinationFileFolder"" Value=""c:\FabrikamRM\Backup\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID4</x:Reference><x:Reference>__ReferenceID5</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ComponentActivity DisplayName=""Fabrikam Call Center"" sap:VirtualizedContainerService.HintSize=""355,51"" IsSkipped=""False"" WorkflowActivityId=""00152f01-0651-4221-9835-3394f1739a2f""><ComponentActivity.StageActivity><mtrdm:StageActivity ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4016"" HasPendingSecurityChanges=""False"" Id=""-416602"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""00152f01-0651-4221-9835-3394f1739a2f""><mtrdm:StageActivity.Component><mtrdm:ApplicationVersionComponent Id=""4016"" InMemoryStatus=""Existing"" Name=""Fabrikam Call Center"" OverrideRelativePathToPackageLocation=""""><mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID6"" Id=""10034"" Name=""Installation Path"" Value="""" /></mtrdm:ApplicationVersionComponent.ConfigurationVariables></mtrdm:ApplicationVersionComponent></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID7"" Id=""10034"" Name=""Installation Path"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ComponentActivity.StageActivity><ComponentActivity.StageActivityVariables><x:Reference>__ReferenceID7</x:Reference></ComponentActivity.StageActivityVariables></ComponentActivity><ActionActivity DisplayName=""Create Web Site"" sap:VirtualizedContainerService.HintSize=""355,169"" IsSkipped=""False"" WorkflowActivityId=""51c262e8-d1fe-4ff8-add6-879c2718616b""><ActionActivity.StageActivity><mtrdm:StageActivity ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4009"" HasPendingSecurityChanges=""False"" Id=""28"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""51c262e8-d1fe-4ff8-add6-879c2718616b""><mtrdm:StageActivity.Component><mtrdm:Component x:Name=""__ReferenceID23"" ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""-CreateWebSite -sn &quot;__SiteName__&quot; -port __PortNumber__ -pd &quot;__PhysicalPath__&quot; -ap &quot;__AppPoolName__&quot; -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__"" CategoryId=""2005"" CategoryName=""IIS"" Command=""IISConfig.exe"" DeployerToolId=""2014"" DeploymentMethod=""2013"" Description=""Create a web site (IIS 6.0+)"" HasPendingSecurityChanges=""False"" Id=""4009"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Create Web Site"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID8"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID9"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID10"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID11"" Id=""10019"" Name=""SiteName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID12"" Id=""10020"" Name=""PortNumber"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID13"" Id=""10021"" Name=""PhysicalPath"" Value="""" /></mtrdm:Component.ConfigurationVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID14"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID15"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID16"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID17"" Id=""10019"" Name=""SiteName"" Value=""FabrikamDEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID18"" Id=""10020"" Name=""PortNumber"" Value=""8000"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID19"" Id=""10021"" Name=""PhysicalPath"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID14</x:Reference><x:Reference>__ReferenceID15</x:Reference><x:Reference>__ReferenceID16</x:Reference><x:Reference>__ReferenceID17</x:Reference><x:Reference>__ReferenceID18</x:Reference><x:Reference>__ReferenceID19</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><RollbackActivity DisplayName=""Rollback"" sap:VirtualizedContainerService.HintSize=""355,430""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""333,97"" IsSkipped=""False"" WorkflowActivityId=""e7783400-9b05-4970-8777-6ab92c78d155""><ActionActivity.StageActivity><mtrdm:StageActivity Component=""{x:Reference __ReferenceID20}"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""29"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""0"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""e7783400-9b05-4970-8777-6ab92c78d155""><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID21"" Id=""9950"" Name=""SourceFileFolder"" Value=""c:\FabrikamRM\Backup\DEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID22"" Id=""9951"" Name=""DestinationFileFolder"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID21</x:Reference><x:Reference>__ReferenceID22</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ActionActivity DisplayName=""Create Web Site"" sap:VirtualizedContainerService.HintSize=""333,169"" IsSkipped=""False"" WorkflowActivityId=""a657d65d-f4d6-42f8-aa1f-7ac638496557""><ActionActivity.StageActivity><mtrdm:StageActivity Component=""{x:Reference __ReferenceID23}"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4009"" HasPendingSecurityChanges=""False"" Id=""30"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""0"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""a657d65d-f4d6-42f8-aa1f-7ac638496557""><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID24"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID25"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID26"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID27"" Id=""10019"" Name=""SiteName"" Value=""FabrikamDEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID28"" Id=""10020"" Name=""PortNumber"" Value=""8000"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID29"" Id=""10021"" Name=""PhysicalPath"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID24</x:Reference><x:Reference>__ReferenceID25</x:Reference><x:Reference>__ReferenceID26</x:Reference><x:Reference>__ReferenceID27</x:Reference><x:Reference>__ReferenceID28</x:Reference><x:Reference>__ReferenceID29</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity></RollbackActivity></ServerActivity></DeploymentSequenceActivity>";

            var parsedTemplate =
                (new RMDeploymentSequence { StageId = 100, WorkflowXaml = releaseTemplate }).ToReleaseTemplate();
            scriptGenerator.ScriptGenerationNotification += PrintEvents;
            await scriptGenerator.GenerateScriptAsync(parsedTemplate, path);
        }

        [TestMethod]
        public async Task FabrikamFiber_From_BKVM_With_Component_In_Rollback()
        {
            var fakeFs = new FakeFileSystem();
            var componentRepo = new FakeComponentRepo();
            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4010, 
                        WorkflowActivityId = Guid.Parse("d25617e8-7fac-4637-a7cb-2ad4da996156"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"-DropWebSite -sn ""__SiteName__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                "FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4016, 
                        WorkflowActivityId = Guid.Parse("00152F01-0651-4221-9835-3394F1739A2F"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"*.* ""__Installation Path__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "Installation Path", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4016, 
                        WorkflowActivityId = Guid.Parse("8FCB1DCB-3EC0-4678-8F7D-25D727A26011"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"*.* ""__Installation Path__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "Installation Path", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\Rollback", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 3981, 
                        WorkflowActivityId = Guid.Parse("E7783400-9B05-4970-8777-6AB92C78D155"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"""__SourceFileFolder__"" ""__DestinationFileFolder__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SourceFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\Backup\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "DestinationFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4009, 
                        WorkflowActivityId = Guid.Parse("A657D65D-F4D6-42F8-AA1F-7AC638496557"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments =
                            @"-CreateWebSite -sn ""__SiteName__"" -port __PortNumber__ -pd ""__PhysicalPath__"" -ap ""__AppPoolName__"" -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "AppPoolName", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsPreloadEnabled", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsAutoStart", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                @"FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PortNumber", 
                                            Value = @"8000", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PhysicalPath", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4009, 
                        WorkflowActivityId = Guid.Parse("51C262E8-D1FE-4FF8-ADD6-879C2718616B"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments =
                            @"-CreateWebSite -sn ""__SiteName__"" -port __PortNumber__ -pd ""__PhysicalPath__"" -ap ""__AppPoolName__"" -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "AppPoolName", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsPreloadEnabled", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsAutoStart", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                @"FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PortNumber", 
                                            Value = @"8000", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PhysicalPath", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 3981, 
                        WorkflowActivityId = Guid.Parse("8160F49B-34C4-4860-AEC4-B9FC4CF81E9B"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"""__SourceFileFolder__"" ""__DestinationFileFolder__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SourceFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "DestinationFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\Backup\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            var fakeUserRepo = new FakeUserRepo();
            var fakeDeployerToolRepo = new FakeDeployerToolRepo();

            var scriptGenerator = new ScriptGenerator(fakeFs, componentRepo, fakeUserRepo, fakeDeployerToolRepo);
            var path = @"C:\RMWorkflow";
            var releaseTemplate =
                @"<DeploymentSequenceActivity xmlns=""clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow"" xmlns:mtrdm=""clr-namespace:Microsoft.TeamFoundation.Release.Data.Model;assembly=Microsoft.TeamFoundation.Release.Data"" xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities"" xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation"" xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" DisplayName=""Deployment Sequence"" sap:VirtualizedContainerService.HintSize=""401.6,1345.6"" mva:VisualBasic.Settings=""Assembly references and imported namespaces serialized as XML namespaces""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><ServerActivity sap:VirtualizedContainerService.HintSize=""379.2,1220.8"" InstanceSequenceNumber=""1"" ServerId=""1"" ServerName=""VSALM""><ActionActivity DisplayName=""Remove Web Site"" sap:VirtualizedContainerService.HintSize=""356.8,81.6"" IsSkipped=""False"" WorkflowActivityId=""d25617e8-7fac-4637-a7cb-2ad4da996156""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Remove Web Site"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4010"" HasPendingSecurityChanges=""False"" Id=""98"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""d25617e8-7fac-4637-a7cb-2ad4da996156""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""-DropWebSite -sn &quot;__SiteName__&quot;"" CategoryId=""2005"" CategoryName=""IIS"" Command=""IISConfig.exe"" DeployerToolId=""2014"" DeploymentMethod=""2013"" Description=""Remove a web site (IIS 6.0+)"" HasPendingSecurityChanges=""False"" Id=""4010"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Remove Web Site"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID0"" Id=""10022"" Name=""SiteName"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID1"" Id=""10022"" Name=""SiteName"" Value=""FabrikamDev"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID1</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""356.8,99.2"" IsSkipped=""False"" WorkflowActivityId=""8160f49b-34c4-4860-aec4-b9fc4cf81e9b""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""99"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""8160f49b-34c4-4860-aec4-b9fc4cf81e9b""><mtrdm:StageActivity.Component><mtrdm:Component x:Name=""__ReferenceID22"" ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""&quot;__SourceFileFolder__&quot; &quot;__DestinationFileFolder__&quot;"" CategoryId=""1998"" CategoryName=""Windows OS"" Command=""irxcopy.cmd"" DeployerToolId=""12"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID2"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID3"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID4"" Id=""9950"" Name=""SourceFileFolder"" Value=""c:\FabrikamRM\WebSite\DEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID5"" Id=""9951"" Name=""DestinationFileFolder"" Value=""c:\FabrikamRM\Backup\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID4</x:Reference><x:Reference>__ReferenceID5</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ComponentActivity DisplayName=""Fabrikam Call Center"" sap:VirtualizedContainerService.HintSize=""356.8,52.8"" IsSkipped=""False"" WorkflowActivityId=""00152f01-0651-4221-9835-3394f1739a2f""><ComponentActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Fabrikam Call Center"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4016"" HasPendingSecurityChanges=""False"" Id=""103"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""00152f01-0651-4221-9835-3394f1739a2f""><mtrdm:StageActivity.Component><mtrdm:ApplicationVersionComponent Id=""4016"" InMemoryStatus=""Existing"" Name=""Fabrikam Call Center"" OverrideRelativePathToPackageLocation=""""><mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID6"" Id=""10034"" Name=""Installation Path"" Value="""" /></mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ApplicationVersionComponent.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:ApplicationVersionComponent.PropertyBagVariables></mtrdm:ApplicationVersionComponent></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID7"" Id=""10034"" Name=""Installation Path"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ComponentActivity.StageActivity><ComponentActivity.StageActivityVariables><x:Reference>__ReferenceID7</x:Reference></ComponentActivity.StageActivityVariables></ComponentActivity><ActionActivity DisplayName=""Create Web Site"" sap:VirtualizedContainerService.HintSize=""356.8,172.8"" IsSkipped=""False"" WorkflowActivityId=""51c262e8-d1fe-4ff8-add6-879c2718616b""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Create Web Site"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4009"" HasPendingSecurityChanges=""False"" Id=""100"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""51c262e8-d1fe-4ff8-add6-879c2718616b""><mtrdm:StageActivity.Component><mtrdm:Component x:Name=""__ReferenceID25"" ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""-CreateWebSite -sn &quot;__SiteName__&quot; -port __PortNumber__ -pd &quot;__PhysicalPath__&quot; -ap &quot;__AppPoolName__&quot; -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__"" CategoryId=""2005"" CategoryName=""IIS"" Command=""IISConfig.exe"" DeployerToolId=""2014"" DeploymentMethod=""2013"" Description=""Create a web site (IIS 6.0+)"" HasPendingSecurityChanges=""False"" Id=""4009"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Create Web Site"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID8"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID9"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID10"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID11"" Id=""10019"" Name=""SiteName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID12"" Id=""10020"" Name=""PortNumber"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID13"" Id=""10021"" Name=""PhysicalPath"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID14"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID15"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID16"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID17"" Id=""10019"" Name=""SiteName"" Value=""FabrikamDEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID18"" Id=""10020"" Name=""PortNumber"" Value=""8000"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID19"" Id=""10021"" Name=""PhysicalPath"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID14</x:Reference><x:Reference>__ReferenceID15</x:Reference><x:Reference>__ReferenceID16</x:Reference><x:Reference>__ReferenceID17</x:Reference><x:Reference>__ReferenceID18</x:Reference><x:Reference>__ReferenceID19</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><RollbackActivity DisplayName=""Rollback"" sap:VirtualizedContainerService.HintSize=""356.8,529.6""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><ComponentActivity DisplayName=""Fabrikam Call Center"" sap:VirtualizedContainerService.HintSize=""334.4,52.8"" IsSkipped=""False"" WorkflowActivityId=""8fcb1dcb-3ec0-4678-8f7d-25d727a26011""><ComponentActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Fabrikam Call Center"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4016"" HasPendingSecurityChanges=""False"" Id=""-176675"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""8fcb1dcb-3ec0-4678-8f7d-25d727a26011""><mtrdm:StageActivity.Component><mtrdm:ApplicationVersionComponent Id=""4016"" InMemoryStatus=""Existing"" Name=""Fabrikam Call Center"" OverrideRelativePathToPackageLocation=""""><mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID20"" Id=""10034"" Name=""Installation Path"" Value="""" /></mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ApplicationVersionComponent.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:ApplicationVersionComponent.PropertyBagVariables></mtrdm:ApplicationVersionComponent></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID21"" Id=""10034"" Name=""Installation Path"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ComponentActivity.StageActivity><ComponentActivity.StageActivityVariables><x:Reference>__ReferenceID21</x:Reference></ComponentActivity.StageActivityVariables></ComponentActivity><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""334.4,99.2"" IsSkipped=""False"" WorkflowActivityId=""e7783400-9b05-4970-8777-6ab92c78d155""><ActionActivity.StageActivity><mtrdm:StageActivity Component=""{x:Reference __ReferenceID22}"" ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""101"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""0"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""e7783400-9b05-4970-8777-6ab92c78d155""><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID23"" Id=""9950"" Name=""SourceFileFolder"" Value=""c:\FabrikamRM\Backup\DEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID24"" Id=""9951"" Name=""DestinationFileFolder"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID23</x:Reference><x:Reference>__ReferenceID24</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ActionActivity DisplayName=""Create Web Site"" sap:VirtualizedContainerService.HintSize=""334.4,172.8"" IsSkipped=""False"" WorkflowActivityId=""a657d65d-f4d6-42f8-aa1f-7ac638496557""><ActionActivity.StageActivity><mtrdm:StageActivity Component=""{x:Reference __ReferenceID25}"" ActivityDisplayName=""Create Web Site"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4009"" HasPendingSecurityChanges=""False"" Id=""102"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""0"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""a657d65d-f4d6-42f8-aa1f-7ac638496557""><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID26"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID27"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID28"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID29"" Id=""10019"" Name=""SiteName"" Value=""FabrikamDEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID30"" Id=""10020"" Name=""PortNumber"" Value=""8000"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID31"" Id=""10021"" Name=""PhysicalPath"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID26</x:Reference><x:Reference>__ReferenceID27</x:Reference><x:Reference>__ReferenceID28</x:Reference><x:Reference>__ReferenceID29</x:Reference><x:Reference>__ReferenceID30</x:Reference><x:Reference>__ReferenceID31</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity></RollbackActivity></ServerActivity></DeploymentSequenceActivity>";

            var parsedTemplate =
                (new RMDeploymentSequence { StageId = 100, WorkflowXaml = releaseTemplate }).ToReleaseTemplate();
            scriptGenerator.ScriptGenerationNotification += PrintEvents;
            await scriptGenerator.GenerateScriptAsync(parsedTemplate, path);
        }

        [TestMethod]
        public async Task FabrikamFiber_From_BKVM_With_Configuration_Variables()
        {
            var fakeFs = new FakeFileSystem();
            var componentRepo = new FakeComponentRepo();
            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4010, 
                        WorkflowActivityId = Guid.Parse("d25617e8-7fac-4637-a7cb-2ad4da996156"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"-DropWebSite -sn ""__SiteName__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                "FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4016, 
                        WorkflowActivityId = Guid.Parse("00152F01-0651-4221-9835-3394F1739A2F"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"*.* ""__Installation Path__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "Installation Path", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4017, 
                        WorkflowActivityId = Guid.Parse("41BF9071-F439-4046-AB42-F3A1184237F3"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = "*.configBeforeAfter", 
                        Arguments = @"*.* ""__Installation Path__""", 
                        VariableReplacementMethod = VariableReplacementMethod.BeforeAndAfterInstallation, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "Installation Path", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "BeforeAfter_1", 
                                            Value =
                                                @"BeforeAfter 1", 
                                            IsParameter =
                                                false
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "BeforeAfter 2 Encrypted", 
                                            Value =
                                                @"Muyy7/eWpqb8UZ4wZc3fN23pu7dvdTpAY0H+zD3Vd0S+HTL4vQHqbnSe3w616qnJ0y9rxRdkVaZiX8ydhUI/snwsdlUGk/070dz3Kjbwb7twzObCXcY4EnjgB7SDr9iu650TUzt5MSM8VbK723c0kS+jWK5g/EEefJwpdiZgoYI=", 
                                            IsParameter =
                                                false, 
                                            Encrypted = true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4018, 
                        WorkflowActivityId = Guid.Parse("2718F66A-E5C0-4FE2-A90D-61B550E3A6C1"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = "*.configBefore", 
                        Arguments = @"*.* ""__Installation Path__""", 
                        VariableReplacementMethod = VariableReplacementMethod.BeforeInstallation, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "Installation Path", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "Before_1", 
                                            Value =
                                                @"Before 1", 
                                            IsParameter =
                                                false
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "Before 2 Encrypted", 
                                            Value =
                                                @"Jlx0czDQZOU3OGtck915VE5rNvzNvwzu0VfyBgg5QwQ4gjHyyDBae3j5w0chqjYCtX4l+ruzKgYWxXe+UFTbw1vbZH5u1fMsKG95nw0DN6WH9+GQwhp8Ab15D2ZbGyrMLf9eTcI9Qxlw6QdAJl8SM5ZkkX8g64lq/Vd/MV+akR8=", 
                                            IsParameter =
                                                false, 
                                            Encrypted = true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4019, 
                        WorkflowActivityId = Guid.Parse("2B607E62-6ED8-4358-91CC-EA8BFD751AC6"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = "*.configAfter", 
                        Arguments = @"*.* ""__Installation Path__""", 
                        VariableReplacementMethod = VariableReplacementMethod.AfterInstallation, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "Installation Path", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "After_1", 
                                            Value =
                                                @"After 1", 
                                            IsParameter =
                                                false
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "After 2 Encrypted", 
                                            Value =
                                                @"Bj8P2jC2DYyO/8dTJrecmWJM+BUlr0TeyYV1qYbM/izIXMqHeINxdOND2wDyowLEkn4dezqfiEhDPVubeTeZriQhqDTI/6u9aol9RR4cnoLJC9n7ykgWjMCPDqDPQpjig3w/snecAmYJWdnwBDZezfe106X9Uv7qI+QCyzdqTIc=", 
                                            IsParameter =
                                                false, 
                                            Encrypted = true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 3981, 
                        WorkflowActivityId = Guid.Parse("E7783400-9B05-4970-8777-6AB92C78D155"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"""__SourceFileFolder__"" ""__DestinationFileFolder__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SourceFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\Backup\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "DestinationFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4009, 
                        WorkflowActivityId = Guid.Parse("A657D65D-F4D6-42F8-AA1F-7AC638496557"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments =
                            @"-CreateWebSite -sn ""__SiteName__"" -port __PortNumber__ -pd ""__PhysicalPath__"" -ap ""__AppPoolName__"" -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "AppPoolName", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsPreloadEnabled", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsAutoStart", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                @"FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PortNumber", 
                                            Value = @"8000", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PhysicalPath", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4009, 
                        WorkflowActivityId = Guid.Parse("51C262E8-D1FE-4FF8-ADD6-879C2718616B"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments =
                            @"-CreateWebSite -sn ""__SiteName__"" -port __PortNumber__ -pd ""__PhysicalPath__"" -ap ""__AppPoolName__"" -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "AppPoolName", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsPreloadEnabled", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsAutoStart", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                @"FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PortNumber", 
                                            Value = @"8000", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PhysicalPath", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 3981, 
                        WorkflowActivityId = Guid.Parse("8160F49B-34C4-4860-AEC4-B9FC4CF81E9B"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"""__SourceFileFolder__"" ""__DestinationFileFolder__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SourceFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "DestinationFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\Backup\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            var fakeUserRepo = new FakeUserRepo();
            var fakeDeployerToolRepo = new FakeDeployerToolRepo();

            var scriptGenerator = new ScriptGenerator(fakeFs, componentRepo, fakeUserRepo, fakeDeployerToolRepo);

            var path = @"C:\RMWorkflow";
            var releaseTemplate =
                @"<DeploymentSequenceActivity xmlns=""clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow"" xmlns:mtrdm=""clr-namespace:Microsoft.TeamFoundation.Release.Data.Model;assembly=Microsoft.TeamFoundation.Release.Data"" xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities"" xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation"" xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" DisplayName=""Deployment Sequence"" sap:VirtualizedContainerService.HintSize=""401.6,1531.2"" mva:VisualBasic.Settings=""Assembly references and imported namespaces serialized as XML namespaces""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><ServerActivity sap:VirtualizedContainerService.HintSize=""379.2,1406.4"" InstanceSequenceNumber=""1"" ServerId=""1"" ServerName=""VSALM""><ActionActivity DisplayName=""Remove Web Site"" sap:VirtualizedContainerService.HintSize=""356.8,81.6"" IsSkipped=""False"" WorkflowActivityId=""d25617e8-7fac-4637-a7cb-2ad4da996156""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Remove Web Site"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4010"" HasPendingSecurityChanges=""False"" Id=""41"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""d25617e8-7fac-4637-a7cb-2ad4da996156""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""-DropWebSite -sn &quot;__SiteName__&quot;"" CategoryId=""2005"" CategoryName=""IIS"" Command=""IISConfig.exe"" DeployerToolId=""2014"" DeploymentMethod=""2013"" Description=""Remove a web site (IIS 6.0+)"" HasPendingSecurityChanges=""False"" Id=""4010"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Remove Web Site"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID0"" Id=""10022"" Name=""SiteName"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID1"" Id=""10022"" Name=""SiteName"" Value=""FabrikamDev"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID1</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""356.8,99.2"" IsSkipped=""False"" WorkflowActivityId=""8160f49b-34c4-4860-aec4-b9fc4cf81e9b""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""42"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""8160f49b-34c4-4860-aec4-b9fc4cf81e9b""><mtrdm:StageActivity.Component><mtrdm:Component x:Name=""__ReferenceID38"" ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""&quot;__SourceFileFolder__&quot; &quot;__DestinationFileFolder__&quot;"" CategoryId=""1998"" CategoryName=""Windows OS"" Command=""irxcopy.cmd"" DeployerToolId=""12"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID2"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID3"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID4"" Id=""9950"" Name=""SourceFileFolder"" Value=""c:\FabrikamRM\WebSite\DEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID5"" Id=""9951"" Name=""DestinationFileFolder"" Value=""c:\FabrikamRM\Backup\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID4</x:Reference><x:Reference>__ReferenceID5</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ComponentActivity DisplayName=""Fabrikam Call Center"" sap:VirtualizedContainerService.HintSize=""356.8,52.8"" IsSkipped=""True"" WorkflowActivityId=""00152f01-0651-4221-9835-3394f1739a2f""><ComponentActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Fabrikam Call Center"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4016"" HasPendingSecurityChanges=""False"" Id=""46"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""00152f01-0651-4221-9835-3394f1739a2f""><mtrdm:StageActivity.Component><mtrdm:ApplicationVersionComponent Id=""4016"" InMemoryStatus=""Existing"" Name=""Fabrikam Call Center"" OverrideRelativePathToPackageLocation=""""><mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID6"" Id=""10034"" Name=""Installation Path"" Value="""" /></mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ApplicationVersionComponent.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:ApplicationVersionComponent.PropertyBagVariables></mtrdm:ApplicationVersionComponent></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID7"" Id=""10034"" Name=""Installation Path"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ComponentActivity.StageActivity><ComponentActivity.StageActivityVariables><x:Reference>__ReferenceID7</x:Reference></ComponentActivity.StageActivityVariables></ComponentActivity><ComponentActivity DisplayName=""Fabrikam Call Center After"" sap:VirtualizedContainerService.HintSize=""356.8,52.8"" IsSkipped=""False"" WorkflowActivityId=""2b607e62-6ed8-4358-91cc-ea8bfd751ac6""><ComponentActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Fabrikam Call Center After"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4019"" HasPendingSecurityChanges=""False"" Id=""-993475"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""2b607e62-6ed8-4358-91cc-ea8bfd751ac6""><mtrdm:StageActivity.Component><mtrdm:ApplicationVersionComponent OverrideRelativePathToPackageLocation=""{x:Null}"" Id=""4019"" InMemoryStatus=""New"" Name=""Fabrikam Call Center After""><mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID8"" Id=""10041"" Name=""Installation Path"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID9"" Id=""10042"" Name=""After_1"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID10"" Id=""10043"" Name=""After 2 Encrypted"" Value="""" /></mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ApplicationVersionComponent.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:ApplicationVersionComponent.PropertyBagVariables></mtrdm:ApplicationVersionComponent></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID11"" Id=""10041"" Name=""Installation Path"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID12"" Id=""10042"" Name=""After_1"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID13"" Id=""10043"" Name=""After 2 Encrypted"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ComponentActivity.StageActivity><ComponentActivity.StageActivityVariables><x:Reference>__ReferenceID11</x:Reference><x:Reference>__ReferenceID12</x:Reference><x:Reference>__ReferenceID13</x:Reference></ComponentActivity.StageActivityVariables></ComponentActivity><ComponentActivity DisplayName=""Fabrikam Call Center Before"" sap:VirtualizedContainerService.HintSize=""356.8,52.8"" IsSkipped=""False"" WorkflowActivityId=""2718f66a-e5c0-4fe2-a90d-61b550e3a6c1""><ComponentActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Fabrikam Call Center Before"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4018"" HasPendingSecurityChanges=""False"" Id=""-76941"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""2718f66a-e5c0-4fe2-a90d-61b550e3a6c1""><mtrdm:StageActivity.Component><mtrdm:ApplicationVersionComponent OverrideRelativePathToPackageLocation=""{x:Null}"" Id=""4018"" InMemoryStatus=""New"" Name=""Fabrikam Call Center Before""><mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID14"" Id=""10038"" Name=""Installation Path"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID15"" Id=""10039"" Name=""Before_1"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID16"" Id=""10040"" Name=""Before 2 Encrypted"" Value="""" /></mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ApplicationVersionComponent.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:ApplicationVersionComponent.PropertyBagVariables></mtrdm:ApplicationVersionComponent></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID17"" Id=""10038"" Name=""Installation Path"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID18"" Id=""10039"" Name=""Before_1"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID19"" Id=""10040"" Name=""Before 2 Encrypted"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ComponentActivity.StageActivity><ComponentActivity.StageActivityVariables><x:Reference>__ReferenceID17</x:Reference><x:Reference>__ReferenceID18</x:Reference><x:Reference>__ReferenceID19</x:Reference></ComponentActivity.StageActivityVariables></ComponentActivity><ComponentActivity DisplayName=""Fabrikam Call Center BeforeAfter"" sap:VirtualizedContainerService.HintSize=""356.8,52.8"" IsSkipped=""False"" WorkflowActivityId=""41bf9071-f439-4046-ab42-f3a1184237f3""><ComponentActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Fabrikam Call Center BeforeAfter"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4017"" HasPendingSecurityChanges=""False"" Id=""-480684"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""41bf9071-f439-4046-ab42-f3a1184237f3""><mtrdm:StageActivity.Component><mtrdm:ApplicationVersionComponent OverrideRelativePathToPackageLocation=""{x:Null}"" Id=""4017"" InMemoryStatus=""New"" Name=""Fabrikam Call Center BeforeAfter""><mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID20"" Id=""10035"" Name=""Installation Path"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID21"" Id=""10036"" Name=""BeforeAfter_1"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID22"" Id=""10037"" Name=""BeforeAfter 2 Encrypted"" Value="""" /></mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ApplicationVersionComponent.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:ApplicationVersionComponent.PropertyBagVariables></mtrdm:ApplicationVersionComponent></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID23"" Id=""10035"" Name=""Installation Path"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID24"" Id=""10036"" Name=""BeforeAfter_1"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID25"" Id=""10037"" Name=""BeforeAfter 2 Encrypted"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ComponentActivity.StageActivity><ComponentActivity.StageActivityVariables><x:Reference>__ReferenceID23</x:Reference><x:Reference>__ReferenceID24</x:Reference><x:Reference>__ReferenceID25</x:Reference></ComponentActivity.StageActivityVariables></ComponentActivity><ActionActivity DisplayName=""Create Web Site"" sap:VirtualizedContainerService.HintSize=""356.8,172.8"" IsSkipped=""False"" WorkflowActivityId=""51c262e8-d1fe-4ff8-add6-879c2718616b""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Create Web Site"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4009"" HasPendingSecurityChanges=""False"" Id=""43"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""51c262e8-d1fe-4ff8-add6-879c2718616b""><mtrdm:StageActivity.Component><mtrdm:Component x:Name=""__ReferenceID41"" ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""-CreateWebSite -sn &quot;__SiteName__&quot; -port __PortNumber__ -pd &quot;__PhysicalPath__&quot; -ap &quot;__AppPoolName__&quot; -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__"" CategoryId=""2005"" CategoryName=""IIS"" Command=""IISConfig.exe"" DeployerToolId=""2014"" DeploymentMethod=""2013"" Description=""Create a web site (IIS 6.0+)"" HasPendingSecurityChanges=""False"" Id=""4009"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Create Web Site"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID26"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID27"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID28"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID29"" Id=""10019"" Name=""SiteName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID30"" Id=""10020"" Name=""PortNumber"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID31"" Id=""10021"" Name=""PhysicalPath"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID32"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID33"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID34"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID35"" Id=""10019"" Name=""SiteName"" Value=""FabrikamDEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID36"" Id=""10020"" Name=""PortNumber"" Value=""8000"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID37"" Id=""10021"" Name=""PhysicalPath"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID32</x:Reference><x:Reference>__ReferenceID33</x:Reference><x:Reference>__ReferenceID34</x:Reference><x:Reference>__ReferenceID35</x:Reference><x:Reference>__ReferenceID36</x:Reference><x:Reference>__ReferenceID37</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><RollbackActivity DisplayName=""Rollback"" sap:VirtualizedContainerService.HintSize=""356.8,436.8""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""334.4,99.2"" IsSkipped=""False"" WorkflowActivityId=""e7783400-9b05-4970-8777-6ab92c78d155""><ActionActivity.StageActivity><mtrdm:StageActivity Component=""{x:Reference __ReferenceID38}"" ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""44"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""0"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""e7783400-9b05-4970-8777-6ab92c78d155""><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID39"" Id=""9950"" Name=""SourceFileFolder"" Value=""c:\FabrikamRM\Backup\DEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID40"" Id=""9951"" Name=""DestinationFileFolder"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID39</x:Reference><x:Reference>__ReferenceID40</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ActionActivity DisplayName=""Create Web Site"" sap:VirtualizedContainerService.HintSize=""334.4,172.8"" IsSkipped=""False"" WorkflowActivityId=""a657d65d-f4d6-42f8-aa1f-7ac638496557""><ActionActivity.StageActivity><mtrdm:StageActivity Component=""{x:Reference __ReferenceID41}"" ActivityDisplayName=""Create Web Site"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4009"" HasPendingSecurityChanges=""False"" Id=""45"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""0"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""a657d65d-f4d6-42f8-aa1f-7ac638496557""><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID42"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID43"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID44"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID45"" Id=""10019"" Name=""SiteName"" Value=""FabrikamDEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID46"" Id=""10020"" Name=""PortNumber"" Value=""8000"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID47"" Id=""10021"" Name=""PhysicalPath"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID42</x:Reference><x:Reference>__ReferenceID43</x:Reference><x:Reference>__ReferenceID44</x:Reference><x:Reference>__ReferenceID45</x:Reference><x:Reference>__ReferenceID46</x:Reference><x:Reference>__ReferenceID47</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity></RollbackActivity></ServerActivity></DeploymentSequenceActivity>";

            var parsedTemplate =
                (new RMDeploymentSequence { StageId = 100, WorkflowXaml = releaseTemplate }).ToReleaseTemplate();
            scriptGenerator.ScriptGenerationNotification += PrintEvents;
            await scriptGenerator.GenerateScriptAsync(parsedTemplate, path);
        }

        [TestMethod]
        public async Task FabrikamFiber_From_BKVM_With_Manual_Interventions()
        {
            var fakeFs = new FakeFileSystem();
            var componentRepo = new FakeComponentRepo();

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4010, 
                        WorkflowActivityId = Guid.Parse("d25617e8-7fac-4637-a7cb-2ad4da996156"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"-DropWebSite -sn ""__SiteName__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                "FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4016, 
                        WorkflowActivityId = Guid.Parse("00152F01-0651-4221-9835-3394F1739A2F"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"*.* ""__Installation Path__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "Installation Path", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 3981, 
                        WorkflowActivityId = Guid.Parse("E7783400-9B05-4970-8777-6AB92C78D155"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"""__SourceFileFolder__"" ""__DestinationFileFolder__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SourceFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\Backup\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "DestinationFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4009, 
                        WorkflowActivityId = Guid.Parse("A657D65D-F4D6-42F8-AA1F-7AC638496557"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments =
                            @"-CreateWebSite -sn ""__SiteName__"" -port __PortNumber__ -pd ""__PhysicalPath__"" -ap ""__AppPoolName__"" -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "AppPoolName", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsPreloadEnabled", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsAutoStart", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                @"FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PortNumber", 
                                            Value = @"8000", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PhysicalPath", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4009, 
                        WorkflowActivityId = Guid.Parse("51C262E8-D1FE-4FF8-ADD6-879C2718616B"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments =
                            @"-CreateWebSite -sn ""__SiteName__"" -port __PortNumber__ -pd ""__PhysicalPath__"" -ap ""__AppPoolName__"" -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "AppPoolName", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsPreloadEnabled", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsAutoStart", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                @"FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PortNumber", 
                                            Value = @"8000", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PhysicalPath", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 3981, 
                        WorkflowActivityId = Guid.Parse("8160F49B-34C4-4860-AEC4-B9FC4CF81E9B"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"""__SourceFileFolder__"" ""__DestinationFileFolder__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SourceFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "DestinationFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\Backup\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            var fakeUserRepo = new FakeUserRepo
                                 {
                                     Groups =
                                         new HashSet<RMGroup> { new RMGroup { Id = 1, Name = "Ops Team" } }, 
                                     Users =
                                         new HashSet<RMUser>
                                             {
                                                 new RMUser
                                                     {
                                                         Id = 9006, 
                                                         Name = "Daniel Mann"
                                                     }
                                             }
                                 };

            var fakeDeployerToolRepo = new FakeDeployerToolRepo();

            var scriptGenerator = new ScriptGenerator(fakeFs, componentRepo, fakeUserRepo, fakeDeployerToolRepo);
            var path = @"C:\RMWorkflow";
            var releaseTemplate =
                @"<DeploymentSequenceActivity xmlns=""clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow"" xmlns:mtrdm=""clr-namespace:Microsoft.TeamFoundation.Release.Data.Model;assembly=Microsoft.TeamFoundation.Release.Data"" xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities"" xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation"" xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" DisplayName=""Deployment Sequence"" sap:VirtualizedContainerService.HintSize=""401.6,1806.4"" mva:VisualBasic.Settings=""Assembly references and imported namespaces serialized as XML namespaces""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><ServerActivity sap:VirtualizedContainerService.HintSize=""379.2,1681.6"" InstanceSequenceNumber=""1"" ServerId=""1"" ServerName=""VSALM""><ManualInterventionActivity Details=""Ops team test&#xD;&#xA;Linebreak&#xD;&#xA;Another linebreak"" DisplayName=""Manual Intervention"" sap:VirtualizedContainerService.HintSize=""356.8,236.8"" Recipient=""2:1"" WorkflowActivityId=""e5533b50-60ca-4b9f-b4f3-64e772885bbe"" /><ActionActivity DisplayName=""Remove Web Site"" sap:VirtualizedContainerService.HintSize=""356.8,81.6"" IsSkipped=""False"" WorkflowActivityId=""d25617e8-7fac-4637-a7cb-2ad4da996156""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Remove Web Site"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4010"" HasPendingSecurityChanges=""False"" Id=""59"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""d25617e8-7fac-4637-a7cb-2ad4da996156""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""-DropWebSite -sn &quot;__SiteName__&quot;"" CategoryId=""2005"" CategoryName=""IIS"" Command=""IISConfig.exe"" DeployerToolId=""2014"" DeploymentMethod=""2013"" Description=""Remove a web site (IIS 6.0+)"" HasPendingSecurityChanges=""False"" Id=""4010"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Remove Web Site"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID0"" Id=""10022"" Name=""SiteName"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID1"" Id=""10022"" Name=""SiteName"" Value=""FabrikamDev"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID1</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ManualInterventionActivity Details=""Single user test&#xD;&#xA;Linebreak&#xD;&#xA;Another linebreak"" DisplayName=""Manual Intervention"" sap:VirtualizedContainerService.HintSize=""356.8,236.8"" Recipient=""1:9006"" WorkflowActivityId=""90fcd753-621a-4a81-9b95-5270e04f264d"" /><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""356.8,99.2"" IsSkipped=""False"" WorkflowActivityId=""8160f49b-34c4-4860-aec4-b9fc4cf81e9b""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""60"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""8160f49b-34c4-4860-aec4-b9fc4cf81e9b""><mtrdm:StageActivity.Component><mtrdm:Component x:Name=""__ReferenceID20"" ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""&quot;__SourceFileFolder__&quot; &quot;__DestinationFileFolder__&quot;"" CategoryId=""1998"" CategoryName=""Windows OS"" Command=""irxcopy.cmd"" DeployerToolId=""12"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID2"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID3"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID4"" Id=""9950"" Name=""SourceFileFolder"" Value=""c:\FabrikamRM\WebSite\DEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID5"" Id=""9951"" Name=""DestinationFileFolder"" Value=""c:\FabrikamRM\Backup\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID4</x:Reference><x:Reference>__ReferenceID5</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ComponentActivity DisplayName=""Fabrikam Call Center"" sap:VirtualizedContainerService.HintSize=""356.8,52.8"" IsSkipped=""False"" WorkflowActivityId=""00152f01-0651-4221-9835-3394f1739a2f""><ComponentActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Fabrikam Call Center"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4016"" HasPendingSecurityChanges=""False"" Id=""64"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""00152f01-0651-4221-9835-3394f1739a2f""><mtrdm:StageActivity.Component><mtrdm:ApplicationVersionComponent Id=""4016"" InMemoryStatus=""Existing"" Name=""Fabrikam Call Center"" OverrideRelativePathToPackageLocation=""""><mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID6"" Id=""10034"" Name=""Installation Path"" Value="""" /></mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ApplicationVersionComponent.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:ApplicationVersionComponent.PropertyBagVariables></mtrdm:ApplicationVersionComponent></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID7"" Id=""10034"" Name=""Installation Path"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ComponentActivity.StageActivity><ComponentActivity.StageActivityVariables><x:Reference>__ReferenceID7</x:Reference></ComponentActivity.StageActivityVariables></ComponentActivity><ActionActivity DisplayName=""Create Web Site"" sap:VirtualizedContainerService.HintSize=""356.8,172.8"" IsSkipped=""False"" WorkflowActivityId=""51c262e8-d1fe-4ff8-add6-879c2718616b""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Create Web Site"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4009"" HasPendingSecurityChanges=""False"" Id=""61"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""51c262e8-d1fe-4ff8-add6-879c2718616b""><mtrdm:StageActivity.Component><mtrdm:Component x:Name=""__ReferenceID23"" ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""-CreateWebSite -sn &quot;__SiteName__&quot; -port __PortNumber__ -pd &quot;__PhysicalPath__&quot; -ap &quot;__AppPoolName__&quot; -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__"" CategoryId=""2005"" CategoryName=""IIS"" Command=""IISConfig.exe"" DeployerToolId=""2014"" DeploymentMethod=""2013"" Description=""Create a web site (IIS 6.0+)"" HasPendingSecurityChanges=""False"" Id=""4009"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Create Web Site"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID8"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID9"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID10"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID11"" Id=""10019"" Name=""SiteName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID12"" Id=""10020"" Name=""PortNumber"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID13"" Id=""10021"" Name=""PhysicalPath"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID14"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID15"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID16"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID17"" Id=""10019"" Name=""SiteName"" Value=""FabrikamDEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID18"" Id=""10020"" Name=""PortNumber"" Value=""8000"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID19"" Id=""10021"" Name=""PhysicalPath"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID14</x:Reference><x:Reference>__ReferenceID15</x:Reference><x:Reference>__ReferenceID16</x:Reference><x:Reference>__ReferenceID17</x:Reference><x:Reference>__ReferenceID18</x:Reference><x:Reference>__ReferenceID19</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><RollbackActivity DisplayName=""Rollback"" sap:VirtualizedContainerService.HintSize=""356.8,436.8""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""334.4,99.2"" IsSkipped=""False"" WorkflowActivityId=""e7783400-9b05-4970-8777-6ab92c78d155""><ActionActivity.StageActivity><mtrdm:StageActivity Component=""{x:Reference __ReferenceID20}"" ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""62"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""0"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""e7783400-9b05-4970-8777-6ab92c78d155""><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID21"" Id=""9950"" Name=""SourceFileFolder"" Value=""c:\FabrikamRM\Backup\DEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID22"" Id=""9951"" Name=""DestinationFileFolder"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID21</x:Reference><x:Reference>__ReferenceID22</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ActionActivity DisplayName=""Create Web Site"" sap:VirtualizedContainerService.HintSize=""334.4,172.8"" IsSkipped=""False"" WorkflowActivityId=""a657d65d-f4d6-42f8-aa1f-7ac638496557""><ActionActivity.StageActivity><mtrdm:StageActivity Component=""{x:Reference __ReferenceID23}"" ActivityDisplayName=""Create Web Site"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4009"" HasPendingSecurityChanges=""False"" Id=""63"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""0"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""a657d65d-f4d6-42f8-aa1f-7ac638496557""><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID24"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID25"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID26"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID27"" Id=""10019"" Name=""SiteName"" Value=""FabrikamDEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID28"" Id=""10020"" Name=""PortNumber"" Value=""8000"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID29"" Id=""10021"" Name=""PhysicalPath"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID24</x:Reference><x:Reference>__ReferenceID25</x:Reference><x:Reference>__ReferenceID26</x:Reference><x:Reference>__ReferenceID27</x:Reference><x:Reference>__ReferenceID28</x:Reference><x:Reference>__ReferenceID29</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity></RollbackActivity></ServerActivity></DeploymentSequenceActivity>";

            var parsedTemplate =
                (new RMDeploymentSequence { StageId = 100, WorkflowXaml = releaseTemplate }).ToReleaseTemplate();

            scriptGenerator.ScriptGenerationNotification += PrintEvents;
            await scriptGenerator.GenerateScriptAsync(parsedTemplate, path);
        }

        [TestMethod]
        public async Task FabrikamFiber_From_BKVM_With_Mix_Of_Rollback_Blocks()
        {
            var fakeFs = new FakeFileSystem();
            var componentRepo = new FakeComponentRepo();
            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4010, 
                        WorkflowActivityId = Guid.Parse("d25617e8-7fac-4637-a7cb-2ad4da996156"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"-DropWebSite -sn ""__SiteName__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                "FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4016, 
                        WorkflowActivityId = Guid.Parse("00152F01-0651-4221-9835-3394F1739A2F"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"*.* ""__Installation Path__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "Installation Path", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 3981, 
                        WorkflowActivityId = Guid.Parse("E7783400-9B05-4970-8777-6AB92C78D155"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"""__SourceFileFolder__"" ""__DestinationFileFolder__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SourceFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\Backup\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "DestinationFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4009, 
                        WorkflowActivityId = Guid.Parse("A657D65D-F4D6-42F8-AA1F-7AC638496557"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments =
                            @"-CreateWebSite -sn ""__SiteName__"" -port __PortNumber__ -pd ""__PhysicalPath__"" -ap ""__AppPoolName__"" -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "AppPoolName", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsPreloadEnabled", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsAutoStart", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                @"FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PortNumber", 
                                            Value = @"8000", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PhysicalPath", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 4009, 
                        WorkflowActivityId = Guid.Parse("51C262E8-D1FE-4FF8-ADD6-879C2718616B"), 
                        Command = "IISConfig.exe", 
                        DeployerToolId = 2014, 
                        FileExtensionFilter = string.Empty, 
                        Arguments =
                            @"-CreateWebSite -sn ""__SiteName__"" -port __PortNumber__ -pd ""__PhysicalPath__"" -ap ""__AppPoolName__"" -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "AppPoolName", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsPreloadEnabled", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "IsAutoStart", 
                                            Value = string.Empty, 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SiteName", 
                                            Value =
                                                @"FabrikamDEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PortNumber", 
                                            Value = @"8000", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "PhysicalPath", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 3981, 
                        WorkflowActivityId = Guid.Parse("8160F49B-34C4-4860-AEC4-B9FC4CF81E9B"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"""__SourceFileFolder__"" ""__DestinationFileFolder__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SourceFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "DestinationFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\Backup\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 3981, 
                        WorkflowActivityId = Guid.Parse("75cac0c3-d5ae-4629-b7be-e0acc5243610"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"""__SourceFileFolder__"" ""__DestinationFileFolder__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SourceFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "DestinationFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\Backup\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            componentRepo.Components.Add(
                new RMComponent
                    {
                        Id = 3981, 
                        WorkflowActivityId = Guid.Parse("8e02b95b-b61c-4db3-9a6c-a1231d1c1b63"), 
                        Command = "irxcopy.cmd", 
                        DeployerToolId = 12, 
                        FileExtensionFilter = string.Empty, 
                        Arguments = @"""__SourceFileFolder__"" ""__DestinationFileFolder__""", 
                        VariableReplacementMethod = VariableReplacementMethod.OnlyInCommand, 
                        ConfigurationVariables =
                            new List<ConfigurationVariable>
                                {
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "SourceFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\WebSite\DEV", 
                                            IsParameter =
                                                true
                                        }, 
                                    new ConfigurationVariable
                                        {
                                            OriginalName =
                                                "DestinationFileFolder", 
                                            Value =
                                                @"c:\FabrikamRM\Backup\DEV", 
                                            IsParameter =
                                                true
                                        }
                                }
                    });

            var fakeUserRepo = new FakeUserRepo();
            var fakeDeployerToolRepo = new FakeDeployerToolRepo();

            var scriptGenerator = new ScriptGenerator(fakeFs, componentRepo, fakeUserRepo, fakeDeployerToolRepo);
            var path = @"C:\RMWorkflow";
            var releaseTemplate =
                @"<DeploymentSequenceActivity xmlns=""clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow"" xmlns:mtrdm=""clr-namespace:Microsoft.TeamFoundation.Release.Data.Model;assembly=Microsoft.TeamFoundation.Release.Data"" xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities"" xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation"" xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" DisplayName=""Deployment Sequence"" sap:VirtualizedContainerService.HintSize=""401.6,1780.8"" mva:VisualBasic.Settings=""Assembly references and imported namespaces serialized as XML namespaces""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><ServerActivity sap:VirtualizedContainerService.HintSize=""379.2,1656"" InstanceSequenceNumber=""1"" ServerId=""1"" ServerName=""VSALM""><ActionActivity DisplayName=""Remove Web Site"" sap:VirtualizedContainerService.HintSize=""356.8,81.6"" IsSkipped=""False"" WorkflowActivityId=""d25617e8-7fac-4637-a7cb-2ad4da996156""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Remove Web Site"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4010"" HasPendingSecurityChanges=""False"" Id=""80"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""d25617e8-7fac-4637-a7cb-2ad4da996156""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""-DropWebSite -sn &quot;__SiteName__&quot;"" CategoryId=""2005"" CategoryName=""IIS"" Command=""IISConfig.exe"" DeployerToolId=""2014"" DeploymentMethod=""2013"" Description=""Remove a web site (IIS 6.0+)"" HasPendingSecurityChanges=""False"" Id=""4010"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Remove Web Site"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID0"" Id=""10022"" Name=""SiteName"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID1"" Id=""10022"" Name=""SiteName"" Value=""FabrikamDev"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID1</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><RollbackActivity DisplayName=""Rollback"" sap:VirtualizedContainerService.HintSize=""356.8,224""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""334.4,99.2"" IsSkipped=""False"" WorkflowActivityId=""75cac0c3-d5ae-4629-b7be-e0acc5243610""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""-101388"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""75cac0c3-d5ae-4629-b7be-e0acc5243610""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""&quot;__SourceFileFolder__&quot; &quot;__DestinationFileFolder__&quot;"" CategoryId=""1998"" CategoryName=""Windows OS"" Command=""irxcopy.cmd"" DeployerToolId=""12"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable Id=""-916511"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable Id=""-916511"" Name=""DestinationFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID2"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID3"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID4"" Id=""9950"" Name=""SourceFileFolder"" Value=""c:\FabrikamRM\Backup\DEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID5"" Id=""9951"" Name=""DestinationFileFolder"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID4</x:Reference><x:Reference>__ReferenceID5</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity></RollbackActivity><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""356.8,99.2"" IsSkipped=""False"" WorkflowActivityId=""8160f49b-34c4-4860-aec4-b9fc4cf81e9b""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""81"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""8160f49b-34c4-4860-aec4-b9fc4cf81e9b""><mtrdm:StageActivity.Component><mtrdm:Component x:Name=""__ReferenceID28"" ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""&quot;__SourceFileFolder__&quot; &quot;__DestinationFileFolder__&quot;"" CategoryId=""1998"" CategoryName=""Windows OS"" Command=""irxcopy.cmd"" DeployerToolId=""12"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID6"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID7"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID8"" Id=""9950"" Name=""SourceFileFolder"" Value=""c:\FabrikamRM\WebSite\DEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID9"" Id=""9951"" Name=""DestinationFileFolder"" Value=""c:\FabrikamRM\Backup\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID8</x:Reference><x:Reference>__ReferenceID9</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><RollbackAlwaysActivity DisplayName=""Rollback Always"" sap:VirtualizedContainerService.HintSize=""356.8,224""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""334.4,99.2"" IsSkipped=""False"" WorkflowActivityId=""8e02b95b-b61c-4db3-9a6c-a1231d1c1b63""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""-813370"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""8e02b95b-b61c-4db3-9a6c-a1231d1c1b63""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""&quot;__SourceFileFolder__&quot; &quot;__DestinationFileFolder__&quot;"" CategoryId=""1998"" CategoryName=""Windows OS"" Command=""irxcopy.cmd"" DeployerToolId=""12"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable Id=""-727852"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable Id=""-727852"" Name=""DestinationFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID10"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID11"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID12"" Id=""9950"" Name=""SourceFileFolder"" Value=""c:\FabrikamRM\Backup\DEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID13"" Id=""9951"" Name=""DestinationFileFolder"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID12</x:Reference><x:Reference>__ReferenceID13</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity></RollbackAlwaysActivity><ComponentActivity DisplayName=""Fabrikam Call Center"" sap:VirtualizedContainerService.HintSize=""356.8,52.8"" IsSkipped=""False"" WorkflowActivityId=""00152f01-0651-4221-9835-3394f1739a2f""><ComponentActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Fabrikam Call Center"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4016"" HasPendingSecurityChanges=""False"" Id=""85"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""00152f01-0651-4221-9835-3394f1739a2f""><mtrdm:StageActivity.Component><mtrdm:ApplicationVersionComponent Id=""4016"" InMemoryStatus=""Existing"" Name=""Fabrikam Call Center"" OverrideRelativePathToPackageLocation=""""><mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID14"" Id=""10034"" Name=""Installation Path"" Value="""" /></mtrdm:ApplicationVersionComponent.ConfigurationVariables><mtrdm:ApplicationVersionComponent.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:ApplicationVersionComponent.PropertyBagVariables></mtrdm:ApplicationVersionComponent></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID15"" Id=""10034"" Name=""Installation Path"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ComponentActivity.StageActivity><ComponentActivity.StageActivityVariables><x:Reference>__ReferenceID15</x:Reference></ComponentActivity.StageActivityVariables></ComponentActivity><ActionActivity DisplayName=""Create Web Site"" sap:VirtualizedContainerService.HintSize=""356.8,172.8"" IsSkipped=""False"" WorkflowActivityId=""51c262e8-d1fe-4ff8-add6-879c2718616b""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Create Web Site"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4009"" HasPendingSecurityChanges=""False"" Id=""82"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""1"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""51c262e8-d1fe-4ff8-add6-879c2718616b""><mtrdm:StageActivity.Component><mtrdm:Component x:Name=""__ReferenceID31"" ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""-CreateWebSite -sn &quot;__SiteName__&quot; -port __PortNumber__ -pd &quot;__PhysicalPath__&quot; -ap &quot;__AppPoolName__&quot; -enablepreload __IsPreloadEnabled__ -autostart __IsAutoStart__"" CategoryId=""2005"" CategoryName=""IIS"" Command=""IISConfig.exe"" DeployerToolId=""2014"" DeploymentMethod=""2013"" Description=""Create a web site (IIS 6.0+)"" HasPendingSecurityChanges=""False"" Id=""4009"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Create Web Site"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID16"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID17"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID18"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID19"" Id=""10019"" Name=""SiteName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID20"" Id=""10020"" Name=""PortNumber"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID21"" Id=""10021"" Name=""PhysicalPath"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID22"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID23"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID24"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID25"" Id=""10019"" Name=""SiteName"" Value=""FabrikamDEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID26"" Id=""10020"" Name=""PortNumber"" Value=""8000"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID27"" Id=""10021"" Name=""PhysicalPath"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID22</x:Reference><x:Reference>__ReferenceID23</x:Reference><x:Reference>__ReferenceID24</x:Reference><x:Reference>__ReferenceID25</x:Reference><x:Reference>__ReferenceID26</x:Reference><x:Reference>__ReferenceID27</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><RollbackActivity DisplayName=""Rollback"" sap:VirtualizedContainerService.HintSize=""356.8,436.8""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""334.4,99.2"" IsSkipped=""False"" WorkflowActivityId=""e7783400-9b05-4970-8777-6ab92c78d155""><ActionActivity.StageActivity><mtrdm:StageActivity Component=""{x:Reference __ReferenceID28}"" ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""83"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""0"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""e7783400-9b05-4970-8777-6ab92c78d155""><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID29"" Id=""9950"" Name=""SourceFileFolder"" Value=""c:\FabrikamRM\Backup\DEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID30"" Id=""9951"" Name=""DestinationFileFolder"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID29</x:Reference><x:Reference>__ReferenceID30</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity><ActionActivity DisplayName=""Create Web Site"" sap:VirtualizedContainerService.HintSize=""334.4,172.8"" IsSkipped=""False"" WorkflowActivityId=""a657d65d-f4d6-42f8-aa1f-7ac638496557""><ActionActivity.StageActivity><mtrdm:StageActivity Component=""{x:Reference __ReferenceID31}"" ActivityDisplayName=""Create Web Site"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""4009"" HasPendingSecurityChanges=""False"" Id=""84"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""1"" ServerInstanceSequenceNumber=""0"" ServerName=""VSALM"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""a657d65d-f4d6-42f8-aa1f-7ac638496557""><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID32"" Id=""9881"" Name=""AppPoolName"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID33"" Id=""9882"" Name=""IsPreloadEnabled"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID34"" Id=""9883"" Name=""IsAutoStart"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID35"" Id=""10019"" Name=""SiteName"" Value=""FabrikamDEV"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID36"" Id=""10020"" Name=""PortNumber"" Value=""8000"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID37"" Id=""10021"" Name=""PhysicalPath"" Value=""c:\FabrikamRM\WebSite\DEV"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID32</x:Reference><x:Reference>__ReferenceID33</x:Reference><x:Reference>__ReferenceID34</x:Reference><x:Reference>__ReferenceID35</x:Reference><x:Reference>__ReferenceID36</x:Reference><x:Reference>__ReferenceID37</x:Reference></ActionActivity.StageActivityVariables><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean><x:Boolean x:Key=""IsPinned"">False</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState></ActionActivity></RollbackActivity></ServerActivity></DeploymentSequenceActivity>";

            var parsedTemplate =
                (new RMDeploymentSequence { StageId = 100, WorkflowXaml = releaseTemplate }).ToReleaseTemplate();
            scriptGenerator.ScriptGenerationNotification += PrintEvents;
            await scriptGenerator.GenerateScriptAsync(parsedTemplate, path);
        }

        [TestMethod]
        public async Task Scripts_Are_Not_Generated_For_Containers()
        {
            // Arrange
            var fs = new FakeFileSystem();
            var fakeComponentRepo = new FakeComponentRepo
                                      {
                                          Components =
                                              new List<RMComponent>
                                                  {
                                                      new RMComponent
                                                          {
                                                              Id = 3981, 
                                                              WorkflowActivityId
                                                                  =
                                                                  Guid
                                                                  .Parse(
                                                                      "54b7b5f4-6a4d-4cec-b1e9-edae6bda0f20")
                                                          }, 
                                                      new RMComponent
                                                          {
                                                              Id = 3981, 
                                                              WorkflowActivityId
                                                                  =
                                                                  Guid
                                                                  .Parse(
                                                                      "9f2080c8-d438-431e-8cb5-f312c5c2b836")
                                                          }
                                                  }
                                      };

            var fakeUserRepo = new FakeUserRepo();
            var fakeDeployerToolRepo = new FakeDeployerToolRepo();

            var gen = new ScriptGenerator(fs, fakeComponentRepo, fakeUserRepo, fakeDeployerToolRepo);

            var testXml =
                @"<DeploymentSequenceActivity xmlns=""clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow"" xmlns:mtrdm=""clr-namespace:Microsoft.TeamFoundation.Release.Data.Model;assembly=Microsoft.TeamFoundation.Release.Data"" xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities"" xmlns:p=""http://schemas.microsoft.com/netfx/2009/xaml/activities"" xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation"" xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" DisplayName=""Deployment Sequence"" sap:VirtualizedContainerService.HintSize=""643.2,473.6"" mva:VisualBasic.Settings=""Assembly references and imported namespaces serialized as XML namespaces""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><p:Sequence DisplayName=""Sequence&lt;&gt;+"" sap:VirtualizedContainerService.HintSize=""620.8,348.8""><p:Parallel DisplayName=""Parallel|&quot;"" sap:VirtualizedContainerService.HintSize=""598.4,224""><ServerActivity sap:VirtualizedContainerService.HintSize=""222.4,177.6"" InstanceSequenceNumber=""1"" ServerId=""2"" ServerName=""placeholderServer?*!#:;""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""200,52.8"" IsSkipped=""False"" WorkflowActivityId=""54b7b5f4-6a4d-4cec-b1e9-edae6bda0f20""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""-221106"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""2"" ServerInstanceSequenceNumber=""1"" ServerName=""placeholderServer?*!#:;"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""54b7b5f4-6a4d-4cec-b1e9-edae6bda0f20""><mtrdm:StageActivity.Component><mtrdm:Component x:Name=""__ReferenceID2"" ActionTypeId=""2"" ActionTypeName=""Custom action"" ApplyFullValidation=""False"" CategoryId=""1998"" CategoryName=""Windows OS"" DeployerToolId=""0"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""New"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""True"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""4"" TypeName=""Without source"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID0"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID1"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID0</x:Reference><x:Reference>__ReferenceID1</x:Reference></ActionActivity.StageActivityVariables></ActionActivity></ServerActivity><ServerTagActivity ExecutionMode=""1"" sap:VirtualizedContainerService.HintSize=""222.4,177.6"" InstanceSequenceNumber=""1"" TagName=""placeholderTag?!""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""200,52.8"" IsSkipped=""False"" WorkflowActivityId=""9f2080c8-d438-431e-8cb5-f312c5c2b836""><ActionActivity.StageActivity><mtrdm:StageActivity Component=""{x:Reference __ReferenceID2}"" ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""-35752"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""1"" LinkedV2ComponentId=""0"" Name=""placeholderTag?!"" ServerId=""0"" ServerInstanceSequenceNumber=""1"" StatusId=""2"" StatusName=""Active"" TagName=""placeholderTag?!"" WorkflowActivityId=""9f2080c8-d438-431e-8cb5-f312c5c2b836""><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID3"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID4"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID3</x:Reference><x:Reference>__ReferenceID4</x:Reference></ActionActivity.StageActivityVariables></ActionActivity></ServerTagActivity></p:Parallel></p:Sequence></DeploymentSequenceActivity>";
            var targetPath = @"C:\RMWorkflow\";

            // Act
            var sequence = (new RMDeploymentSequence { StageId = 100, WorkflowXaml = testXml }).ToReleaseTemplate();
            await gen.GenerateScriptAsync(sequence, targetPath);

            // Assert
            Assert.IsFalse(fs.Files.ContainsKey(@"C:\RMWorkflow\1_Sequence_Sequence+\ReleaseScript.ps1"));
        }

        private static void PrintEvents(object o, GenerationEventArgs args)
        {
            Debug.WriteLine(
                $"{args.GenerationEventType}: {args.Sequence} - {args.BlockType} {args.DisplayName}  IsContainer: {args.IsContainer} IsEnabled: {args.IsEnabled}");
            Debug.Flush();
        }
    }
}