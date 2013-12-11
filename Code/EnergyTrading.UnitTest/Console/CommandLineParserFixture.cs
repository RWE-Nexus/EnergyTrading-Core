namespace EnergyTrading.UnitTest.Console
{
    using System;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Console;
    using EnergyTrading.Test;

    [TestClass]
    public class CommandLineParserFixture : Fixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IllegalSwitch()
        {
            BadSwitch config = new BadSwitch();
            CommandLineParser parser = new CommandLineParser(string.Empty, config);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IllegalAlias()
        {
            BadAlias config = new BadAlias();
            CommandLineParser parser = new CommandLineParser(string.Empty, config);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmptySwitch()
        {
            StringEmptySwitch config = new StringEmptySwitch();
            CommandLineParser parser = new CommandLineParser(string.Empty, config);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmptyAlias()
        {
            StringEmptyAlias config = new StringEmptyAlias();
            CommandLineParser parser = new CommandLineParser(string.Empty, config);
        }

        [TestMethod]
        public void NullCommandLine()
        {
            Check(string.Empty, "My Name", "XXXX", false, 999, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void StringSwitch()
        {
            Check("/Name:Fred", "Fred", "XXXX", false, 999, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void SecondStringSwitch()
        {
            Check("/Test:Fred", "My Name", "Fred", false, 999, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void IntSwitch()
        {
            Check("/Age:10", "My Name", "XXXX", false, 10, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void NegativeIntSwitch()
        {
            Check("/Age:-10", "My Name", "XXXX", false, -10, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void PositiveIntSwitch()
        {
            Check("/Age:+10", "My Name", "XXXX", false, 10, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void BoolSwitch()
        {
            Check("/Wibble+", "My Name", "XXXX", true, 999, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void NakedBoolSwitch()
        {
            Check("/Wibble", "My Name", "XXXX", true, 999, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void NakedBoolSwitchAlias()
        {
            Check("/w", "My Name", "XXXX", true, 999, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void NegativeBoolSwitch()
        {
            Application app = new Application();
            app.Wibble = true;
            Assert.AreEqual(true, app.Wibble, "Wibble - A");
            Check(app, "/Wibble-", "My Name", "XXXX", false, 999, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void EnumSwitch()
        {
            Check("/day:Fri", "My Name", "XXXX", false, 999, Application.DaysOfWeek.Fri);
        }

        [TestMethod]
        public void UseSwitchAlias()
        {
            Check("/User:Fred", "Fred", "XXXX", false, 999, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void ColonQuotedString()
        {
            Check("/Name:\"Fred Jones\"", "Fred Jones", "XXXX", false, 999, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void SpaceQuotedString()
        {
            Check("/Name \"Fred Jones\"", "Fred Jones", "XXXX", false, 999, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void StringWithSlash()
        {
            Check(@"/Test:C:\Test\", "My Name", @"C:\Test\", false, 999, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void StringWithEmbeddedSwitch()
        {
            Check(@"/Test:""C:\Test\ /b:Fred""", "My Name", @"C:\Test\ /b:Fred", false, 999, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void TwoStringWithEmbeddedSwitch()
        {
            Check(@"/Test:""/Name foo"" /Name:""bar""", "bar", @"/Name foo", false, 999, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void QuotedStringWithSlash()
        {
            Check(@"/Test ""C:\Test Lab\""", "My Name", @"C:\Test Lab\", false, 999, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void AliasedColonQuotedString()
        {
            Check("/User:\"Fred Jones\"", "Fred Jones", "XXXX", false, 999, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void AliasedSpaceQuotedString()
        {
            Check("/User \"Fred Jones\"", "Fred Jones", "XXXX", false, 999, Application.DaysOfWeek.Sun);
        }

        [TestMethod]
        public void DisplayUsageNotNull()
        {
            CommandLineParser p = Check(string.Empty, "My Name", "XXXX", false, 999, Application.DaysOfWeek.Sun);
            Assert.IsNotNull(p.DisplayUsage());
        }

        [TestMethod]
        public void ExtraParams()
        {
            CommandLineParser p = Check("/Name:Fred /Jim:Test /Bob:10", "Fred", "XXXX", false, 999, Application.DaysOfWeek.Sun);
            string[] unhandled = p.UnhandledSwitches;
            Assert.AreEqual(2, unhandled.Length);
            Assert.AreEqual(2, p.Parameters.Length);
        }

        [TestMethod]
        public void ExtraParamsNeedColons()
        {
            CommandLineParser p = Check("/Name:Fred /Jim Test /Bob 10", "Fred", "XXXX", false, 999, Application.DaysOfWeek.Sun);
            string[] unhandled = p.UnhandledSwitches;
            Assert.AreEqual(2, unhandled.Length);
            Assert.AreEqual(4, p.Parameters.Length);
        }

        private static CommandLineParser Check(string commandLine, string userName, string test, bool wibble, int age, Application.DaysOfWeek dow)
        {
            Application app = new Application();
            return Check(app, commandLine, userName, test, wibble, age, dow);
        }

        private static CommandLineParser Check(Application app, string commandLine, string userName, string test, bool wibble, int age, Application.DaysOfWeek dow)
        {
            CommandLineParser parser = app.Run(@"""C:\Test\Apps\Test.exe"" " + commandLine);
            Assert.AreEqual(@"""C:\Test\Apps\Test.exe""", parser.ApplicationName);
            Check(app, userName, test, wibble, age, dow);

            return parser;
        }

        private static void Check(
                Application app, string userName, string test, bool wibble, int age, Application.DaysOfWeek dow)
        {
            Assert.AreEqual(userName, app.UserName, "UserName differs");
            Assert.AreEqual(test, app.Test, "Test differs");
            Assert.AreEqual(wibble, app.Wibble, "Wibble differs");
            Assert.AreEqual(age, app.Age, "Age differs");
            Assert.AreEqual(dow, app.DoW, "Dow differs");
        }

        /// <summary>
        /// The application call acts as a tester for the command line
        /// parser.  It demonstrates using switch attributes on properties
        /// meaning that the coder does not have to implement anything except
        /// instantiating the parser in the most basic way.
        /// </summary>
        private sealed class Application
        {
            public enum DaysOfWeek
            {
                Sun,
                Mon,
                Tue,
                Wed,
                Thu,
                Fri,
                Sat
            }

            private bool showHelp;
            private bool wibble;
            private string someName = "My Name";
            private int age = 999;
            private string test = "XXXX";
            private DaysOfWeek doW = DaysOfWeek.Sun;

            /// <summary>
            /// Simple example of a Boolean switch.
            /// </summary>
            [CommandLineSwitch("Help", "Show help")]
            public bool ShowSomeHelp
            {
                get { return this.showHelp; }
                set { this.showHelp = value; }
            }

            /// <summary>
            /// Simple example of a Boolean switch.
            /// </summary>
            /// <remark>
            /// There is no get value set, so this value is in effect
            /// a write only one.  This can affect the implementation of the toggle
            /// for Boolean values.
            /// </remark>
            [CommandLineSwitch("Wibble", "Do something else")]
            [CommandLineAlias("w")]
            public bool Wibble
            {
                get { return this.wibble; }
                set { this.wibble = value; }
            }

            /// <summary>
            /// Simple example of a string switch with an alias.
            /// </summary>
            [CommandLineSwitch("Name", "User Name")]
            [CommandLineAlias("User")]
            public string UserName
            {
                get { return this.someName; }
                set { this.someName = value; }
            }

            /// <summary>
            /// Simple example of an integer switch.
            /// </summary>
            [CommandLineSwitch("Age", "User age")]
            public int Age
            {
                get { return this.age; }
                set { this.age = value; }
            }

            /// <summary>
            /// Simple example of a read-only switch, 
            /// </summary>
            [CommandLineSwitch("Test", "Test switch")]
            public string Test
            {
                get { return this.test; }
                set { this.test = value; }
            }

            [CommandLineSwitch("Day", "Day of the week selection")]
            [CommandLineAlias("DoW")]
            public DaysOfWeek DoW
            {
                get { return this.doW; }
                set { this.doW = value; }
            }

            public CommandLineParser Run(string cmdLine)
            {
                if (cmdLine == null)
                {
                    cmdLine = Environment.CommandLine;
                }

                // Parse the command line.
                CommandLineParser parser = this.CreateParser(cmdLine);
                parser.Parse();

                // Output what we have
                // Console.WriteLine(this.WriteParserInfo(parser));
                return parser;
            }

            private CommandLineParser CreateParser(string commandLine)
            {
                // Initialise the command line parser, passing in a reference to this
                // class so that we can look for any attributes that implement
                // command line switches.
                CommandLineParser parser = new CommandLineParser(commandLine, this);

                // Programmatically add some switches to the command line parser.
                parser.AddSwitch("Wobble", "Do something silly");

                // Add a switches with lots of aliases for the first name, "help" and "a".
                parser.AddSwitch(new string[] { "help", @"\?" }, "show help");
                parser.AddSwitch(new string[] { "a", "b", "c", "d", "e", "f" }, "Early alphabet");
                return parser;
            }

            private string WriteParserInfo(CommandLineParser parser)
            {
                StringBuilder builder = new StringBuilder();

                // ----------------------- DEBUG OUTPUT -------------------------------
                builder.AppendFormat("Program Name      : {0}\r\n", parser.ApplicationName);
                builder.AppendFormat("Non-switch Params : {0}\r\n", parser.Parameters.Length);
                for (int j = 0; j < parser.Parameters.Length; j++)
                {
                    builder.AppendFormat("                {0} : {1}\r\n", j, parser.Parameters[j]);
                }

                builder.AppendLine("----");
                builder.AppendFormat("Value of ShowSomeHelp    : {0}\r\n", this.ShowSomeHelp);
                builder.AppendFormat("Value of m_SomethingElse : {0}\r\n", this.wibble);
                builder.AppendFormat("Value of UserName        : {0}\r\n", this.UserName);
                builder.AppendLine("----");

                // Walk through all of the registered switches getting the available
                // information back out.
                CommandLineParser.SwitchInfo[] si = parser.Switches;
                if (si != null)
                {
                    builder.AppendFormat("There are {0} registered switches:\r\n", si.Length);
                    foreach (CommandLineParser.SwitchInfo s in si)
                    {
                        builder.AppendFormat("Command : {0} - [{1}]\r\n", s.Name, s.Description);
                        builder.AppendFormat("Type    : {0}\r\n", s.Type);

                        if (s.IsEnum)
                        {
                            builder.Append("- Enums allowed (");
                            foreach (string e in s.Enumerations)
                            {
                                builder.AppendFormat("{0} ", e);
                            }

                            builder.Append(")");
                        }

                        builder.AppendLine();

                        if (s.Aliases != null)
                        {
                            Console.Write("Aliases : [{0}] - ", s.Aliases.Length);
                            foreach (string alias in s.Aliases)
                            {
                                Console.Write(" {0}", alias);
                            }

                            builder.AppendLine();
                        }

                        builder.AppendFormat(
                                "------> Value is : {0} (Without any callbacks {1})\n",
                                s.Value ?? "(Unknown)",
                                s.InternalValue ?? "(Unknown)");
                    }
                }
                else
                {
                    builder.AppendFormat("There are no registered switches.");
                }

                // Test looking for a specificly named values.
                builder.AppendFormat("----");
                if (parser["help"] != null)
                {
                    builder.AppendFormat("Request for help = {0}\r\n", parser["help"]);
                }
                else
                {
                    builder.AppendFormat("Request for help has no associated value.\r\n");
                }

                builder.AppendFormat("User Name is {0}\r\n", parser["name"]);

                // Note the difference between the parser and a callback value.
                builder.AppendFormat(
                        "The property of test (/test) is internally is read-only, "
                        + "e.g. no update can be made by the parser:\n" + "   -- The indexer gives a value of : {0}\n"
                        + "   -- Internally the parser has    : {1}\r\n",
                        parser["test"],
                        parser.InternalValue("test"));

                // Test if the enumeration value has changed to Friday!
                if (this.DoW == DaysOfWeek.Fri)
                {
                    builder.AppendLine("\nYeah Friday.... PUB LUNCH TODAY...");
                }

                // For error handling, were any switches handled?
                string[] unhandled = parser.UnhandledSwitches;
                if (unhandled != null)
                {
                    builder.AppendLine("\nThe following switches were not handled.");
                    foreach (string s in unhandled)
                    {
                        builder.AppendFormat("  - {0}\r\n", s);
                    }
                }

                return builder.ToString();
            }

            public static int Main(string[] cmdLine)
            {
                Application app = new Application();
                app.Run(null);
                return 0;
            }
        }

        private class BadSwitch
        {
            [CommandLineSwitch("?", "Not allowed")]
            public string Fred
            {
                get { return null; }
            }
        }

        private class BadAlias
        {
            [CommandLineSwitch("Fred", "Configure fred")]
            [CommandLineAlias("?")]
            public string Fred
            {
                get { return null; }
            }
        }

        private class StringEmptySwitch
        {
            [CommandLineSwitch("", "Configure fred")]
            public string Fred
            {
                get { return null; }
            }
        }

        private class StringEmptyAlias
        {
            [CommandLineSwitch("Fred", "Configure fred")]
            [CommandLineAlias("")]
            public string Fred
            {
                get { return null; }
            }
        }
    }
}