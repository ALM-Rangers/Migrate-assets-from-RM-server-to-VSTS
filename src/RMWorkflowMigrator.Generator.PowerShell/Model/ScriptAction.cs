// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptAction.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell.Model
{
    using System.Collections.Generic;
    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model;

    public class ScriptAction
    {
        public string DisplayName { get; set; }

        public bool Enabled { get; set; }

        public bool IsComponent { get; set; }

        public int DeployerToolId { get; set; }

        public string FileExtensionFilter { get; set; }

        public string Command { get; set; }

        public string Arguments { get; set; }

        public VariableReplacementMethod VariableReplacementMethod { get; set; }

        public IEnumerable<ConfigurationVariable> ConfigurationVariables { get; set; } = new List<ConfigurationVariable>();

        public int Sequence { get; set; }

        public Dictionary<string, List<ScriptAction>> RollbackScripts { get; set; } = new Dictionary<string, List<ScriptAction>>();

        public bool CommandIsExtractedTool { get; set; }  
    }
}
