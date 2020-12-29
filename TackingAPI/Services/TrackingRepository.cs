using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TackingAPI.Models;
using TackingAPI.Shared;
using System.IO.Ports;

namespace TackingAPI.Services
{
    public class TrackingRepository
    {
        public string Latitude;
        public string Longitude;
        private const string CacheKey = "ContactStore";
        public List<Tracker> GetLocations(DateTime starttime, DateTime EndTime)
        {
            List<Tracker> VehicleList = new List<Tracker>();
            var ctx = HttpContext.Current;
            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {

                    Tracker tracker = null;
                    DataSet data = new DataSet();
                    BLL logic = new BLL();
                    data = logic.FetchLocations(starttime, EndTime);
                    foreach (DataRow dr in data.Tables[0].Rows)
                    {
                        tracker = new Tracker();
                        tracker.VehicleName = dr[0].ToString();
                        tracker.RegisterationNumber = dr[1].ToString();
                        tracker.Latitude = dr[2].ToString();
                        tracker.Longitude = dr[3].ToString();
                        VehicleList.Add(tracker);
                    }
                  
                }
            }
            return VehicleList;
        }
        public Tracker GetCurrentLocation()
        {
            Tracker tracker = new Tracker();

            var ctx = HttpContext.Current;
            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    DataSet data = new DataSet();
                    BLL logic = new BLL();
                    data = logic.FetchCurrentLocation(DateTime.Now);
                    if (data.Tables[0].Rows.Count > 0)
                    {
                        tracker.VehicleName = data.Tables[0].Columns[0].ToString();
                        tracker.RegisterationNumber = data.Tables[1].Columns[0].ToString();
                        tracker.Latitude = data.Tables[0].Columns[2].ToString();
                        tracker.Longitude = data.Tables[0].Columns[3].ToString();
                    }
                }
            }
            return tracker;
        }

        public void InsertLocation()
        {
            var ctx = HttpContext.Current;
            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    BLL logic = new BLL();
                    SerialPort serialPort1 = new SerialPort();
                    DataSet devices = new DataSet();
                    devices = logic.FetchConnectedDevices();
                    foreach (DataRow dr in devices.Tables[0].Rows)
                    {

                        serialPort1.Close();
                        serialPort1.PortName = dr[1].ToString();
                        serialPort1.Open();
                        if (serialPort1.IsOpen)
                        {
                            string data = serialPort1.ReadExisting();
                            string[] strArr = data.Split('$');
                            for (int i = 0; i < strArr.Length; i++)
                            {
                                string strTemp = strArr[i];
                                string[] lineArr = strTemp.Split(',');
                                if (lineArr[0] == "GPGGA")
                                {

                                    try
                                    {
                                        //Latitude
                                        Double dLat = Convert.ToDouble(lineArr[2]);
                                        dLat = dLat / 100;
                                        string[] lat = dLat.ToString().Split('.');
                                        Latitude = lineArr[3].ToString() + lat[0].ToString() + "." + ((Convert.ToDouble(lat[1]) / 60)).ToString("#####");

                                        //Longitude
                                        Double dLon = Convert.ToDouble(lineArr[4]);
                                        dLon = dLon / 100;
                                        string[] lon = dLon.ToString().Split('.');
                                        Longitude = lineArr[5].ToString() + lon[0].ToString() + "." + ((Convert.ToDouble(lon[1]) / 60)).ToString("#####");
                                        logic.AddLocation(Convert.ToInt32(dr[0].ToString()), Convert.ToDecimal(Latitude), Convert.ToDecimal(Longitude));


                                    }
                                    catch
                                    {

                                    }
                                }
                            }

                         }
                    }
                }
            }
        }
    }
}