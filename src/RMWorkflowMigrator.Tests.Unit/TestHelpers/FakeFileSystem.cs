// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeFileSystem.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Tests.Unit.TestHelpers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell;

    [ExcludeFromCodeCoverage]
    public class FakeFileSystem : IFileSystem
    {
        public FakeFileSystem()
        {
            this.Directories = new HashSet<string>();
            this.Files = new Dictionary<string, string>();
        }

        public HashSet<string> Directories { get; set; }

        public Dictionary<string, string> Files { get; set; }

        public void CreateDirectory(string path)
        {
            this.Directories.Add(path);
        }

        public bool Exists(string path)
        {
            return this.Files.ContainsKey(path);
        }

        public void WriteAllText(string path, string contents)
        {
            this.Files.Add(path, contents);
        }
    }
}