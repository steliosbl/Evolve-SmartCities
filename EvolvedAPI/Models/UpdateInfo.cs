namespace EvolvedAPI.Models
{
    using System;
    using System.Linq;

    public class UpdateInfo
    {
        public int ID { get; set; }
        public string State { get; set; }
        public DateTime Timestamp { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
