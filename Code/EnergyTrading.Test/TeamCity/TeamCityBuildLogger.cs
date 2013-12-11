namespace EnergyTrading.Test.TeamCity
{
    public class TeamCityBuildLogger : ITeamCityBuildLogger
    {
        public void LogBuildFailed(string message, bool includeStandardTeamCityStatusText = false)
        {
            var text = includeStandardTeamCityStatusText ? string.Format("{{build.status.text}} {0}", StripInvalidMarkupCharacters(message)) : StripInvalidMarkupCharacters(message);
            System.Console.Out.WriteLine("##teamcity[buildStatus status='FAILURE' text='{0}']", text);
        }

        public void LogBuildSucceeded(string message = null)
        {
            System.Console.Out.WriteLine("##teamcity[buildStatus status='SUCCESS'{0}]", message == null ? string.Empty : string.Format(" text='{0}'", StripInvalidMarkupCharacters(message)));
        }

        public void LogSuiteStarted(string suiteName)
        {
            System.Console.Out.WriteLine("##teamcity[testSuiteStarted name='{0}']", suiteName);
        }

        public void LogSuiteFinished(string suiteName)
        {
            System.Console.Out.WriteLine("##teamcity[testSuiteFinished name='{0}']", suiteName);
        }

        public void LogTestStarted(string testName)
        {
            System.Console.WriteLine("##teamcity[testStarted name='{0}']", testName);
        }

        public void LogTestOutput(string testName, string message, bool isError = false)
        {
            System.Console.WriteLine("##teamcity[{0} name='{1}' out='{2}']", isError ? "testStdErr" : "testStdOut", testName, StripInvalidMarkupCharacters(message));
        }

        public void LogTestFailed(string testName, string message)
        {
            System.Console.WriteLine("##teamcity[testFailed name='{0}' message='{1}']", testName, StripInvalidMarkupCharacters(message));
        }

        public void LogTestFinished(string testName, int duration = 0)
        {
            System.Console.WriteLine("##teamcity[testFinished name='{0}' {1}]", testName, duration > 0 ? string.Format("duration='{0}'", duration) : string.Empty);
        }

        public void LogTestRun(string testName, string failureMessage = null, string[] testLogMessages = null, string[] testErrorMessages = null, int duration = 0)
        {
            LogTestStarted(testName);
            if (testLogMessages != null)
            {
                foreach (var message in testLogMessages)
                {
                    LogTestOutput(testName, message);
                }
            }
            if (testErrorMessages != null)
            {
                foreach (var message in testErrorMessages)
                {
                    LogTestOutput(testName, message, true);
                }
            }
            if (failureMessage != null)
            {
                LogTestFailed(testName, failureMessage);
            }
            LogTestFinished(testName, duration);
        }

        public void LogBuildProgressMessage(string message)
        {
            System.Console.WriteLine("##teamcity[progressMessage '{0}']", StripInvalidMarkupCharacters(message));
        }

        public void LogBuildProgressStartMessage(string message)
        {
            System.Console.WriteLine("##teamcity[progressStart '{0}']", StripInvalidMarkupCharacters(message));
        }

        public void LogBuildProgressFinishMessage(string message)
        {
            System.Console.WriteLine("##teamcity[progressFinish '{0}']", StripInvalidMarkupCharacters(message));
        }

        private string StripInvalidMarkupCharacters(string message)
        {
            var result = message.Replace("[", "|[");
            result = result.Replace("]", "|]");
            result = result.Replace("'", "|'");

            return result;
        }
    }
}