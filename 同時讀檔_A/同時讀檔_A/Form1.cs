using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace 同時讀檔_A
{
    public partial class Form1 : Form
    {
        StreamWriter sw = null;
        string savefullpath = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string savefilename = System.DateTime.Now.Year + "-"
                       + System.DateTime.Now.Month.ToString("00") + "-" +
                       System.DateTime.Now.Day.ToString("00") + "-" +
                       System.DateTime.Now.Hour.ToString("00") +
                       ".log";
            savefullpath = @"d:\sample5\"+savefilename;
            sw = new StreamWriter(savefullpath, true);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sw.WriteLine(System.DateTime.Now + " \r\n".Trim());
            sw.Flush();  //將buffer的值寫入檔案
            
        }
    }
}
