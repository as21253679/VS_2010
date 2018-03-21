using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Management;

namespace CDC
{
    public partial class Form1 : Form
    {
        string ports ="";
        private SerialPort port;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetSerialPort();
            port = new SerialPort(Convert.ToString(ports), 115200, Parity.None, 8, StopBits.One);
            port.Open();
        }
        private void GetSerialPort()
        {
            try
            {
                ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_PnPEntity");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string s = string.Format("{0}", queryObj["Name"]);
                    Console.WriteLine(s);
                    string s1 = "";
                    if (s.Length >= 23)
                    {
                        s1 = s.Substring(0, s.Length - 8);
                        ports = s.Substring(18,5);
                    }
                    //if (s == "Prolific USB-to-Serial Comm Port")
                    if (s1 == "USB-SERIAL CH340")
                    {
                        Console.WriteLine(ports);
                        break;
                    }
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
    }
}