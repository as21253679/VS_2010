//教學 http://www.blueshop.com.tw/board/FUM20050124192253INM/BRD20060926171557EJE.html 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32; 

namespace CDC註冊檔
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text=System.Convert.ToString(Registry.GetValue("HKEY_LOCAL_MACHINE\\HARDWARE\\DEVICEMAP\\SERIALCOMM", "\\Device\\BthModem0", ""));
        }
    }
}
