using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FileMonitorService
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer;
        private string directoryPath;
        private string fileName;

        public Service1()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = 120000; // 2 minute
            timer.Elapsed += new ElapsedEventHandler(CheckForFile);

            // Set up the directory and file to monitor
            directoryPath = @"C:\temp\weighing\";
            fileName = "w11.txt";
        }

        protected override void OnStart(string[] args)
        {
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
        }

        private void CheckForFile(object sender, ElapsedEventArgs e)
        {
            // Check if the file exists in the directory
            if (!File.Exists(Path.Combine(directoryPath, fileName)))
            {
                // If the file doesn't exist, log an event to the application event log
                string message = "The file " + fileName + " was not found in the directory " + directoryPath + ".";
                EventLog.WriteEntry("FileMonitorService", message, EventLogEntryType.Warning);
            }
        }
    }
}
