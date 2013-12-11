namespace EnergyTrading.Console
{
    using System;

    /// <summary>
    /// This class implements an alias attribute to work in conjunction
    /// with the <see cref="CommandLineSwitchAttribute">CommandLineSwitchAttribute</see>
    /// attribute.  If the CommandLineSwitchAttribute exists, then this attribute
    /// defines an alias for it.
    /// </summary>
    /// <remarks>Based on http://www.codeproject.com/csharp/commandlineparser.asp</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class CommandLineAliasAttribute : Attribute
    {
        private readonly string alias;

        public CommandLineAliasAttribute(string alias)
        {
            this.alias = alias;
        }

        public string Alias
        {
            get { return this.alias; }
        }
    }
}