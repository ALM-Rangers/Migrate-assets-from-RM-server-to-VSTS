// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationVariableEqualityComparer.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Defines the ConfigurationVariableEqualityComparer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model
{
    using System.Collections.Generic;

    public class ConfigurationVariableEqualityComparer : IEqualityComparer<ConfigurationVariable>
    {
        public bool Equals(ConfigurationVariable x, ConfigurationVariable y)
        {
            return x.RemappedName == y.RemappedName;
        }

        public int GetHashCode(ConfigurationVariable obj)
        {
            return obj.RemappedName.GetHashCode();
        }
    }
}