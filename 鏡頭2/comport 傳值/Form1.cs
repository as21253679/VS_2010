using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;


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
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            byte[] pkt = new byte[122];
            char[] c = new char[1];
            int total = 0;
            int timeout = 0;
            port = new SerialPort("COM19", 115200, Parity.None, 8, StopBits.One);
            port.Open();
            port.Write("A");
            FileStream file = new FileStream("C:\\1/123.jpg", FileMode.Create);
            while (true)
            {
                while (port.BytesToRead > 0) 
                {
                        port.Read(c, 0, 1);
                        port.Read(c, 0, 1);
                        total += (int)(c[0] - '0') * 100;
                        port.Read(c, 0, 1);
                        total += (int)(c[0] - '0') * 10;
                        port.Read(c, 0, 1);
                        total += (int)(c[0] - '0');
                        if (total < 80)
                            total = 100;
                        label1.Text = total.ToString();
                        goto d;
                }
            }
        d:
            for (int i = 0; i < total; i++)
            {
                while (port.BytesToRead < 122)
                {
                    timeout++;
                    if (timeout > 5000000)
                        goto a;
                }
                port.Read(pkt, 0, 122);
                for (int j = 0; j < 122; j++)
                {
                    file.WriteByte(pkt[j]);
                }
            }
        a:
            file.Close();
            port.Close();
            //顯示圖
            FileStream fs = File.OpenRead("C:\\1/123.jpg");
            int filelength = 0;
            filelength = (int)fs.Length; //獲得檔長度
            Byte[] image = new Byte[filelength]; //建立一個位元組陣列
            fs.Read(image, 0, filelength); //按位元組流讀取
            pictureBox1.Image = System.Drawing.Image.FromStream(fs);
            fs.Close();
        }
    }
}