namespace EnergyTrading.Console
{
    using System;

    /// <summary>
    /// Implements a basic command-line switch by taking the
    /// switching name and the associated description.
    /// </summary>
    /// <remark>
    /// Based on http://www.codeproject.com/csharp/commandlineparser.asp
    /// Only currently is implemented for properties, so all
    /// auto-switching variables should have a get/set method supplied.
    /// </remark>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CommandLineSwitchAttribute : Attribute
    {
        private readonly string name;
        private readonly string description;

        /// <summary>
        /// Attribute constructor.
        /// </summary>
        public CommandLineSwitchAttribute(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        /// <summary>
        /// Accessor for retrieving the switch-name for an associated
        /// property.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Accessor for retrieving the description for a switch of
        /// an associated property.
        /// </summary>
        public string Description
        {
            get { return this.description; }
        }
    }
}