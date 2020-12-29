using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TackingAPI.Models
{
    public class Tracker
    {
        public string VehicleName { get; set; }
        public string RegisterationNumber { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class TrackerList
    {
        public TrackerList()
        {
            tracker = new Tracker();
        }

        Tracker tracker { get; set; }
    }

}