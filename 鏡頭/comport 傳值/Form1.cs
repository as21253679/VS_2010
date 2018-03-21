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
            byte[] command = { 0xAA, 0x0D, 0x00, 0x00, 0x00, 0x00 };
            byte[] rx = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            int total = 0;
            port = new SerialPort("COM22", 115200, Parity.None, 8, StopBits.One);
            port.Open();
            FileStream file = new FileStream("C:\\1/123.jpg", FileMode.Create);
            //喚醒模組
            /*for (int i = 0; i < 500; i++)
            {
                port.Write(command, 0, 6);
                if (port.BytesToRead>0)
                    break;
            }*/

            textBox1.Text += "--------------------" + "\r\n";
            //初始化
            command[1] = 0x01;
            command[2] = 0x00;
            command[3] = 0x07;
            command[4] = 0x01;
            command[5] = 0x07;
            port.Write(command, 0, 6);    //{'AA','01','00','07','01','07'}
            while (port.BytesToRead != 6) ;
            port.Read(rx, 0, 6);
            textBox1.Text += BitConverter.ToString(rx) + "\r\n";
            //設定128 bytes
            command[1] = 0x06;
            command[2] = 0x08;
            command[3] = 0x80;
            command[4] = 0x00;
            command[5] = 0x00;
            port.Write(command, 0, 6);    //{'AA','06','08','80','00','00'}
            while (port.BytesToRead != 6) ;
            port.Read(rx, 0, 6);
            textBox1.Text += BitConverter.ToString(rx) + "\r\n";
            //壓縮圖像
            command[1] = 0x05;
            command[2] = 0x00;
            command[4] = 0x00;
            port.Write(command, 0, 6);    //{'AA','05','00','00','00','00'}
            while (port.BytesToRead != 6) ;
            port.Read(rx, 0, 6);
            textBox1.Text += BitConverter.ToString(rx) + "\r\n";
            //獲取圖像
            command[1] = 0x04;
            command[2] = 0x01;
            port.Write(command, 0, 6);    //{'AA','04','01','00','00','00'}
            while (port.BytesToRead != 12) ;
            port.Read(rx, 0, 6);
            textBox1.Text += BitConverter.ToString(rx) + "\r\n";
            port.Read(rx, 0, 6);
            textBox1.Text += BitConverter.ToString(rx) + "\r\n";
            total = (rx[3]) | (rx[4] << 8);
            textBox1.Text += total + "\r\n";

            //收圖
            int pktCnt = total / (128 - 6); //68
            if ((total % (128 - 6)) != 0) pktCnt += 1;
            command[1] = 0x0E;
            command[2] = 0x00;
            command[3] = 0x00;
            command[4] = 0x00;
            command[5] = 0x00;
            byte[] pkt = new byte[128];
            char[] pktc = new char[128];
            for (int i = 0; i < pktCnt - 1; i++)
            {
                command[1] = 0x0E;
                command[2] = 0x00;
                command[3] = 0x00;
                command[4] = Convert.ToByte(i & 0xff);
                command[5] = Convert.ToByte((i >> 8) & 0xff);

                port.Write(command, 0, 6);
                while (port.BytesToRead != 128) ;
                port.Read(pkt, 0, 128);
                for (int j = 4; j < 126; j++)
                {
                    file.WriteByte(pkt[j]);
                }
                //file.write((const uint8_t *)&pkt[4], cnt-6); 
            }
            command[4] = 0xf0;
            command[5] = 0xf0;
            port.Write(command, 0, 6);
            file.Close();
            port.Close();
            //顯示圖
            FileStream fs = File.OpenRead("C:\\1/123.jpg");
            int filelength = 0;
            filelength = (int)fs.Length; //獲得檔長度
            Byte[] image = new Byte[filelength]; //建立一個位元組陣列
            fs.Read(image, 0, filelength); //按位元組流讀取
            pictureBox1.Image= System.Drawing.Image.FromStream(fs);
            fs.Close();
        }
    }
}