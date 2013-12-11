namespace EnergyTrading.Test.TeamCity
{
    public interface ITeamCityBuildLogger
    {
        void LogBuildFailed(string message, bool includeStandardTeamCityStatusText = false);
        void LogBuildSucceeded(string message = null);
        void LogTestRun(string testName, string failureMessage = null, string[] testLogMessages = null, string[] testErrorMessages = null, int duration = 0);
        void LogBuildProgressMessage(string message);
        void LogBuildProgressStartMessage(string message);
        void LogBuildProgressFinishMessage(string message);
        void LogSuiteStarted(string suiteName);
        void LogSuiteFinished(string suiteName);
        void LogTestStarted(string testName);
        void LogTestOutput(string testName, string message, bool isError = false);
        void LogTestFailed(string testName, string message);
        void LogTestFinished(string testName, int duration = 0);
    }
}