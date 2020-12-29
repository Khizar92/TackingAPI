using System;
using System.Configuration;

namespace TackingAPI.Shared
{
    public class Connection
    {
        private static string con;

        static Connection()
        {
            try
            {
                con = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ToString();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
        }

        public string getconnection()
        {
            return con;
        }
    }
}