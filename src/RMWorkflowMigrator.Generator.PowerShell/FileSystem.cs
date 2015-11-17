// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystem.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the FileSystem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;

    using Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell.Model;

    public class FileSystem : IFileSystem
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);

            if (new FileInfo(path).Extension != ".ps1")
            {
                return;
            }

            Collection<PSParseError> errors;
            PSParser.Tokenize(contents, out errors);

            if (errors.Any())
            {
                throw new ScriptGeneratorException(
                    "The script that was generated at path {path} contains syntax errors. This is likely an application bug.",
                    errors.ToDictionary(error => error.Token));
            }
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}