using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TackingAPI.Models;
using TackingAPI.Shared;
using TackingAPI.Services;
using System.Web.Http.Description;

namespace TackingAPI.Controllers
{
    [RoutePrefix("api/Tracker")]
    public class TrackerController : ApiController
    {
        private TrackingRepository trackingRepository;
        public TrackerController()
        {
            this.trackingRepository = new TrackingRepository();
        }
        [Route("GetCurrentLocation")]
        public Tracker GetCurrentLocation()
        {
            return trackingRepository.GetCurrentLocation();
        }
        [Route("GetLocations")]
        public List<Tracker> GetLocations(DateTime startDate,DateTime endDate)
        {
            return trackingRepository.GetLocations(startDate,endDate);
        }
        [Route("InsertLocation")]
        public void InsertLocation()
        {
            trackingRepository.InsertLocation();
        }


    }
}
