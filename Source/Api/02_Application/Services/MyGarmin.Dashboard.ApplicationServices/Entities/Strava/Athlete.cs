using System;

namespace MyGarmin.Dashboard.ApplicationServices.Entities.Strava
{
    public class Athlete
    {
        public long Id { get; private set; }

        public string ClientId { get; private set; }

        public string Name { get; private set; }

        public string LastName { get; private set; }

        public Uri ProfileImage { get; private set; }

        public string MeasurementPreference { get; private set; }

        public Equipment Equipment { get; private set; }

        public Athlete(
            long id,
            string clientId,
            string name,
            string lastName,
            Uri profileImage,
            string measurementPreference)
        {
            this.Id = id;
            this.ClientId = clientId;
            this.Name = name;
            this.LastName = lastName;
            this.ProfileImage = profileImage;
            this.MeasurementPreference = measurementPreference;
            this.Equipment = new Equipment();
        }

        public void AddShoeToEquipment(string id, string brand, string model, float distance)
        {
            this.Equipment.Shoes.Add(new Shoe(id, brand, model, distance));
        }
    }
}
