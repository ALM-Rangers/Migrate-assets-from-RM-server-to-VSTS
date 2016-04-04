// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptGenerator.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Interfaces;
    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell.Model;
    using static Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell.Model.GenerationEventType;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell.Templates;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Parser.Model;

    /// <summary>
    /// Generates a series of PowerShell scripts for a Release Management release template given a <see cref="DeploymentSequence"/> object.
    /// </summary>
    public class ScriptGenerator
    {
        private readonly IFileSystem fs;

        private readonly bool generateInitializationScript;

        private readonly string initScriptName = "InitializationScript.ps1";

        private readonly string meaningfulDisplayNameFolderFormat = "{0}_{1}_{2}";

        private readonly string meaninglessDisplayNameFolderFormat = "{0}_{1}";

        private readonly string releaseScriptName = "ReleaseScript.ps1";

        private readonly IRMComponentRepository componentRepo;

        private readonly IRMDeployerToolRepository deployerToolRepo;

        private readonly IRMUserRepository userRepo;

        private string deployerToolsPath = "DeployerTools";

        public ScriptGenerator(
            IFileSystem fs, 
            IRMComponentRepository componentRepo, 
            IRMUserRepository userRepo, 
            IRMDeployerToolRepository deployerToolRepo, 
            bool generateInitializationScript = true)
        {
            this.fs = fs;
            this.componentRepo = componentRepo;
            this.userRepo = userRepo;
            this.deployerToolRepo = deployerToolRepo;
            this.generateInitializationScript = generateInitializationScript;
        }

        public event EventHandler<GenerationEventArgs> ScriptGenerationNotification;

        /// <summary>
        /// Generates the script.
        /// </summary>
        /// <param name="sequence">
        /// The sequence.
        /// </param>
        /// <param name="targetPath">
        /// The target path.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task GenerateScriptAsync(DeploymentSequence sequence, string targetPath)
        {
            if (sequence.ReleaseTemplateName != null && sequence.ReleaseTemplateStageName != null)
            {
                targetPath = Path.Combine(targetPath, CommonRegex.ValidFileRegex.Replace(sequence.ReleaseTemplateName, string.Empty), CommonRegex.ValidFileRegex.Replace(sequence.ReleaseTemplateStageName, string.Empty));
            }

            this.deployerToolsPath = Path.Combine(targetPath, "DeployerTools");
            var tokenScriptPath = Path.Combine(this.deployerToolsPath, "TokenizationScript.ps1");

            this.fs.CreateDirectory(this.deployerToolsPath);

            if (!this.fs.Exists(tokenScriptPath))
            {
                var tokenScript = new TokenizationScript();
                this.fs.WriteAllText(tokenScriptPath, tokenScript.TransformText());
            }

            foreach (var container in sequence.Containers)
            {
                await this.ProcessContainerContainerAsync(sequence.ReleaseTemplateStageId, container, targetPath);
            }
        }

        private static ScriptAction CreateScriptAction(RMComponent component, ReleaseAction action)
        {
            return new ScriptAction
                       {
                           Enabled = action.Enabled, 
                           DisplayName = action.DisplayName, 
                           Sequence = action.Sequence, 
                           IsComponent = action.IsComponent, 
                           DeployerToolId = component.DeployerToolId, 
                           FileExtensionFilter = component.FileExtensionFilter, 
                           Command = component.Command, 
                           Arguments = component.Arguments, 
                           VariableReplacementMethod = component.VariableReplacementMethod, 
                           ConfigurationVariables = component.ConfigurationVariables
                       };
        }

        private static string CreateScriptFromTemplate(
            IEnumerable<ScriptAction> scriptElements, 
            IEnumerable<ScriptManualIntervention> manualInterventionElements, 
            IEnumerable<ConfigurationVariable> scriptParams, 
            bool generateInitScript)
        {
            var individualActions = new Dictionary<int, string>();
            var rollbackComponents =
                scriptElements.SelectMany(se => se.RollbackScripts.Values)
                    .SelectMany(se => se)
                    .Where(se => se.IsComponent && se.DeployerToolId != 0);

            var components =
                scriptElements.Where(se => se.IsComponent && se.DeployerToolId != 0).Union(rollbackComponents).ToList();
            foreach (var individualAction in scriptElements.ToList())
            {
                var actionTemplate = new IndividualActionTemplate
                                         {
                                             Session =
                                                 new Dictionary<string, object>
                                                     {
                                                         {
                                                             "action", 
                                                             individualAction
                                                         }, 
                                                         {
                                                             "components", 
                                                             components
                                                         }
                                                     }
                                         };
                actionTemplate.Initialize();
                individualActions.Add(individualAction.Sequence, actionTemplate.TransformText());
            }

            foreach (var manualIntervention in manualInterventionElements.ToList())
            {
                var manualInterventionTemplate = new ManualInterventionTemplate
                                                     {
                                                         Session =
                                                             new Dictionary<string, object>
                                                                 {
                                                                     {
                                                                         "manualIntervention", 
                                                                         manualIntervention
                                                                     }
                                                                 }
                                                     };
                manualInterventionTemplate.Initialize();
                individualActions.Add(manualIntervention.Sequence, manualInterventionTemplate.TransformText());
            }

            var releaseScript = new ReleaseScriptTemplate
                                    {
                                        Session =
                                            new Dictionary<string, object>
                                                {
                                                    {
                                                        "releaseActions", 
                                                        individualActions
                                                        .OrderBy(
                                                            ia =>
                                                            ia.Key)
                                                        .Select(
                                                            ia =>
                                                            ia.Value)
                                                    }, 
                                                    {
                                                        "components", 
                                                        components
                                                    }, 
                                                    {
                                                        "scriptParams", 
                                                        scriptParams
                                                        .ToList()
                                                    }, 
                                                    {
                                                        "generateInitializationScript", 
                                                        generateInitScript
                                                    }
                                                }
                                    };

            releaseScript.Initialize();
            return releaseScript.TransformText();
        }

        private static GenerationEventArgs GetActionGenerationArgs(ReleaseAction action, GenerationEventType eventType)
        {
            return new GenerationEventArgs
                       {
                           BlockType = action.ItemType, 
                           DisplayName = action.DisplayName, 
                           Sequence = action.Sequence, 
                           IsEnabled = action.Enabled, 
                           IsContainer = false, 
                           GenerationEventType = eventType
                       };
        }

        private static GenerationEventArgs GetContainerGenerationArgs(
            IReleaseActionContainer<IReleaseWorkflowBlock> container, 
            GenerationEventType eventType)
        {
            return new GenerationEventArgs
                       {
                           BlockType = container.ItemType, 
                           DisplayName = container.DisplayName, 
                           Sequence = container.Sequence, 
                           IsEnabled = true, 
                           IsContainer = true, 
                           GenerationEventType = eventType
                       };
        }

        private async Task<ScriptManualIntervention> CreateScriptManualIntervention(
            ManualIntervention manualIntervention)
        {
            string target;
            if (manualIntervention.IsGroup)
            {
                var groupData = await this.userRepo.GetGroupAsync(manualIntervention.UserId);
                target = groupData.Name;
            }
            else
            {
                var userData = await this.userRepo.GetUserAsync(manualIntervention.UserId);
                target = userData.Name;
            }

            return new ScriptManualIntervention
                       {
                           DisplayName = manualIntervention.DisplayName, 
                           Sequence = manualIntervention.Sequence, 
                           Enabled = manualIntervention.Enabled, 
                           InterventionText = manualIntervention.Text, 
                           Target = target, 
                           IsTargetGroup = manualIntervention.IsGroup
                       };
        }

        private async Task ProcessContainerContainerAsync(
            int stageId, 
            IReleaseActionContainer<IReleaseWorkflowBlock> container, 
            string targetPath)
        {
            this.ScriptGenerationNotification?.Invoke(this, GetContainerGenerationArgs(container, ContainerStart));

            var folderFormat = container.DisplayNameIsMeaningful
                                   ? this.meaningfulDisplayNameFolderFormat
                                   : this.meaninglessDisplayNameFolderFormat;
            var newPath = Path.Combine(
                targetPath, 
                string.Format(folderFormat, container.Sequence, container.ItemType, container.ValidFileName));
            this.fs.CreateDirectory(newPath);

            foreach (var subcontainer in
                container.SubItems.OfType<IReleaseActionContainer<IReleaseWorkflowBlock>>()
                    .Where(
                        subcontainer =>
                        subcontainer.ItemType != BlockType.Rollback && subcontainer.ItemType != BlockType.RollbackAlways))
            {
                // ScriptGenerationNotification?.Invoke(this, GetContainerGenerationArgs(subcontainer, ContainerStart));
                await this.ProcessContainerContainerAsync(stageId, subcontainer, newPath);

                // ScriptGenerationNotification?.Invoke(this, GetContainerGenerationArgs(subcontainer, ContainerEnd));
            }

            await this.ProcessReleaseActionContainerAsync(stageId, container.SubItems?.OfType<ReleaseAction>(), newPath);
            this.ScriptGenerationNotification?.Invoke(this, GetContainerGenerationArgs(container, ContainerEnd));
        }

        private async Task ProcessReleaseActionContainerAsync(
            int stageId, 
            IEnumerable<ReleaseAction> actions, 
            string targetPath)
        {
            if (!actions.Any())
            {
                return;
            }

            var scriptActionElements = new List<ScriptAction>();
            var scriptManualInterventionElements = new List<ScriptManualIntervention>();

            var enabledActions = actions.Where(a => a.Enabled);

            /* Find all of the actions that are not rollback blocks and create a ScriptAction that encompasses both the
               stuff extracted from the RM XAML workflow and stuff we pull out of the RM database.
            */
            foreach (var action in actions)
            {
                if (action is RollbackBlock)
                {
                    continue;
                }

                if (!action.Enabled)
                {
                    this.ScriptGenerationNotification?.Invoke(this, GetActionGenerationArgs(action, Warning));
                    continue;
                }

                this.ScriptGenerationNotification?.Invoke(this, GetActionGenerationArgs(action, ActionStart));

                if (action.ItemType == BlockType.ManualIntervention)
                {
                    scriptManualInterventionElements.Add(
                        await this.CreateScriptManualIntervention((ManualIntervention)action));
                }
                else
                {
                    var component = await this.componentRepo.GetComponentByIdAsync(action.WorkflowActivityId, stageId);
                    await this.deployerToolRepo.WriteToolToDiskAsync(component.DeployerToolId, this.deployerToolsPath);
                    scriptActionElements.Add(CreateScriptAction(component, action));
                }

                this.ScriptGenerationNotification?.Invoke(this, GetActionGenerationArgs(action, ActionEnd));
            }

            // Resolve the rollback actions within this container -- a rollback is a subcontainer full of actions, tied to a specific step (or all steps, in the case of rollback always)
            var rollbackActions = await this.ResolveRollbackActions(stageId, enabledActions, scriptActionElements);

            /* We'll need to resolve unique script parameters, since RM can have non-unique parameters. For example, 
                Action 1: __Hello __ = "placeholder"
                Action 2: __Hello__ = "Bar"
                We can't pass in a parameter called $Hello to a script, since it requires different values. 
                Thus, we need to flatten out the list of parameters across the entire server/tag container and "uniqueify" them.
            */
            var elementsToResolve =
                rollbackActions.SelectMany(ra => ra.Value).Union(scriptActionElements).OrderBy(s => s.Sequence);

            UniquePropertyResolver.ResolveProperties(elementsToResolve);

            // Invoke the T4 template for the container and for any rollback/rollback always blocks and write them out to disk
            var rollbackParameters =
                rollbackActions.SelectMany(s => s.Value).SelectMany(s => s.ConfigurationVariables).ToList();
            var scriptParameters = scriptActionElements.SelectMany(s => s.ConfigurationVariables).ToList();

            var releaseScriptText = CreateScriptFromTemplate(
                scriptActionElements, 
                scriptManualInterventionElements, 
                scriptParameters.Union(rollbackParameters).Distinct(new ConfigurationVariableEqualityComparer()), 
                this.generateInitializationScript);
            this.fs.WriteAllText(Path.Combine(targetPath, this.releaseScriptName), releaseScriptText);

            foreach (var rollbackGroup in rollbackActions)
            {
                string rollbackScriptName = $"{rollbackGroup.Key}.ps1";

                var rollbackScript = CreateScriptFromTemplate(
                    rollbackGroup.Value, 
                    scriptManualInterventionElements,
                    rollbackGroup.Value.SelectMany(s => s.ConfigurationVariables).Distinct(new ConfigurationVariableEqualityComparer()),
                    false);
                this.fs.WriteAllText(Path.Combine(targetPath, rollbackScriptName), rollbackScript);
            }

            if (this.generateInitializationScript)
            {
                var initScript = new InitializationScript
                                     {
                                         Session =
                                             new Dictionary<string, object>
                                                 {
                                                     {
                                                         "releaseActions", 
                                                         scriptActionElements
                                                     }, 
                                                     {
                                                         "scriptParams", 
                                                         scriptParameters
                                                         .Union(
                                                             rollbackParameters)
                                                         .Distinct(new ConfigurationVariableEqualityComparer())
                                                     }
                                                 }
                                     };
                initScript.Initialize();
                this.fs.WriteAllText(Path.Combine(targetPath, this.initScriptName), initScript.TransformText());
            }
        }

        private async Task<Dictionary<string, List<ScriptAction>>> ResolveRollbackActions(
            int stageId, 
            IEnumerable<ReleaseAction> actions, 
            IReadOnlyCollection<ScriptAction> scriptElements)
        {
            var rollbackActions = new Dictionary<string, List<ScriptAction>>();

            // Group the rollback action parameters
            var groupedRollbackActions =
                actions.OfType<RollbackBlock>()
                    .GroupBy(
                        g =>
                        string.Format(
                            g.DisplayNameIsMeaningful
                                ? this.meaningfulDisplayNameFolderFormat
                                : this.meaninglessDisplayNameFolderFormat, 
                            g.Sequence, 
                            g.ItemType, 
                            g.DisplayName))
                    .Select(s => new { s.Key, Item = s.First() })
                    .ToList();

            foreach (var rollbackGroup in groupedRollbackActions)
            {
                var rollbackScriptActions = new List<ScriptAction>();
                if (rollbackGroup.Item == null)
                {
                    continue;
                }

                /* Figure out which actions need rollback blocks
                   Normally, a rollback is attached to just the preceding action.
                   Rollback always blocks are attached to all of the actions in the script. Basically, if the script fails, it should always run that block.
                */
                IEnumerable<ScriptAction> scriptElementsToAttachRollbacks;
                if (rollbackGroup.Item.ItemType == BlockType.RollbackAlways)
                {
                    scriptElementsToAttachRollbacks = scriptElements.ToList();
                }
                else
                {
                    // Attach the rollback script to the action directly preceding it, and to all actions that happen after.
                    scriptElementsToAttachRollbacks =
                        scriptElements.Where(se => se.Sequence >= rollbackGroup.Item.Sequence - 1).ToList();
                }

                this.ScriptGenerationNotification?.Invoke(
                    this, 
                    GetContainerGenerationArgs(rollbackGroup.Item, ContainerStart));
                foreach (var rollbackAction in rollbackGroup.Item.SubItems.OfType<ReleaseAction>())
                {
                    this.ScriptGenerationNotification?.Invoke(
                        this, 
                        GetActionGenerationArgs(rollbackAction, ActionStart));
                    var component =
                        await this.componentRepo.GetComponentByIdAsync(rollbackAction.WorkflowActivityId, stageId);
                    await this.deployerToolRepo.WriteToolToDiskAsync(component.DeployerToolId, this.deployerToolsPath);
                    var action = CreateScriptAction(component, rollbackAction);
                    action.Sequence += rollbackGroup.Item.Sequence;
                    rollbackScriptActions.Add(action);
                    this.ScriptGenerationNotification?.Invoke(this, GetActionGenerationArgs(rollbackAction, ActionEnd));
                }

                foreach (var scriptElementToAttachRollback in scriptElementsToAttachRollbacks)
                {
                    scriptElementToAttachRollback.RollbackScripts.Add(rollbackGroup.Key, rollbackScriptActions);
                }

                rollbackActions.Add(rollbackGroup.Key, rollbackScriptActions);
                this.ScriptGenerationNotification?.Invoke(
                    this, 
                    GetContainerGenerationArgs(rollbackGroup.Item, ContainerEnd));
            }

            return rollbackActions;
        }
    }
}