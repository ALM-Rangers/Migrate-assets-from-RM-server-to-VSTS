// --------------------------------------------------------------------------------------------------------------------
// <copyright file="When_Generating_A_Folder_Structure_From_A_Workflow.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

 // ReSharper disable InconsistentNaming

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Parser;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Tests.Unit.TestHelpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class When_Generating_A_Folder_Structure_From_A_Workflow
    {
        [TestMethod]
        public async Task A_Valid_Folder_Structure_Is_Generated_For_A_Rollback_Block()
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
                                                                      "1d5e32ae-1b13-4e9a-8493-70e243a4ffed")
                                                          }, 
                                                      new RMComponent
                                                          {
                                                              Id = 3981, 
                                                              WorkflowActivityId
                                                                  =
                                                                  Guid
                                                                  .Parse(
                                                                      "6d0c925e-4420-4420-a8a0-c800f5bd48f4")
                                                          }, 
                                                      new RMComponent
                                                          {
                                                              Id = 3982, 
                                                              WorkflowActivityId
                                                                  =
                                                                  Guid
                                                                  .Parse(
                                                                      "74fb48d1-ca36-4477-88b2-22e5d9f41ca1")
                                                          }
                                                  }
                                      };

            var fakeUserRepo = new FakeUserRepo();
            var fakeDeployerToolRepo = new FakeDeployerToolRepo();

            var gen = new ScriptGenerator(fs, fakeComponentRepo, fakeUserRepo, fakeDeployerToolRepo);

            var testXml =
                @"<DeploymentSequenceActivity xmlns=""clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow"" xmlns:mtrdm=""clr-namespace:Microsoft.TeamFoundation.Release.Data.Model;assembly=Microsoft.TeamFoundation.Release.Data"" xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities"" xmlns:p=""http://schemas.microsoft.com/netfx/2009/xaml/activities"" xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation"" xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" DisplayName=""Deployment Sequence"" sap:VirtualizedContainerService.HintSize=""643.2,566.4"" mva:VisualBasic.Settings=""Assembly references and imported namespaces serialized as XML namespaces""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><p:Parallel DisplayName=""Parallel"" sap:VirtualizedContainerService.HintSize=""620.8,441.6""><ServerActivity sap:VirtualizedContainerService.HintSize=""244.8,395.2"" InstanceSequenceNumber=""1"" ServerId=""19"" ServerName=""lab-dmann""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""222.4,52.8"" IsSkipped=""False"" WorkflowActivityId=""1d5e32ae-1b13-4e9a-8493-70e243a4ffed""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""1498"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""19"" ServerInstanceSequenceNumber=""1"" ServerName=""lab-dmann"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""1d5e32ae-1b13-4e9a-8493-70e243a4ffed""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""&quot;__SourceFileFolder__&quot; &quot;__DestinationFileFolder__&quot;"" CategoryId=""1998"" CategoryName=""Windows OS"" Command=""irxcopy.cmd"" DeployerToolId=""12"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID0"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID1"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID2"" Id=""9950"" Name=""SourceFileFolder"" Value=""placeholder"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID3"" Id=""9951"" Name=""DestinationFileFolder"" Value=""bar"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID2</x:Reference><x:Reference>__ReferenceID3</x:Reference></ActionActivity.StageActivityVariables></ActionActivity><RollbackActivity DisplayName=""Rollback"" sap:VirtualizedContainerService.HintSize=""222.4,177.6""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""200,52.8"" IsSkipped=""False"" WorkflowActivityId=""6d0c925e-4420-4420-a8a0-c800f5bd48f4""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""-252944"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""19"" ServerInstanceSequenceNumber=""1"" ServerName=""lab-dmann"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""6d0c925e-4420-4420-a8a0-c800f5bd48f4""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""2"" ActionTypeName=""Custom action"" ApplyFullValidation=""False"" CategoryId=""1998"" CategoryName=""Windows OS"" DeployerToolId=""0"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""New"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""True"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""4"" TypeName=""Without source"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID4"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID5"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID4</x:Reference><x:Reference>__ReferenceID5</x:Reference></ActionActivity.StageActivityVariables></ActionActivity></RollbackActivity></ServerActivity><ServerTagActivity ExecutionMode=""1"" sap:VirtualizedContainerService.HintSize=""222.4,395.2"" InstanceSequenceNumber=""1"" TagName=""Dev""><ActionActivity DisplayName=""Create Folder"" sap:VirtualizedContainerService.HintSize=""200,52.8"" IsSkipped=""False"" WorkflowActivityId=""74fb48d1-ca36-4477-88b2-22e5d9f41ca1""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Create Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3982"" HasPendingSecurityChanges=""False"" Id=""1499"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""1"" LinkedV2ComponentId=""0"" ServerId=""0"" ServerInstanceSequenceNumber=""1"" StatusId=""2"" StatusName=""Active"" TagName=""Dev"" WorkflowActivityId=""74fb48d1-ca36-4477-88b2-22e5d9f41ca1""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""-command ./ManageWindowsIO.ps1 -Action Create -FileFolderName '__FolderName__'"" CategoryId=""1998"" CategoryName=""Windows OS"" Command=""powershell"" DeployerToolId=""2011"" DeploymentMethod=""2013"" Description=""Create a folder"" HasPendingSecurityChanges=""False"" Id=""3982"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Create Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID6"" Id=""9952"" Name=""FolderName"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID7"" Id=""9952"" Name=""FolderName"" Value=""baz"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID7</x:Reference></ActionActivity.StageActivityVariables></ActionActivity></ServerTagActivity></p:Parallel></DeploymentSequenceActivity>";
            var targetPath = @"C:\RMWorkflow\";

            // Act
            var sequence = (new RMDeploymentSequence { StageId = 100, WorkflowXaml = testXml }).ToReleaseTemplate();
            await gen.GenerateScriptAsync(sequence, targetPath);

            // Assert
            Assert.IsTrue(fs.Directories.Contains(@"C:\RMWorkflow\1_Parallel"));
            Assert.IsTrue(fs.Directories.Contains(@"C:\RMWorkflow\1_Parallel\1_Server_lab-dmann"));
            Assert.IsTrue(fs.Directories.Contains(@"C:\RMWorkflow\1_Parallel\2_ServerTag_Dev"));
        }

        [TestMethod]
        public async Task A_Valid_Folder_Structure_Is_Generated_For_A_Sequence_Containing_A_Server()
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
                                                                      "1d5e32ae-1b13-4e9a-8493-70e243a4ffed")
                                                          }, 
                                                      new RMComponent
                                                          {
                                                              Id = 3981, 
                                                              WorkflowActivityId
                                                                  =
                                                                  Guid
                                                                  .Parse(
                                                                      "6d0c925e-4420-4420-a8a0-c800f5bd48f4")
                                                          }, 
                                                      new RMComponent
                                                          {
                                                              Id = 3982, 
                                                              WorkflowActivityId
                                                                  =
                                                                  Guid
                                                                  .Parse(
                                                                      "74fb48d1-ca36-4477-88b2-22e5d9f41ca1")
                                                          }
                                                  }
                                      };

            var fakeUserRepo = new FakeUserRepo();
            var fakeDeployerToolRepo = new FakeDeployerToolRepo();

            var gen = new ScriptGenerator(fs, fakeComponentRepo, fakeUserRepo, fakeDeployerToolRepo);

            var testXml =
                @"<DeploymentSequenceActivity xmlns=""clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow"" xmlns:mtrdm=""clr-namespace:Microsoft.TeamFoundation.Release.Data.Model;assembly=Microsoft.TeamFoundation.Release.Data"" xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities"" xmlns:p=""http://schemas.microsoft.com/netfx/2009/xaml/activities"" xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation"" xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" DisplayName=""Deployment Sequence"" sap:VirtualizedContainerService.HintSize=""620.8,348.8"" mva:VisualBasic.Settings=""Assembly references and imported namespaces serialized as XML namespaces""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><p:Parallel DisplayName=""Parallel"" sap:VirtualizedContainerService.HintSize=""598.4,224""><ServerActivity sap:VirtualizedContainerService.HintSize=""222.4,177.6"" InstanceSequenceNumber=""1"" ServerId=""19"" ServerName=""lab-dmann""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""200,52.8"" IsSkipped=""False"" WorkflowActivityId=""1d5e32ae-1b13-4e9a-8493-70e243a4ffed""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""-702266"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""19"" ServerInstanceSequenceNumber=""1"" ServerName=""lab-dmann"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""1d5e32ae-1b13-4e9a-8493-70e243a4ffed""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""2"" ActionTypeName=""Custom action"" ApplyFullValidation=""False"" CategoryId=""1998"" CategoryName=""Windows OS"" DeployerToolId=""0"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""New"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""True"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""4"" TypeName=""Without source"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID0"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID1"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID0</x:Reference><x:Reference>__ReferenceID1</x:Reference></ActionActivity.StageActivityVariables></ActionActivity></ServerActivity><ServerTagActivity ExecutionMode=""1"" sap:VirtualizedContainerService.HintSize=""222.4,177.6"" InstanceSequenceNumber=""1"" TagName=""Dev""><ActionActivity DisplayName=""Create Folder"" sap:VirtualizedContainerService.HintSize=""200,52.8"" IsSkipped=""False"" WorkflowActivityId=""74fb48d1-ca36-4477-88b2-22e5d9f41ca1""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Create Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3982"" HasPendingSecurityChanges=""False"" Id=""-468156"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""1"" LinkedV2ComponentId=""0"" Name=""Dev"" ServerId=""0"" ServerInstanceSequenceNumber=""1"" StatusId=""2"" StatusName=""Active"" TagName=""Dev"" WorkflowActivityId=""74fb48d1-ca36-4477-88b2-22e5d9f41ca1""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""2"" ActionTypeName=""Custom action"" ApplyFullValidation=""False"" CategoryId=""1998"" CategoryName=""Windows OS"" DeployerToolId=""0"" DeploymentMethod=""2013"" Description=""Create a folder"" HasPendingSecurityChanges=""False"" Id=""3982"" InMemoryStatus=""New"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""True"" IsSerializing=""False"" Name=""Create Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""4"" TypeName=""Without source"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID2"" Id=""9952"" Name=""FolderName"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID2</x:Reference></ActionActivity.StageActivityVariables></ActionActivity></ServerTagActivity></p:Parallel></DeploymentSequenceActivity>";
            var targetPath = @"C:\RMWorkflow\";

            // Act
            var sequence = (new RMDeploymentSequence { StageId = 100, WorkflowXaml = testXml }).ToReleaseTemplate();
            await gen.GenerateScriptAsync(sequence, targetPath);

            // Assert
            Assert.IsTrue(fs.Directories.Contains(@"C:\RMWorkflow\1_Parallel"));
            Assert.IsTrue(fs.Directories.Contains(@"C:\RMWorkflow\1_Parallel\1_Server_lab-dmann"));
            Assert.IsTrue(fs.Directories.Contains(@"C:\RMWorkflow\1_Parallel\2_ServerTag_Dev"));
        }

        [TestMethod]
        public async Task A_Valid_Folder_Structure_Is_Generated_For_A_Sequence_With_A_Server()
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
                                                                      "a122e31d-9faa-4468-b09b-2ec09ea51567")
                                                          }
                                                  }
                                      };

            var fakeUserRepo = new FakeUserRepo();
            var fakeDeployerToolRepo = new FakeDeployerToolRepo();

            var gen = new ScriptGenerator(fs, fakeComponentRepo, fakeUserRepo, fakeDeployerToolRepo);
            var testXml =
                @"<DeploymentSequenceActivity xmlns=""clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow"" xmlns:mtrdm=""clr-namespace:Microsoft.TeamFoundation.Release.Data.Model;assembly=Microsoft.TeamFoundation.Release.Data"" xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities"" xmlns:p=""http://schemas.microsoft.com/netfx/2009/xaml/activities"" xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation"" xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" DisplayName=""Deployment Sequence"" sap:VirtualizedContainerService.HintSize=""244.8,302.4"" mva:VisualBasic.Settings=""Assembly references and imported namespaces serialized as XML namespaces""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><ServerActivity sap:VirtualizedContainerService.HintSize=""222.4,177.6"" InstanceSequenceNumber=""1"" ServerId=""19"" ServerName=""lab-dmann""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""200,52.8"" IsSkipped=""False"" WorkflowActivityId=""a122e31d-9faa-4468-b09b-2ec09ea51567""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""-835378"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""19"" ServerInstanceSequenceNumber=""1"" ServerName=""lab-dmann"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""a122e31d-9faa-4468-b09b-2ec09ea51567""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""2"" ActionTypeName=""Custom action"" ApplyFullValidation=""False"" CategoryId=""1998"" CategoryName=""Windows OS"" DeployerToolId=""0"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""New"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""True"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""4"" TypeName=""Without source"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID0"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID1"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID0</x:Reference><x:Reference>__ReferenceID1</x:Reference></ActionActivity.StageActivityVariables></ActionActivity></ServerActivity></DeploymentSequenceActivity>";
            var targetPath = @"C:\RMWorkflow\";

            // Act
            var sequence = (new RMDeploymentSequence { StageId = 100, WorkflowXaml = testXml }).ToReleaseTemplate();
            await gen.GenerateScriptAsync(sequence, targetPath);

            // Assert
            Assert.IsTrue(fs.Directories.Contains(@"C:\RMWorkflow\1_Server_lab-dmann"));
        }

        [TestMethod]
        public async Task Invalid_Characters_Are_Scrubbed_From_Folder_Names()
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
            Assert.IsTrue(fs.Directories.Contains(@"C:\RMWorkflow\1_Sequence_Sequence+"));
            Assert.IsTrue(fs.Directories.Contains(@"C:\RMWorkflow\1_Sequence_Sequence+\1_Parallel"));
            Assert.IsTrue(
                fs.Directories.Contains(@"C:\RMWorkflow\1_Sequence_Sequence+\1_Parallel\1_Server_placeholderServer!#"));
            Assert.IsTrue(
                fs.Directories.Contains(@"C:\RMWorkflow\1_Sequence_Sequence+\1_Parallel\2_ServerTag_placeholderTag!"));
        }

        [TestMethod]
        public async Task Scripts_Are_Created_In_The_Folders_As_Appropriate()
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
                                                                      "1d5e32ae-1b13-4e9a-8493-70e243a4ffed")
                                                          }, 
                                                      new RMComponent
                                                          {
                                                              Id = 3981, 
                                                              WorkflowActivityId
                                                                  =
                                                                  Guid
                                                                  .Parse(
                                                                      "6d0c925e-4420-4420-a8a0-c800f5bd48f4")
                                                          }, 
                                                      new RMComponent
                                                          {
                                                              Id = 3982, 
                                                              WorkflowActivityId
                                                                  =
                                                                  Guid
                                                                  .Parse(
                                                                      "74fb48d1-ca36-4477-88b2-22e5d9f41ca1")
                                                          }
                                                  }
                                      };

            var fakeUserRepo = new FakeUserRepo();
            var fakeDeployerToolRepo = new FakeDeployerToolRepo();

            var gen = new ScriptGenerator(fs, fakeComponentRepo, fakeUserRepo, fakeDeployerToolRepo);

            var testXml =
                @"<DeploymentSequenceActivity xmlns=""clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow"" xmlns:mtrdm=""clr-namespace:Microsoft.TeamFoundation.Release.Data.Model;assembly=Microsoft.TeamFoundation.Release.Data"" xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities"" xmlns:p=""http://schemas.microsoft.com/netfx/2009/xaml/activities"" xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation"" xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" DisplayName=""Deployment Sequence"" sap:VirtualizedContainerService.HintSize=""643.2,566.4"" mva:VisualBasic.Settings=""Assembly references and imported namespaces serialized as XML namespaces""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><p:Parallel DisplayName=""Parallel"" sap:VirtualizedContainerService.HintSize=""620.8,441.6""><ServerActivity sap:VirtualizedContainerService.HintSize=""244.8,395.2"" InstanceSequenceNumber=""1"" ServerId=""19"" ServerName=""lab-dmann""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""222.4,52.8"" IsSkipped=""False"" WorkflowActivityId=""1d5e32ae-1b13-4e9a-8493-70e243a4ffed""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""1498"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""19"" ServerInstanceSequenceNumber=""1"" ServerName=""lab-dmann"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""1d5e32ae-1b13-4e9a-8493-70e243a4ffed""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""&quot;__SourceFileFolder__&quot; &quot;__DestinationFileFolder__&quot;"" CategoryId=""1998"" CategoryName=""Windows OS"" Command=""irxcopy.cmd"" DeployerToolId=""12"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID0"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID1"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID2"" Id=""9950"" Name=""SourceFileFolder"" Value=""placeholder"" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID3"" Id=""9951"" Name=""DestinationFileFolder"" Value=""bar"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID2</x:Reference><x:Reference>__ReferenceID3</x:Reference></ActionActivity.StageActivityVariables></ActionActivity><RollbackActivity DisplayName=""Rollback"" sap:VirtualizedContainerService.HintSize=""222.4,177.6""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""200,52.8"" IsSkipped=""False"" WorkflowActivityId=""6d0c925e-4420-4420-a8a0-c800f5bd48f4""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""-252944"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""19"" ServerInstanceSequenceNumber=""1"" ServerName=""lab-dmann"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""6d0c925e-4420-4420-a8a0-c800f5bd48f4""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""2"" ActionTypeName=""Custom action"" ApplyFullValidation=""False"" CategoryId=""1998"" CategoryName=""Windows OS"" DeployerToolId=""0"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""New"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""True"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""4"" TypeName=""Without source"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID4"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID5"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID4</x:Reference><x:Reference>__ReferenceID5</x:Reference></ActionActivity.StageActivityVariables></ActionActivity></RollbackActivity></ServerActivity><ServerTagActivity ExecutionMode=""1"" sap:VirtualizedContainerService.HintSize=""222.4,395.2"" InstanceSequenceNumber=""1"" TagName=""Dev""><ActionActivity DisplayName=""Create Folder"" sap:VirtualizedContainerService.HintSize=""200,52.8"" IsSkipped=""False"" WorkflowActivityId=""74fb48d1-ca36-4477-88b2-22e5d9f41ca1""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Create Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3982"" HasPendingSecurityChanges=""False"" Id=""1499"" IsDeletable=""True"" IsDirty=""False"" IsSerializing=""False"" IsTagBased=""1"" LinkedV2ComponentId=""0"" ServerId=""0"" ServerInstanceSequenceNumber=""1"" StatusId=""2"" StatusName=""Active"" TagName=""Dev"" WorkflowActivityId=""74fb48d1-ca36-4477-88b2-22e5d9f41ca1""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""1"" ApplyFullValidation=""False"" Arguments=""-command ./ManageWindowsIO.ps1 -Action Create -FileFolderName '__FolderName__'"" CategoryId=""1998"" CategoryName=""Windows OS"" Command=""powershell"" DeployerToolId=""2011"" DeploymentMethod=""2013"" Description=""Create a folder"" HasPendingSecurityChanges=""False"" Id=""3982"" InMemoryStatus=""Existing"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""False"" IsSerializing=""False"" Name=""Create Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""3"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID6"" Id=""9952"" Name=""FolderName"" Value="""" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID7"" Id=""9952"" Name=""FolderName"" Value=""baz"" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID7</x:Reference></ActionActivity.StageActivityVariables></ActionActivity></ServerTagActivity></p:Parallel></DeploymentSequenceActivity>";
            var targetPath = @"C:\RMWorkflow\";

            // Act
            var sequence = (new RMDeploymentSequence { StageId = 100, WorkflowXaml = testXml }).ToReleaseTemplate();
            await gen.GenerateScriptAsync(sequence, targetPath);

            // Assert
            Assert.IsTrue(fs.Files.ContainsKey(@"C:\RMWorkflow\1_Parallel\1_Server_lab-dmann\ReleaseScript.ps1"));
            Assert.IsTrue(fs.Files.ContainsKey(@"C:\RMWorkflow\1_Parallel\1_Server_lab-dmann\2_Rollback.ps1"));
            Assert.IsTrue(fs.Files.ContainsKey(@"C:\RMWorkflow\1_Parallel\2_ServerTag_Dev\ReleaseScript.ps1"));
            Assert.IsTrue(fs.Files.ContainsKey(@"C:\RMWorkflow\DeployerTools\TokenizationScript.ps1"));
        }

        [TestMethod]
        public async Task Folder_Structure_Contains_Release_Template_And_Stage_Name()
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
                                                                      "a122e31d-9faa-4468-b09b-2ec09ea51567")
                                                          }
                                                  }
            };

            var fakeUserRepo = new FakeUserRepo();
            var fakeDeployerToolRepo = new FakeDeployerToolRepo();

            var gen = new ScriptGenerator(fs, fakeComponentRepo, fakeUserRepo, fakeDeployerToolRepo);
            var testXml =
                @"<DeploymentSequenceActivity xmlns=""clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow"" xmlns:mtrdm=""clr-namespace:Microsoft.TeamFoundation.Release.Data.Model;assembly=Microsoft.TeamFoundation.Release.Data"" xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities"" xmlns:p=""http://schemas.microsoft.com/netfx/2009/xaml/activities"" xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation"" xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" DisplayName=""Deployment Sequence"" sap:VirtualizedContainerService.HintSize=""244.8,302.4"" mva:VisualBasic.Settings=""Assembly references and imported namespaces serialized as XML namespaces""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><ServerActivity sap:VirtualizedContainerService.HintSize=""222.4,177.6"" InstanceSequenceNumber=""1"" ServerId=""19"" ServerName=""lab-dmann""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""200,52.8"" IsSkipped=""False"" WorkflowActivityId=""a122e31d-9faa-4468-b09b-2ec09ea51567""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""-835378"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""19"" ServerInstanceSequenceNumber=""1"" ServerName=""lab-dmann"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""a122e31d-9faa-4468-b09b-2ec09ea51567""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""2"" ActionTypeName=""Custom action"" ApplyFullValidation=""False"" CategoryId=""1998"" CategoryName=""Windows OS"" DeployerToolId=""0"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""New"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""True"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""4"" TypeName=""Without source"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID0"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID1"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID0</x:Reference><x:Reference>__ReferenceID1</x:Reference></ActionActivity.StageActivityVariables></ActionActivity></ServerActivity></DeploymentSequenceActivity>";
            var targetPath = @"C:\RMWorkflow\";

            // Act
            var sequence = (new RMDeploymentSequence { StageId = 100, ReleaseTemplateName = "Template", ReleaseTemplateStageName = "Stage", WorkflowXaml = testXml }).ToReleaseTemplate();
            await gen.GenerateScriptAsync(sequence, targetPath);

            // Assert
            Assert.IsTrue(fs.Directories.Contains(@"C:\RMWorkflow\Template\Stage\1_Server_lab-dmann"));
        }

        [TestMethod]
        public async Task Invalid_Characters_Are_Stripped_From_Template_Name()
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
                                                                      "a122e31d-9faa-4468-b09b-2ec09ea51567")
                                                          }
                                                  }
            };

            var fakeUserRepo = new FakeUserRepo();
            var fakeDeployerToolRepo = new FakeDeployerToolRepo();

            var gen = new ScriptGenerator(fs, fakeComponentRepo, fakeUserRepo, fakeDeployerToolRepo);
            var testXml =
                @"<DeploymentSequenceActivity xmlns=""clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow"" xmlns:mtrdm=""clr-namespace:Microsoft.TeamFoundation.Release.Data.Model;assembly=Microsoft.TeamFoundation.Release.Data"" xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities"" xmlns:p=""http://schemas.microsoft.com/netfx/2009/xaml/activities"" xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation"" xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" DisplayName=""Deployment Sequence"" sap:VirtualizedContainerService.HintSize=""244.8,302.4"" mva:VisualBasic.Settings=""Assembly references and imported namespaces serialized as XML namespaces""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><ServerActivity sap:VirtualizedContainerService.HintSize=""222.4,177.6"" InstanceSequenceNumber=""1"" ServerId=""19"" ServerName=""lab-dmann""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""200,52.8"" IsSkipped=""False"" WorkflowActivityId=""a122e31d-9faa-4468-b09b-2ec09ea51567""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""-835378"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""19"" ServerInstanceSequenceNumber=""1"" ServerName=""lab-dmann"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""a122e31d-9faa-4468-b09b-2ec09ea51567""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""2"" ActionTypeName=""Custom action"" ApplyFullValidation=""False"" CategoryId=""1998"" CategoryName=""Windows OS"" DeployerToolId=""0"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""New"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""True"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""4"" TypeName=""Without source"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID0"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID1"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID0</x:Reference><x:Reference>__ReferenceID1</x:Reference></ActionActivity.StageActivityVariables></ActionActivity></ServerActivity></DeploymentSequenceActivity>";
            var targetPath = @"C:\RMWorkflow\";

            // Act
            var sequence = (new RMDeploymentSequence { StageId = 100, ReleaseTemplateName = "Template*Name", ReleaseTemplateStageName = "Stage", WorkflowXaml = testXml }).ToReleaseTemplate();
            await gen.GenerateScriptAsync(sequence, targetPath);

            // Assert
            Assert.IsTrue(fs.Directories.Contains(@"C:\RMWorkflow\TemplateName\Stage\1_Server_lab-dmann"));
        }

        [TestMethod]
        public async Task Invalid_Characters_Are_Stripped_From_Template_Stage()
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
                                                                      "a122e31d-9faa-4468-b09b-2ec09ea51567")
                                                          }
                                                  }
            };

            var fakeUserRepo = new FakeUserRepo();
            var fakeDeployerToolRepo = new FakeDeployerToolRepo();

            var gen = new ScriptGenerator(fs, fakeComponentRepo, fakeUserRepo, fakeDeployerToolRepo);
            var testXml =
                @"<DeploymentSequenceActivity xmlns=""clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow"" xmlns:mtrdm=""clr-namespace:Microsoft.TeamFoundation.Release.Data.Model;assembly=Microsoft.TeamFoundation.Release.Data"" xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities"" xmlns:p=""http://schemas.microsoft.com/netfx/2009/xaml/activities"" xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation"" xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" DisplayName=""Deployment Sequence"" sap:VirtualizedContainerService.HintSize=""244.8,302.4"" mva:VisualBasic.Settings=""Assembly references and imported namespaces serialized as XML namespaces""><sap:WorkflowViewStateService.ViewState><scg:Dictionary x:TypeArguments=""x:String, x:Object""><x:Boolean x:Key=""IsExpanded"">True</x:Boolean></scg:Dictionary></sap:WorkflowViewStateService.ViewState><ServerActivity sap:VirtualizedContainerService.HintSize=""222.4,177.6"" InstanceSequenceNumber=""1"" ServerId=""19"" ServerName=""lab-dmann""><ActionActivity DisplayName=""Copy File or Folder"" sap:VirtualizedContainerService.HintSize=""200,52.8"" IsSkipped=""False"" WorkflowActivityId=""a122e31d-9faa-4468-b09b-2ec09ea51567""><ActionActivity.StageActivity><mtrdm:StageActivity ActivityDisplayName=""Copy File or Folder"" ApplyFullValidation=""False"" CanEditVariables=""True"" ComponentId=""3981"" HasPendingSecurityChanges=""False"" Id=""-835378"" IsDeletable=""True"" IsDirty=""True"" IsSerializing=""False"" IsTagBased=""0"" LinkedV2ComponentId=""0"" ServerId=""19"" ServerInstanceSequenceNumber=""1"" ServerName=""lab-dmann"" StatusId=""2"" StatusName=""Active"" WorkflowActivityId=""a122e31d-9faa-4468-b09b-2ec09ea51567""><mtrdm:StageActivity.Component><mtrdm:Component ActionTypeId=""2"" ActionTypeName=""Custom action"" ApplyFullValidation=""False"" CategoryId=""1998"" CategoryName=""Windows OS"" DeployerToolId=""0"" DeploymentMethod=""2013"" Description=""Copy file(s) or folder (wildcards can be used)"" HasPendingSecurityChanges=""False"" Id=""3981"" InMemoryStatus=""New"" IsDeletable=""True"" IsDirty=""False"" IsPublishedByMicrosoft=""True"" IsSerializing=""False"" Name=""Copy File or Folder"" StatusId=""2"" StatusName=""Active"" TeamFoundationServerId=""0"" TeamProjectCollectionId=""0"" Timeout=""5"" TypeId=""4"" TypeName=""Without source"" VariableReplacementModeId=""1"" Version=""2013""><mtrdm:Component.ConfigurationVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.ConfigurationVariables><mtrdm:Component.PropertyBagVariables><mtrdm:SortableObservableCollection x:TypeArguments=""mtrdm:ConfigurationVariable"" /></mtrdm:Component.PropertyBagVariables></mtrdm:Component></mtrdm:StageActivity.Component><mtrdm:StageActivity.Variables><mtrdm:ConfigurationVariable x:Name=""__ReferenceID0"" Id=""9950"" Name=""SourceFileFolder"" Value="""" /><mtrdm:ConfigurationVariable x:Name=""__ReferenceID1"" Id=""9951"" Name=""DestinationFileFolder"" Value="""" /></mtrdm:StageActivity.Variables></mtrdm:StageActivity></ActionActivity.StageActivity><ActionActivity.StageActivityVariables><x:Reference>__ReferenceID0</x:Reference><x:Reference>__ReferenceID1</x:Reference></ActionActivity.StageActivityVariables></ActionActivity></ServerActivity></DeploymentSequenceActivity>";
            var targetPath = @"C:\RMWorkflow\";

            // Act
            var sequence = (new RMDeploymentSequence { StageId = 100, ReleaseTemplateName = "TemplateName", ReleaseTemplateStageName = "Stage*Name", WorkflowXaml = testXml }).ToReleaseTemplate();
            await gen.GenerateScriptAsync(sequence, targetPath);

            // Assert
            Assert.IsTrue(fs.Directories.Contains(@"C:\RMWorkflow\TemplateName\StageName\1_Server_lab-dmann"));
        }
    }
}