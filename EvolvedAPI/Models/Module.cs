namespace EvolvedAPI.Models
{
    using System;

    public class Module
    {
        [SDatabase.Attributes.SDBConstructor]
        public Module(int id, int hubid, double longitude, double latitude, string currentState, DateTime lastUpdated, TimeSpan duration)
        {
            this.ID = id;
            this.HubID = hubid;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.CurrentState = currentState;
            this.LastUpdated = lastUpdated;
            this.Duration = duration;
        }

        public int ID { get; private set; }

        public int HubID { get; private set; }

        public double Longitude { get; private set; }

        public double Latitude { get; private set; }

        public string CurrentState { get; private set; }

        public DateTime LastUpdated { get; private set; }

        public TimeSpan Duration { get; private set; }
    }
}
