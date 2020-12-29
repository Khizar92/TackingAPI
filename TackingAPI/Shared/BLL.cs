using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TackingAPI.Shared
{


    public class BLL
    {
        SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ToString());

        public System.Data.DataSet FetchCurrentLocation(DateTime CurrentDate)
        {
            DBAccess data = new DBAccess();
            data.AddParameter("@CurrentTime", CurrentDate);
            return (System.Data.DataSet)data.ExecuteDataSet("FetchVehicleLocationByCurrentTime");
        }

        public System.Data.DataSet FetchLocations(DateTime StartDate, DateTime EndDate)
        {
            DBAccess data = new DBAccess();
            data.AddParameter("@StartTime", StartDate);
            data.AddParameter("@EndTime", EndDate);
            return (System.Data.DataSet)data.ExecuteDataSet("FetchVehicleLocationAtCertTime");

        }

        public System.Data.DataSet FetchConnectedDevices()
        {
            DBAccess data = new DBAccess();
            return (System.Data.DataSet)data.ExecuteDataSet("FetchDevices");

        }



        public void AddLocation(int DeviceId, decimal Longitude, decimal Latitude)
        {
            SqlCommand cmd = new SqlCommand("AddLocation", Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DeviceId", SqlDbType.Int).Value = DeviceId;
            cmd.Parameters.Add("@Longitude", SqlDbType.Decimal).Value = Longitude;
            cmd.Parameters.Add("@Latitude", SqlDbType.Decimal).Value = Latitude;
            Connection.Open();
            cmd.ExecuteScalar();
            Connection.Close();
           
        }

    }
}