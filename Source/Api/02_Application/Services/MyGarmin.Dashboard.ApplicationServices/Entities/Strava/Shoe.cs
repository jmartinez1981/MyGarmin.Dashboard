namespace MyGarmin.Dashboard.ApplicationServices.Entities.Strava
{
    public class Shoe
    {
        public string ShoeId { get; private set; }

        public string Brand { get; private set; }

        public string Model { get; private set; }

        public float Distance { get; private set; }

        public Shoe(string id, string brand, string model, float distance)
        {
            this.ShoeId = id;
            this.Brand = brand;
            this.Model = model;
            this.Distance = distance;
        }
    }
}
