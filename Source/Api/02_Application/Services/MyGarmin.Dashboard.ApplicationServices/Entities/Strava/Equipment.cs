using System;
using System.Collections.Generic;

namespace MyGarmin.Dashboard.ApplicationServices.Entities.Strava
{
    public class Equipment
    {
        public List<Shoe> Shoes { get; private set; }

        public Equipment()
        {
            this.Shoes = new List<Shoe>();
        }
    }
}
