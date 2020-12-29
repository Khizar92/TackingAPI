using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace TrackerUpdateService
{
    public partial class TrackerServiceCaller : ServiceBase
    {
        Timer timer = new Timer();
        DateTime scheduleDateTime;
        public TrackerServiceCaller()
        {
            InitializeComponent();
        }
        public void onDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {

            WriteLogFile("Service is started");
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            scheduleDateTime = DateTime.Today.AddHours(16).AddMinutes(10);
            var scheduleInterval = scheduleDateTime.Subtract(DateTime.Now).TotalSeconds * 1000;
            if (scheduleInterval < 0)
            {
                scheduleInterval += new TimeSpan(24, 0, 0).TotalSeconds * 1000;
            }
            timer.Interval = scheduleInterval;
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteLogFile("Service is stopped");
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            if (timer.Interval != 30 * 1000)
            {
                timer.Interval = 30* 1000;  
            }
            string ApiData = new WebClient().DownloadString("http://localhost:54206/Api/Tracker/InsertLocation");
        }
        public void WriteLogFile(string message)
        {
            StreamWriter sw = null;
            sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
            sw.WriteLine(DateTime.Now + "-" + message);
            sw.Flush();
            sw.Close();
        }
    }
}
