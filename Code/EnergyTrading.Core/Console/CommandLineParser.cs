namespace EnergyTrading.Console
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Implementation of a command-line parsing class.  Is capable of
    /// having switches registered with it directly or can examine a registered
    /// class for any properties with the appropriate attributes appended to
    /// them.
    /// </summary>
    /// <remarks>Based on http://www.codeproject.com/csharp/commandlineparser.asp</remarks>
    public class CommandLineParser
    {
        private readonly string commandLine;
        private string applicationName = string.Empty;
        private string workingString = string.Empty;
        private string[] splitParameters;
        private List<Switch> switches;

        /// <summary>
        /// Initializes a new instance of the CommandLineParser class.
        /// </summary>
        /// <param name="commandLine">The command line to parse</param>
        public CommandLineParser(string commandLine) : this(commandLine, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CommandLineParser class.
        /// </summary>
        /// <param name="commandLine">The command line to pars</param>
        /// <param name="autoAttributeClass">Class instance that has attributed properties for command line switches</param>
        public CommandLineParser(string commandLine, object autoAttributeClass)
        {
            this.commandLine = commandLine;

            if (autoAttributeClass != null)
            {
                this.LocateAutoAttributes(autoAttributeClass);
            }
        }

        public string ApplicationName
        {
            get { return this.applicationName; }
        }

        public string[] Parameters
        {
            get { return this.splitParameters; }
        }

        public SwitchInfo[] Switches
        {
            get
            {
                if (this.SwitchList == null)
                {
                    return null;
                }

                var si = new SwitchInfo[this.SwitchList.Count];
                for (var i = 0; i < this.SwitchList.Count; i++)
                {
                    si[i] = new SwitchInfo(this.SwitchList[i]);
                }

                return si;
            }
        }

        /// <summary>
        /// This function returns a list of the unhandled switches
        /// that the parser has seen, but not processed.
        /// </summary>
        /// <remark>
        /// The unhandled switches are not removed from the remainder
        /// of the command-line.
        /// </remark>
        public string[] UnhandledSwitches
        {
            get
            {
                // TODO: Move to the top and compile it!
                const string SwitchPattern = @"(\s|^)(?<match>(-{1,2}|/)(.+?))(?=(\s|$))";
                var r = new Regex(SwitchPattern, RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
                var m = r.Matches(this.workingString);
                if (m.Count == 0)
                {
                    return null;
                }

                var unhandled = new string[m.Count];
                for (int i = 0; i < m.Count; i++)
                {
                    unhandled[i] = m[i].Groups["match"].Value;
                }

                return unhandled;
            }
        }

        private List<Switch> SwitchList
        {
            get
            {
                if (this.switches == null)
                {
                    this.switches = new List<Switch>();
                }

                return this.switches;
            }
        }

        public object this[string name]
        {
            get
            {
                if (this.SwitchList != null)
                {
                    for (var i = 0; i < this.SwitchList.Count; i++)
                    {
                        if (string.Compare(this.SwitchList[i].Name, name, true, CultureInfo.CurrentCulture) == 0)
                        {
                            return this.SwitchList[i].Value;
                        }
                    }
                }

                return null;
            }
        }

        public void AddSwitch(string name, string description)
        {
            var rec = new Switch(name, description);
            this.SwitchList.Add(rec);
        }

        public void AddSwitch(string[] names, string description)
        {
            var rec = new Switch(names[0], description);
            for (var s = 1; s < names.Length; s++)
            {
                rec.AddAlias(names[s]);
            }

            this.SwitchList.Add(rec);
        }

        public bool Parse()
        {
            this.ExtractApplicationName();

            // Remove switches and associated info.
            this.HandleSwitches();

            // Split parameters.
            this.SplitParameters();

            return true;
        }

        public object InternalValue(string name)
        {
            if (this.SwitchList != null)
            {
                for (var i = 0; i < this.SwitchList.Count; i++)
                {
                    if (string.Compare(this.SwitchList[i].Name, name, true, CultureInfo.CurrentCulture) == 0)
                    {
                        return this.SwitchList[i].InternalValue;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Shows the command-line usage based on the defined switches
        /// </summary>
        /// <returns></returns>
        public string DisplayUsage()
        {
            var builder = new StringBuilder();

            builder.Append(this.ApplicationName);
            builder.AppendLine(" Usage:");

            foreach (var sw in this.SwitchList)
            {
                // NB Strips off the \ value on things like \? - needed to avoid problems with RegEx
                builder.AppendLine(
                        string.Format(
                                CultureInfo.CurrentCulture,
                                "\\{0} - {1}",
                                sw.Name.Replace("\\", string.Empty),
                                sw.Description));
            }

            return builder.ToString();
        }

        private void LocateAutoAttributes(object autoAttributeClass)
        {
            var type = autoAttributeClass.GetType();
            var members = type.GetMembers();

            for (var i = 0; i < members.Length; i++)
            {
                // Get the property information.  We're only handling properties at the moment!
                if (!(members[i] is PropertyInfo))
                {
                    continue;
                }

                // And we need some attributes
                var attributes = members[i].GetCustomAttributes(false);
                if (attributes.Length <= 0)
                {
                    continue;
                }

                Switch rec = this.FindSwitch(autoAttributeClass, members[i] as PropertyInfo, attributes);
                if (rec != null)
                {
                    // Assuming we have a switch record (that may or may not have
                    // aliases), add it to the collection of switches.
                    this.SwitchList.Add(rec);
                }
            }
        }

        private Switch FindSwitch(object autoAttributeClass, PropertyInfo member, object[] attributes)
        {
            Switch rec = null;

            foreach (var attribute in this.TypedAttributes<CommandLineSwitchAttribute>(attributes))
            {
                rec = new Switch(attribute.Name, attribute.Description, member.PropertyType);

                // Map in the Get/Set methods.
                rec.SetMethod = member.GetSetMethod();
                rec.GetMethod = member.GetGetMethod();
                rec.PropertyOwner = autoAttributeClass;

                // Go find any aliases
                this.CaptureAliases(rec, attributes);

                // Can only handle a single switch for each property
                // (otherwise the parsing of aliases gets silly...)
                break;
            }

            return rec;
        }

        private void CaptureAliases(Switch rec, IEnumerable<object> attributes)
        {
            foreach (Attribute attribute in attributes)
            {
                if (attribute is CommandLineAliasAttribute)
                {
                    rec.AddAlias(((CommandLineAliasAttribute)attribute).Alias);
                }
            }
        }

        private IEnumerable<TAttr> TypedAttributes<TAttr>(IEnumerable<object> attributes)
            where TAttr : class
        {
            return attributes.OfType<TAttr>().Select(attribute => attribute);
        }

        private void ExtractApplicationName()
        {
            const string ApplicationPattern = @"^(?<applicationName>(""[^""]+""|(\S)+))(?<remainder>.+)";

            var r = new Regex(ApplicationPattern, RegexOptions.ExplicitCapture);
            var m = r.Match(this.commandLine);
            if (m != null && m.Groups["applicationName"] != null)
            {
                this.applicationName = m.Groups["applicationName"].Value;
                this.workingString = m.Groups["remainder"].Value;
            }
        }

        private void SplitParameters()
        {
            // Populate the split parameters array with the remaining parameters.
            // Note that if quotes are used, the quotes are removed.
            // e.g.   one two three "four five six"
            // 0 - one
            // 1 - two
            // 2 - three
            // 3 - four five six
            // (e.g. 3 is not in quotes).
            var r = new Regex(@"((\s*(""(?<param>[^""]+?)""|(?<param>\S+))))", RegexOptions.ExplicitCapture);
            var m = r.Matches(this.workingString);

            if (m.Count <= 0)
            {
                return;
            }

            this.splitParameters = new string[m.Count];
            for (int i = 0; i < m.Count; i++)
            {
                this.splitParameters[i] = m[i].Groups["param"].Value;
            }
        }

        private void HandleSwitches()
        {
            if (this.SwitchList == null)
            {
                return;
            }

            foreach (var s in this.SwitchList)
            {
                var r = new Regex(s.Pattern, RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
                var m = r.Matches(this.workingString);
                for (var i = 0; i < m.Count; i++)
                {
                    string value = null;
                    if (m[i].Groups != null && m[i].Groups["value"] != null)
                    {
                        value = m[i].Groups["value"].Value;
                    }

                    if (s.Type == typeof(bool))
                    {
                        bool state = true;

                        // The value string may indicate what value we want.
                        if (m[i].Groups != null && m[i].Groups["value"] != null)
                        {
                            switch (value)
                            {
                                case "+":
                                    state = true;
                                    break;

                                case "-":
                                    state = false;
                                    break;

                                case "":
                                    if (s.ReadValue != null)
                                    {
                                        state = !(bool)s.ReadValue;
                                    }

                                    break;

                                default:
                                    break;
                            }
                        }

                        s.Notify(state);
                        break;
                    }

                    this.UpdateValue(s, value);
                }

                this.workingString = r.Replace(this.workingString, " ");
            }
        }

        private void UpdateValue(Switch s, string value)
        {
            if (s.Type == typeof(string))
            {
                s.Notify(value);
            }
            else if (s.Type == typeof(int))
            {
                s.Notify(int.Parse(value));
            }
            else if (s.Type.IsEnum)
            {
                s.Notify(Enum.Parse(s.Type, value, true));
            }
        }

        /// <summary>
        /// A simple internal class for passing back to the caller
        /// some information about the switch.  The internals/implementation
        /// of this class has privileged access to the contents of the
        /// SwitchRecord class.
        /// </summary>
        public class SwitchInfo
        {
            private readonly Switch @switch;

            /// <summary>
            /// Initializes a new instance of the SwitchInfo class.
            /// <para>
            /// In order to hide to the outside world
            /// information not necessary to know, the constructor takes a System.Object (aka
            /// object) as it's registering type.  If the type isn't of the correct type, an exception
            /// is thrown.
            /// </para>
            /// </summary>
            /// <param name="rec">The SwitchRecord for which this class store information.</param>
            /// <exception cref="ArgumentException">Thrown if the rec parameter is not of
            /// the type SwitchRecord.</exception>
            public SwitchInfo(object rec)
            {
                if (rec is Switch)
                {
                    this.@switch = rec as Switch;
                }
                else
                {
                    throw new ArgumentException();
                }
            }

            public string Name
            {
                get { return this.@switch.Name; }
            }

            public string Description
            {
                get { return this.@switch.Description; }
            }

            public string[] Aliases
            {
                get { return this.@switch.Aliases; }
            }

            public Type Type
            {
                get { return this.@switch.Type; }
            }

            public object Value
            {
                get { return this.@switch.Value; }
            }

            public object InternalValue
            {
                get { return this.@switch.InternalValue; }
            }

            public bool IsEnum
            {
                get { return this.@switch.Type.IsEnum; }
            }

            public string[] Enumerations
            {
                get { return this.@switch.Enumerations; }
            }
        }

        /// <summary>
        /// The SwitchRecord is stored within the parser's collection of registered
        /// switches.  This class is private to the outside world.
        /// </summary>
        private sealed class Switch
        {
            private readonly Type switchType;
            private string name;
            private string description;
            private object value;
            private ArrayList aliases;
            private string pattern;

            // The following advanced functions allow for callbacks to be
            // made to manipulate the associated data type.
            private MethodInfo setMethod;
            private MethodInfo getMethod;
            private object propertyOwner;

            /// <summary>
            /// Initializes a new instance of the Switch class.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="description"></param>
            public Switch(string name, string description) : this(name, description, typeof(bool))
            {
            }

            /// <summary>
            /// Initializes a new instance of the Switch class.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="description"></param>
            /// <param name="type"></param>
            public Switch(string name, string description, Type type)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("name");
                }

                switch (name)
                {
                    case "?":
                    case ".":
                    case "\\":
                        throw new ArgumentOutOfRangeException(
                                "name", "Cannot support switich of " + name + " - clashes with regular expression parsing");
                }

                if (type == typeof(bool) || type == typeof(string) || type == typeof(int) || type.IsEnum)
                {
                    this.switchType = type;
                    this.Initialize(name, description);
                }
                else
                {
                    throw new ArgumentException("Currently only Ints, Bool and Strings are supported");
                }
            }

            public string Name
            {
                get { return this.name; }
            }

            public string[] Aliases
            {
                get { return this.aliases != null ? (string[])this.aliases.ToArray(typeof(string)) : null; }
            }

            public string Description
            {
                get { return this.description; }
            }

            public Type Type
            {
                get { return this.switchType; }
            }

            public object InternalValue
            {
                get { return this.value; }
            }

            public object ReadValue
            {
                get
                {
                    object o = null;
                    if (this.propertyOwner != null && this.getMethod != null)
                    {
                        o = this.getMethod.Invoke(this.propertyOwner, null);
                    }

                    return o;
                }
            }

            public object Value
            {
                get { return this.ReadValue ?? this.value; }
            }

            public string Pattern
            {
                get { return this.pattern; }
            }

            public MethodInfo SetMethod
            {
                set { this.setMethod = value; }
            }

            public MethodInfo GetMethod
            {
                set { this.getMethod = value; }
            }

            public object PropertyOwner
            {
                set { this.propertyOwner = value; }
            }

            public string[] Enumerations
            {
                get { return this.switchType.IsEnum ? Enum.GetNames(this.switchType) : null; }
            }

            public void AddAlias(string alias)
            {
                if (string.IsNullOrEmpty(alias))
                {
                    throw new ArgumentNullException("alias");
                }

                switch (alias)
                {
                    case "?":
                    case ".":
                    case "\\":
                        throw new ArgumentOutOfRangeException(
                                "alias", "Cannot support alias of " + alias + " - clashes with regular expression parsing");
                }

                if (this.aliases == null)
                {
                    this.aliases = new ArrayList();
                }

                this.aliases.Add(alias);

                this.BuildPattern();
            }

            public void Notify(object value)
            {
                if (this.propertyOwner != null && this.setMethod != null)
                {
                    object[] parameters = new object[1];
                    parameters[0] = value;
                    this.setMethod.Invoke(this.propertyOwner, parameters);
                }

                this.value = value;
            }

            private void Initialize(string name, string description)
            {
                this.name = name;
                this.description = description;

                this.BuildPattern();
            }

            private void BuildPattern()
            {
                string matchString = this.Name;

                if (this.Aliases != null && this.Aliases.Length > 0)
                {
                    foreach (string s in this.Aliases)
                    {
                        matchString += "|" + s;
                    }
                }

                const string PatternStart = @"(\s|^)(?<match>(-{1,2}|/)(";
                string patternEnd; // To be defined below.

                // The common suffix ensures that the switches are followed by
                // a white-space OR the end of the string.  This will stop
                // switches such as /help matching /helpme
                const string CommonSuffix = @"(?=(\s|$))";

                if (this.Type == typeof(bool))
                {
                    // patternEnd = @")(?<value>(\+|-){0,1}))";
                    patternEnd = @")(?<value>[-+]?))";
                }
                else if (this.Type == typeof(string))
                {
                    // patternEnd = @")(?::|\s+))((?:"")(?<value>[^""]+)(?:"")|(?<value>\S+))";
                    patternEnd = @")(?::|\s+))((?:"")(?<value>[^\""]+)(?:"")|(?<value>\S+))";
                }
                else if (this.Type == typeof(int))
                {
                    // patternEnd = @")(?::|\s+))((?<value>(-|\+)[0-9]+)|(?<value>[0-9]+))";
                    patternEnd = @")(?::|\s+))(?<value>[-+]?[0-9]+)";
                }
                else if (this.Type.IsEnum)
                {
                    string[] enumNames = this.Enumerations;
                    string enumStr = enumNames[0];
                    for (int e = 1; e < enumNames.Length; e++)
                    {
                        enumStr += "|" + enumNames[e];
                    }

                    patternEnd = @")(?::|\s+))(?<value>" + enumStr + @")";
                }
                else if (this.Type == typeof(float) || this.Type == typeof(double) || this.Type == typeof(decimal))
                {
                    patternEnd = @")(?::|\s+))(?<value>[-+]?[0-9]*\.?[0-9]+)";
                }
                else
                {
                    throw new ArgumentException();
                }

                // Set the internal regular expression pattern.
                this.pattern = PatternStart + matchString + patternEnd + CommonSuffix;
            }
        }
    }
}