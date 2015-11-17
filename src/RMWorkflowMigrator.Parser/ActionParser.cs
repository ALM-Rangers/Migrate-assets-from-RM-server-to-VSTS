// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionParser.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Parser.Model;

    public static class ActionParser
    {
        public static EventHandler<ActionParsedEventArgs> ActionParsed;

        private static readonly XName ActionActivity = XName.Get("ActionActivity", @"clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow");

        private static readonly XName ComponentActivity = XName.Get("ComponentActivity", @"clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow");

        private static readonly XName Rollback = XName.Get("RollbackActivity", "clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow");

        private static readonly XName RollbackAlways = XName.Get("RollbackAlwaysActivity", "clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow");

        private static readonly XName ManualIntervention = XName.Get("ManualInterventionActivity", "clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow");

        private static readonly Dictionary<XName, Func<XElement, ReleaseAction>> ActionFactory = new Dictionary<XName, Func<XElement, ReleaseAction>>
                                                                                                     {
                                                                                                         { ActionActivity, ParseReleaseAction },
                                                                                                         { ComponentActivity, ParseReleaseAction },
                                                                                                         { Rollback, ParseRollback },
                                                                                                         { RollbackAlways, ParseRollback },
                                                                                                         { ManualIntervention, ParseManualIntervention }
                                                                                                     };

        internal static IEnumerable<ReleaseAction> ProcessActions(IEnumerable<XElement> nodes)
        {
            int sequence = 0;

            var results = new List<ReleaseAction>();
            foreach (var item in
                nodes.Where(node => ActionFactory.ContainsKey(node.Name))
                    .Select(node => ActionFactory[node.Name](node)))
            {
                item.Sequence = ++sequence;
                results.Add(item);
            }

            return results;
        }

        internal static void FireActionParsedEvent(ReleaseAction action)
        {
            ActionParsed?.Invoke(
                null,
                new ActionParsedEventArgs { DisplayName = action.DisplayName, ItemType = action.ItemType });
        }

        private static ReleaseAction ParseReleaseAction(XElement node)
        {
            var stageActivity = XName.Get("StageActivity", "clr-namespace:Microsoft.TeamFoundation.Release.Data.Model;assembly=Microsoft.TeamFoundation.Release.Data");
            var firstNodeDescendant = node.Descendants(stageActivity).First();
            var componentId = int.Parse(firstNodeDescendant.Attribute("ComponentId").Value);
            var workflowActivityId = Guid.Parse(firstNodeDescendant.Attribute("WorkflowActivityId").Value);
            var itemType = node.Name.LocalName == ActionActivity.LocalName
                ? BlockType.Action
                : node.Name.LocalName == ComponentActivity.LocalName ? BlockType.Component : BlockType.Undefined;

            var action = new ReleaseAction
            {
                DisplayName = node.Attribute("DisplayName").Value,
                DisplayNameIsMeaningful = true,
                Enabled = !bool.Parse(node.Attribute("IsSkipped").Value),
                ItemType = itemType,
                ComponentId = componentId,
                WorkflowActivityId = workflowActivityId
            };

            FireActionParsedEvent(action);

            return action;
        }

        private static ReleaseAction ParseRollback(XElement node)
        {
            var itemType = node.Name.LocalName == Rollback.LocalName
            ? BlockType.Rollback
            : node.Name.LocalName == RollbackAlways.LocalName ? BlockType.RollbackAlways : BlockType.Undefined;

            var rollback = new RollbackBlock
            {
                DisplayName = node.Attribute("DisplayName").Value,
                DisplayNameIsMeaningful = false,
                ItemType = itemType,
                SubItems = ProcessActions(node.Elements())
            };

            FireActionParsedEvent(rollback);

            return rollback;
        }

        private static ReleaseAction ParseManualIntervention(XElement node)
        {
            var idElement = node.Attribute("Recipient").Value;
            var splitId = idElement.Split(':');
            
            var parsedManualIntervention = new ManualIntervention
                                               {
                                                   DisplayName = node.Attribute("DisplayName").Value,
                                                   DisplayNameIsMeaningful = true,
                                                   ItemType = BlockType.ManualIntervention,
                                                   Text = node.Attribute("Details").Value,
                                                   IsGroup = int.Parse(splitId[0]) == 2,
                                                   UserId = int.Parse(splitId[1])
                                               };

            FireActionParsedEvent(parsedManualIntervention);
            return parsedManualIntervention;
        }
    }
}