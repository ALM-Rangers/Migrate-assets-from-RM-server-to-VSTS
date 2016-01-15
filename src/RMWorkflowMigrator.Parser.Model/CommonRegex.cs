namespace Microsoft.ALMRangers.RMWorkflowMigrator.Parser.Model
{
    using System.Text.RegularExpressions;

    public static class CommonRegex
    {
        public static readonly Regex ValidFileRegex = new Regex(@"[^A-Za-z0-9 _&'@\{\}\[\],$=\!\-#\(\)%\.\+\~_]", RegexOptions.Compiled);
        public static readonly Regex InvalidCharactersRegex = new Regex("[^0-9a-zA-Z_]", RegexOptions.Compiled);
        public static readonly Regex ParameterRegex = new Regex("__(.*?)__", RegexOptions.Compiled);
    }
}
