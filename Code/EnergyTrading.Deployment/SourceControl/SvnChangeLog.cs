namespace EnergyTrading.Deployment.SourceControl
{
    using System;

    public class SvnChangeLog
    {
        public string SvnRevision { get; set; }

        public DateTime CommitDateTime { get; set; }

        public string Comment { get; set; }

        public string JiraId { get; set; }

        public string Username { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1} [{2}]", this.CommitDateTime, this.Comment, this.Username);
        }

        public string AsCsv()
        {
            return string.Format("{0},{1},\"{2}\",{3},{4}", this.CommitDateTime, this.Username, this.Comment, this.SvnRevision, string.IsNullOrEmpty(this.JiraId) ? "n/a" : this.JiraId);
        }
    }
}