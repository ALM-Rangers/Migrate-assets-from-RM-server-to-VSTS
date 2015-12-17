// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UniquePropertyResolver.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell.Model;

    public static class UniquePropertyResolver
    {
        private static readonly Regex InvalidCharactersRegex = new Regex("[^0-9a-zA-Z_]");
        private static readonly Regex ParameterRegex = new Regex("__(.*?)__");

        public static IEnumerable<ScriptAction> ResolveProperties(IEnumerable<ScriptAction> actions)
        {
            var sequence = 2;
            var properties = new HashSet<Tuple<string, string>>();
            foreach (var action in actions)
            {
                action.Arguments = CleanActionParameters(action.Arguments);
                action.Command = CleanActionParameters(action.Command);
                CleanConfigurationValues(action.ConfigurationVariables);
                sequence = MakeParametersUnique(action, properties, sequence);
            }

            return actions;
        }

        private static int MakeParametersUnique(ScriptAction action, ISet<Tuple<string, string>> properties, int sequence)
        {
            if (action.ConfigurationVariables == null)
            {
                return sequence;
            }

            foreach (var configVar in action.ConfigurationVariables)
            {
                var tuple = Tuple.Create(CleanInvalidCharacters(configVar.OriginalName), configVar.Value);
                if (!properties.Contains(tuple))
                {
                    if (properties.Any(p => p.Item1 == tuple.Item1 && p.Item2 != tuple.Item2))
                    {
                        var newVariableName = configVar.RemappedName + sequence;
                        sequence++;
                        action.Arguments = action.Arguments?.Replace($"${configVar.RemappedName}", $"${newVariableName}");
                        var commandHasParameter = action.Command?.Contains(configVar.RemappedName);
                        if (commandHasParameter.HasValue && commandHasParameter.Value)
                        {
                            action.Command = action.Command?.Replace($"${configVar.RemappedName}", $"${newVariableName}");
                        }
                        configVar.RemappedName = newVariableName;
                    }
                }

                properties.Add(tuple);
            }

            return sequence;
        }

        private static void CleanConfigurationValues(IEnumerable<ConfigurationVariable> configurationVariables)
        {
            if (configurationVariables == null)
            {
                return;
            }

            foreach (var configVar in configurationVariables)
            {
                configVar.RemappedName = CleanInvalidCharacters(configVar.OriginalName);
            }
        }

        private static string CleanActionParameters(string arguments)
        {
            if (string.IsNullOrWhiteSpace(arguments))
            {
                return arguments;
            }

            var matches = ParameterRegex.Matches(arguments);
            foreach (var match in matches)
            {
                var replacement = match.ToString().Replace("__", string.Empty);
                replacement = "$" + CleanInvalidCharacters(replacement);
                arguments  = arguments.Replace(match.ToString(), replacement);
            }

            return arguments;
        }

        private static string CleanInvalidCharacters(string value)
        {
            return InvalidCharactersRegex.Replace(value, string.Empty);
        }
    }
}
