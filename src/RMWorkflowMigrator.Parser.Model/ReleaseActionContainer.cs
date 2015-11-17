// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReleaseActionContainer.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the ReleaseActionContainer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Parser.Model
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    [DebuggerDisplay("{ItemType}_{DisplayName}_{Sequence}")]
    public class ReleaseActionContainer : IReleaseActionContainer<IReleaseWorkflowBlock>
    {
        private static readonly Regex ValidFileRegex = new Regex(@"[^A-Za-z0-9 _&'@\{\}\[\],$=\!\-#\(\)%\.\+\~_]", RegexOptions.Compiled);

        public string DisplayName { get; set; }

        public string ValidFileName => ValidFileRegex.Replace(this.DisplayName, string.Empty);

        public bool DisplayNameIsMeaningful { get; set; } = true;

        public BlockType ItemType { get; set; }

        public int Sequence { get; set; }

        public IEnumerable<IReleaseWorkflowBlock> SubItems { get; set; }
    }
}