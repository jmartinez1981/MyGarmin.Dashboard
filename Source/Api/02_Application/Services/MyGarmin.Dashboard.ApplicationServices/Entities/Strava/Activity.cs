using System;

namespace MyGarmin.Dashboard.ApplicationServices.Entities.Strava
{
    public class Activity
    {
        public long Id { get; private set; }

        public string ClientId { get; private set; }

        public string ActivityType { get; private set; }

        public string Name { get; private set; }

        public float Distance { get; private set; }

        public float ElevationGain { get; private set; }

        public float ElevationHigh { get; private set; }

        public float ElevationLow { get; private set; }

        public string GearId { get; private set; }

        public DateTime StartDate { get; private set; }

        public int MovingTime { get; private set; }

        public int ElapsedTime { get; private set; }

        public Activity(
            long id,
            string clientId,
            string name,
            string activityType)
        {
            this.Id = id;
            this.ClientId = clientId;
            this.Name = name;
            this.ActivityType = activityType;
        }

        public void SetGearData(string gearId, float distance)
        {
            this.GearId = gearId;
            this.Distance = distance;
        }

        public void SetElevationData(float elevationGain, float elevationHigh, float elevationLow)
        {
            this.ElevationGain = elevationGain;
            this.ElevationHigh = elevationHigh;
            this.ElevationLow = elevationLow;
        }

        public void SetActivityTimeData(DateTime startDate, int movingTime, int elapsedTime)
        {
            this.StartDate = startDate;
            this.MovingTime = movingTime;
            this.ElapsedTime = elapsedTime;
        }
    }
}
