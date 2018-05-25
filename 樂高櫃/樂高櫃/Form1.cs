using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Net;
using System.IO;

namespace 樂高櫃
{
    public partial class Form1 : Form
    {
        private SerialPort port;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] bty={0xAA,0x04,0x94,0x62,0x11,0x31};
            port.Write(bty, 0, bty.Length);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            port = new SerialPort("COM19", 115200, Parity.None, 8, StopBits.One);
            port.Open();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            port = new SerialPort("COM19", 115200, Parity.None, 8, StopBits.One);
            //port.Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] UTF8bytes = {(9*16+4)};
            port.Write(UTF8bytes, 0, UTF8bytes.Length);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string targetUrl = "http://140.130.35.236/skin/a.html";

            HttpWebRequest request = HttpWebRequest.Create(targetUrl) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 30000;

            string result = "";
            // 取得回應資料
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
            }
            textBox1.Text = result;
        }
    }
}
