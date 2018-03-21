using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace 同時讀檔_B
{
    public partial class Form1 : Form
    {
        string savefullpath = "";
        FileStream file_WR;
        StreamReader file2;
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
            savefullpath = @"d:\sample5\" + savefilename;
            //此段為核心，FileShare.ReadWrite 同檔案可被同時讀寫
            file_WR = new FileStream(savefullpath, FileMode.Open,
            FileAccess.Read, FileShare.ReadWrite);
            file2 = new System.IO.StreamReader(file_WR);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string line = "";

            while ((line = file2.ReadLine()) != null)
            {
                textBox1.Text = line + "\r\n";
            }
            //file_WR.Close();
        }
    }
}
