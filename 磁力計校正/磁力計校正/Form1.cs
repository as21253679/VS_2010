using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;

namespace 磁力計校正
{
    public partial class Form1 : Form
    {
        int x = 0;
        int y = 0;
        int mx = 0, my = 0, mz = 0;
        SerialPort comport = new SerialPort("COM10", 115200, Parity.None, 8, StopBits.One);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comport.Open();
            rx.Enabled = true;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int picBoxWidth = pictureBox1.Size.Width;
            int picBoxHeight = pictureBox1.Size.Height;
            int halfWidth = pictureBox1.Size.Width / 2;
            int halfHeight = pictureBox1.Size.Height / 2;
            Graphics objGraphic = e.Graphics;
            Pen pen = new Pen(Color.Black);

            objGraphic.DrawLine(pen, halfWidth, 0, halfWidth, picBoxHeight);
            objGraphic.DrawLine(pen, 0, halfHeight, picBoxWidth, halfHeight);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SolidBrush redBrush_xy = new SolidBrush(Color.Red);
            SolidBrush redBrush_xz = new SolidBrush(Color.Blue);
            SolidBrush redBrush_yz = new SolidBrush(Color.Green);
            Rectangle rect_xy = new Rectangle(mx, my, 5, 5);
            Rectangle rect_xz = new Rectangle(mx, mz, 5, 5);
            Rectangle rect_yz = new Rectangle(my, mz, 5, 5);
            Graphics m_graphics = pictureBox1.CreateGraphics();
            if (checkBox1.Checked)
                m_graphics.FillPie(redBrush_xy, rect_xy, 0, 360);
            if (checkBox2.Checked)
                m_graphics.FillPie(redBrush_xz, rect_xz, 0, 360);
            if (checkBox3.Checked)
                m_graphics.FillPie(redBrush_yz, rect_yz, 0, 360);
        }

        private void rx_Tick(object sender, EventArgs e)
        {
            int ss, c, yaw, count = 0;
            Double azi = 0;
            char[] str1 = new char[5];
            char[] str2 = new char[5];
            char[] str3 = new char[5];
            char[] str4 = new char[5];
            if (comport.BytesToRead > 0)
            {
                try
                {
                    while (true)
                    {
                        ss = comport.ReadChar();
                        if ((char)ss == 'x')
                            break;
                    }
                    while (true)
                    {
                        c = (comport.ReadChar());
                        if ((char)c != 'y')
                        {
                            str1[count] = (char)c;
                            count++;
                        }
                        else
                            break;
                    }
                    count = 0;
                    while (true)
                    {
                        c = (comport.ReadChar());
                        if ((char)c != 'z')
                        {
                            str2[count] = (char)c;
                            count++;
                        }
                        else
                            break;
                    }
                    count = 0;
                    while (true)
                    {
                        c = (comport.ReadChar());
                        if ((char)c != 's')
                        {
                            str3[count] = (char)c;
                            count++;
                        }
                        else
                            break;
                    }
                    count = 0;
                    while (true)
                    {
                        c = (comport.ReadChar());
                        if ((char)c != 'd')
                        {
                            str4[count] = (char)c;
                            count++;
                        }
                        else
                            break;
                    }
                }
                catch
                {
                    Debug.WriteLine("error:1");
                }

                if (str1[0] != '\0')
                {
                    try
                    {
                        mx = Convert.ToInt32(String.Concat(str1));
                        my = Convert.ToInt32(String.Concat(str2));
                        mz = Convert.ToInt32(String.Concat(str3));
                        yaw = Convert.ToInt32(String.Concat(str4));
                        label4.Text = String.Concat(str1);
                        label5.Text = String.Concat(str2);
                        label6.Text = String.Concat(str3);
                        label7.Text = ((int)(yaw)).ToString() + "度";
                    }
                    catch
                    {
                        Debug.WriteLine("error:2");
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // pictureBox1.Image = null;
        }
    }
}
