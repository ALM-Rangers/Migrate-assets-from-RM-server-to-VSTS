﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReleaseTemplateParser.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Parser
{
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Parser.Model;

    public static class ReleaseTemplateParser
    {
        public static DeploymentSequence ToReleaseTemplate(this RMDeploymentSequence sequence)
        {
            var doc = XDocument.Parse(sequence.WorkflowXaml);

            if (doc.Root == null)
            {
                throw new InvalidDataException("XML is not a valid release template.");
            }

            if (doc.Root.Elements(ActionParser.ActionActivity).Any())
            {
                throw new UnsupportedReleaseTemplateTypeException();
            }

            var sequenceName = doc.Root?.Attribute("DisplayName").Value;
            var seq = new DeploymentSequence
            {
                ReleaseTemplateName = sequence.ReleaseTemplateName,
                ReleaseTemplateStageName = sequence.ReleaseTemplateStageName,
                DisplayName = sequenceName,
                ReleaseTemplateStageId = sequence.StageId,
                Containers = ContainerParser.ProcessContainers(doc.Root?.Elements())
            };
            return seq;
        }
    }
}
