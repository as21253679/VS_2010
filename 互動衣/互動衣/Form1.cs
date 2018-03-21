using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace 互動衣
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private SerialPort port= new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            port.Open();
            port.Write("1");
            port.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            port.Open();
            port.Write("2");
            port.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            port.Open();
            port.Write("3");
            port.Close();
        }
    }
}
