namespace EnergyTrading.Deployment.TeamCity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EnergyTrading.Deployment.SourceControl;

    using TeamCitySharp;

    public class TeamCityRestApiClient
    {
        private readonly TeamCityHostConnectionDetails connectionDetails;
        private readonly TeamCityClient client;

        private bool started;

        public TeamCityRestApiClient(TeamCityHostConnectionDetails connectionDetails)
        {
            if (connectionDetails == null) { throw new ArgumentNullException("connectionDetails"); }
            this.connectionDetails = connectionDetails;

            this.client = new TeamCityClient(connectionDetails.Host);
        }

        public void Start()
        {
            if (this.started)
            {
                return;
            }

            this.client.Connect(this.connectionDetails.Username, this.connectionDetails.Password);

            this.started = true;
        }

        public List<SvnChangeLog> AllChangeLogsForBuildBetweenTags(string buildConfigId, string fromTag, string untilTag, string[] unwantedUsernames, int maxCount = 1000)
        {
            this.Start();

            try
            {
                var fromBuild = this.client.Builds.ByConfigIdAndTag(buildConfigId, fromTag);
                var untilBuild = this.client.Builds.ByConfigIdAndTag(buildConfigId, untilTag);
                return this.AllChangeLogsForBuildBetweenDates(buildConfigId,
                    fromBuild.OrderByDescending(b => b.StartDate).First().StartDate,
                    untilBuild.OrderByDescending(b => b.FinishDate).First().StartDate,
                    maxCount,
                    unwantedUsernames);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Unable to determine start and end builds with build type id [{0}] tagged with [{1},{2}]", buildConfigId, fromTag, untilTag), e);
            }
        }

        public string DetermineLatestBuildNumberForBuildWithTag(string buildConfigId, string tag)
        {
            this.Start();
            try
            {
                var builds = this.client.Builds.ByConfigIdAndTag(buildConfigId, tag);
                return builds.OrderByDescending(b => b.FinishDate).First().Number;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Unable to find a build with build type id [{0}] tagged with [{1}]", buildConfigId, tag), e);
            }
        }

        private List<SvnChangeLog> AllChangeLogsForBuildBetweenDates(string buildConfigId, DateTime from, DateTime until, int maxCount, string[] unwantedUsernames = null)
        {
            var changes = this.client.Changes.ByBuildConfigId(buildConfigId).Take(maxCount);
            var result = new List<SvnChangeLog>();
            foreach (var change in changes)
            {
                var details = this.client.Changes.ByChangeId(change.Id);
                if ((details.Date > from && details.Date <= until) && (unwantedUsernames == null || !unwantedUsernames.Contains(details.Username)))
                {
                    var changeLog = new SvnChangeLog
                    {
                        CommitDateTime = details.Date,
                        Comment = details.Comment == null ? string.Empty : details.Comment.Replace("\n", string.Empty),
                        SvnRevision = details.Version,
                        Username = details.Username
                    };
                    result.Add(changeLog);
                }
            }

            return result.OrderByDescending(change => change.CommitDateTime).ToList();
        }
    }
}
