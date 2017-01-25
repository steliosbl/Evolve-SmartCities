namespace EvolvedAPI.Models
{
    public class Module
    {
        public Module(int id, int hubid, double longitude, double latitude, int redduration, int orangeduration, int greenduration, string currentstate)
        {
            this.ID = id;
            this.HubID = hubid;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.RedDuration = redduration;
            this.OrangeDuration = orangeduration;
            this.GreenDuration = greenduration;
            this.CurrentState = currentstate;
        }

        public int ID { get; private set; }

        public int HubID { get; private set; }

        public double Longitude { get; private set; }

        public double Latitude { get; private set; }

        public int RedDuration { get; private set; }

        public int OrangeDuration { get; private set; }

        public int GreenDuration { get; private set; }

        public string CurrentState { get; private set; }
    }
}
