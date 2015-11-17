// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContainerParser.cs" company="Microsoft Corporation">
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

    public static class ContainerParser
    {
        public static EventHandler<ContainerParsedEventArgs> ContainerParsed;

        private static readonly XName Sequence = XName.Get("Sequence", @"http://schemas.microsoft.com/netfx/2009/xaml/activities");
        private static readonly XName Server = XName.Get("ServerActivity", @"clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow");
        private static readonly XName ServerTag = XName.Get("ServerTagActivity", @"clr-namespace:Microsoft.TeamFoundation.Release.Workflow.Activities;assembly=Microsoft.TeamFoundation.Release.Workflow");
        private static readonly XName Parallel = XName.Get("Parallel", @"http://schemas.microsoft.com/netfx/2009/xaml/activities");

        private static readonly Dictionary<XName, Func<XElement, IReleaseActionContainer<IReleaseWorkflowBlock>>> ContainerFactory = new Dictionary<XName, Func<XElement, IReleaseActionContainer<IReleaseWorkflowBlock>>>
        {
            { Sequence, ProcessSequence },
            { Server, ProcessServer },
            { ServerTag, ProcessServerTag },
            { Parallel, ProcessParallel }
        };

        internal static IEnumerable<IReleaseActionContainer<IReleaseWorkflowBlock>> ProcessContainers(IEnumerable<XElement> nodes)
        {
            int sequence = 0;

            var results = new List<IReleaseActionContainer<IReleaseWorkflowBlock>>();
            foreach (var item in
                nodes.Where(node => ContainerFactory.ContainsKey(node.Name))
                    .Select(node => ContainerFactory[node.Name](node)))
            {
                item.Sequence = ++sequence;

                results.Add(item);
            }

            return results;
        }

        private static IReleaseActionContainer<IReleaseWorkflowBlock> ProcessServerTag(XElement element)
        {
            var container = new ReleaseActionContainer
            {
                DisplayName = element.Attribute("TagName").Value,
                DisplayNameIsMeaningful = true,
                ItemType = BlockType.ServerTag
            };

            ContainerParsed?.Invoke(
                null,
                new ContainerParsedEventArgs { DisplayName = container.DisplayName, ItemType = container.ItemType });

            container.SubItems = ActionParser.ProcessActions(element.Elements());

            return container;
        }

        private static IReleaseActionContainer<IReleaseWorkflowBlock> ProcessServer(XElement element)
        {
            var container = new ReleaseActionContainer
            {
                DisplayName = element.Attribute("ServerName").Value,
                DisplayNameIsMeaningful = true,
                ItemType = BlockType.Server
            };

            ContainerParsed?.Invoke(
                null,
                new ContainerParsedEventArgs { DisplayName = container.DisplayName, ItemType = container.ItemType });

            container.SubItems = ActionParser.ProcessActions(element.Elements());

            return container;
        }

        private static IReleaseActionContainer<IReleaseWorkflowBlock> ProcessSequence(XElement element)
        {
            var container = new ReleaseActionContainer
            {
                DisplayName = element.Attribute("DisplayName").Value,
                DisplayNameIsMeaningful = true,
                ItemType = BlockType.Sequence
            };

            ContainerParsed?.Invoke(
                null,
                new ContainerParsedEventArgs { DisplayName = container.DisplayName, ItemType = container.ItemType });

            container.SubItems = ProcessContainers(element.Elements());

            return container;
        }

        private static IReleaseActionContainer<IReleaseWorkflowBlock> ProcessParallel(XElement element)
        {
            var container = new ReleaseActionContainer
            {
                DisplayName = element.Attribute("DisplayName").Value,
                DisplayNameIsMeaningful = false,
                ItemType = BlockType.Parallel,
            };

            ContainerParsed?.Invoke(
                null,
                new ContainerParsedEventArgs { DisplayName = container.DisplayName, ItemType = container.ItemType });

            container.SubItems = ProcessContainers(element.Elements());

            return container;
        }
    }
}