using System.Collections.Generic;
using System.Linq;
using iRSDKSharp;
namespace CrewChiefV4.iRacing
{
    public class SessionData
    {
        const string sessionInfoYamlPath = "SessionInfo:Sessions:SessionNum:{{{0}}}{1}:";

        public SessionData()
        {
            this.SessionId = -1;
            this.IsOval = false;
            this.IsRallyCross = false;
            this.HeatRacing = false;
        }

        public Track Track { get; set; }
        public string EventType { get; set; }
        public string SessionType { get; set; }
        public int SessionId { get; set; }
        public int SubsessionId { get; set; }
        public string SessionTimeString { get; set; }
        public string RaceLaps { get; set; }
        public double RaceTime { get; set; }
        public string IncidentLimitString { get; set; }
        public int IncidentLimit { get; set; }
        public bool IsTeamRacing { get; set; }
        public int NumCarClasses { get; set; }
        public int NumJokerLaps { get; set; }
        public bool IsOval { get; set; }
        public bool IsRallyCross { get; set; }
        public bool HeatRacing { get; set; }
                        
        public void Update(string sessionString, int sessionNumber)
        {
            this.Track = Track.FromSessionInfo(sessionString);            
            this.SubsessionId = Parser.ParseInt(YamlParser.Parse(sessionString, "WeekendInfo:SubSessionID:"));
            this.SessionId = Parser.ParseInt(YamlParser.Parse(sessionString, "WeekendInfo:SessionID:"));            
            this.IsTeamRacing = Parser.ParseInt(YamlParser.Parse(sessionString, "WeekendInfo:TeamRacing:")) == 1;
            this.NumCarClasses = Parser.ParseInt(YamlParser.Parse(sessionString, "WeekendInfo:NumCarClasses:"));
            this.EventType = YamlParser.Parse(sessionString, "WeekendInfo:EventType:");
            this.SessionType = YamlParser.Parse(sessionString, string.Format(sessionInfoYamlPath, sessionNumber, "SessionType"));
            this.RaceLaps = YamlParser.Parse(sessionString, string.Format(sessionInfoYamlPath, sessionNumber, "SessionLaps"));
            this.SessionTimeString = YamlParser.Parse(sessionString, string.Format(sessionInfoYamlPath, sessionNumber, "SessionTime"));
            this.RaceTime = Parser.ParseSec(SessionTimeString);            
            this.IncidentLimitString = YamlParser.Parse(sessionString, "WeekendInfo:WeekendOptions:IncidentLimit:");
            this.NumJokerLaps = Parser.ParseInt(YamlParser.Parse(sessionString, "WeekendInfo:WeekendOptions:IncidentLimit:"));
            this.HeatRacing = Parser.ParseInt(YamlParser.Parse(sessionString, "WeekendInfo:HeatRacing:")) == 1;
            this.IsOval = this.Track.Category.ToLower().Contains("oval");
            this.IsRallyCross = this.Track.Category.ToLower().Contains("dirtroad");

            if(IsLimitedIncidents)
            {
                IncidentLimit = Parser.ParseInt(IncidentLimitString);
            }
            else
            {
                IncidentLimit = -1;
            }
        }
        public bool IsLimitedSessionLaps
        {
            get
            {
                return RaceLaps.ToLower() != "unlimited";
            }
        }

        public bool IsLimitedTime
        {
            get
            {
                return SessionTimeString.ToLower() != "unlimited";
            }
        }
        public bool IsLimitedIncidents
        {
            get
            {
                return IncidentLimitString.ToLower() != "unlimited";
            }
        }
    }
}
