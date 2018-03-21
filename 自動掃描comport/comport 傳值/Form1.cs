using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace comport_傳值
{
    public partial class Form1 : Form
    {
        private SerialPort port;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 1; i < 60; i++)
            {
                port = new SerialPort("COM" + i, 9600, Parity.None, 8, StopBits.One);
                try
                {
                    port.Open();
                    break;
                }
                catch(System.IO.IOException)
                {
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            port.WriteLine("AT+CIPMUX=1");
            port.WriteLine("AT+CIPSERVER=1,8087");
        }
    }
}
