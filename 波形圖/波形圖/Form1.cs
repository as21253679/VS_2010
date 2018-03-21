using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.IO.Ports;

namespace 波形圖
{
    public partial class Form1 : Form
    {
        private Queue<double> dataQueue1 = new Queue<double>(100);
        private Queue<double> dataQueue2 = new Queue<double>(100);
        private Queue<double> dataQueue3 = new Queue<double>(100);
        int x = 0, y = 0, z = 0;
        int right = 0, thorn = 0, straight = 0,timer=0;
        int timer_delay = 0;
        Boolean right_flag = false, timer_flag = false, straight_flag=false;
        private int num = 1;//每次刪除增加幾個點
        string[] ports = SerialPort.GetPortNames();
        private SerialPort port;
        public Form1()
        {
            InitializeComponent();
        }
        private void InitChart()
        {
            //定義圖表區域
            this.chart1.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("123");
            this.chart1.ChartAreas.Add(chartArea1);
            //定義存儲和顯示點的容器
            this.chart1.Series.Clear();
            Series series1 = new Series("ax");
            Series series2 = new Series("ay");
            Series series3 = new Series("az");
            series1.ChartArea = "123";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);
            //設置圖表顯示樣式
            this.chart1.ChartAreas[0].AxisY.Minimum = -100;
            this.chart1.ChartAreas[0].AxisY.Maximum = 500;
            this.chart1.ChartAreas[0].AxisX.Interval = 5;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //設置標題
            this.chart1.Titles.Clear();
            this.chart1.Titles.Add("S01");
            this.chart1.Titles[0].Text = "加速度顯示";
            this.chart1.Titles[0].ForeColor = Color.RoyalBlue;
            this.chart1.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            //設置圖表顯示樣式
            this.chart1.Series[0].Color = Color.Red;
            this.chart1.Series[1].Color = Color.Blue;
            this.chart1.Series[2].Color = Color.Green;
            //設置線寬度
            this.chart1.Series[0].BorderWidth = 3;
            this.chart1.Series[1].BorderWidth = 3;
            this.chart1.Series[2].BorderWidth = 3;

            this.chart1.Series[0].ChartType = SeriesChartType.Line;
            this.chart1.Series[1].ChartType = SeriesChartType.Line;
            this.chart1.Series[2].ChartType = SeriesChartType.Line;

            this.chart1.Series[0].Points.Clear();
            this.chart1.Series[1].Points.Clear();
            this.chart1.Series[2].Points.Clear();
        }

        //更新隊列中的值
        private void UpdateQueueValue()
        {
            if (dataQueue1.Count > 50)
            {
                //先出列
                for (int i = 0; i < num; i++)
                {
                    dataQueue1.Dequeue();
                    dataQueue2.Dequeue();
                    dataQueue3.Dequeue();
                }
            }

            Random r = new Random();
            for (int i = 0; i < num; i++)
            {
                //dataQueue1.Enqueue(r.Next(0, 400));
                //dataQueue2.Enqueue(r.Next(0, 400));
                //dataQueue3.Enqueue(r.Next(0, 400));
                dataQueue1.Enqueue(x);
                dataQueue2.Enqueue(y);
                dataQueue3.Enqueue(z);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InitChart();
            this.timer1.Start();
            this.timer2.Start();
            this.timer3.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
            this.timer2.Stop();
            this.timer3.Stop();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            UpdateQueueValue();
            this.chart1.Series[0].Points.Clear();
            this.chart1.Series[1].Points.Clear();
            this.chart1.Series[2].Points.Clear();
            for (int i = 0; i < dataQueue1.Count; i++)
            {
                this.chart1.Series[0].Points.AddXY((i + 1), dataQueue1.ElementAt(i));
            }
            for (int i = 0; i < dataQueue2.Count; i++)
            {
                this.chart1.Series[1].Points.AddXY((i + 1), dataQueue2.ElementAt(i));
            }
            for (int i = 0; i < dataQueue3.Count; i++)
            {
                this.chart1.Series[2].Points.AddXY((i + 1), dataQueue3.ElementAt(i));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            port = new SerialPort(Convert.ToString(ports[0]), 115200, Parity.None, 8, StopBits.One);
            port.Open();
            InitChart();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int count = 0;
            int c, ss,f1=0,f2=0;
            char[] str1 = new char[5];
            char[] str2 = new char[5];
            char[] str3 = new char[5];
            //port.ReadTimeout = 300;
            if (port.BytesToRead > 0)
            {
                
                while (true)
                {
                    ss = port.ReadChar();
                    if ((char)ss == 'x')
                        break;
                }
                while (true)
                {
                    c = (port.ReadChar());
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
                    c = (port.ReadChar());
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
                    c = (port.ReadChar());
                    if ((char)c != 'd')
                    {
                        str3[count] = (char)c;
                        count++;
                    }
                    else
                        break;
                }
                count = 0;
            }
            textBox1.Text = String.Concat(str1);
            textBox2.Text = String.Concat(str2);
            textBox3.Text = String.Concat(str3);
            if (str1[0] != '\0')
            {
                x = Convert.ToInt32(String.Concat(str1));
                y = Convert.ToInt32(String.Concat(str2));
                z = Convert.ToInt32(String.Concat(str3));
                timer++;
            }
            if (timer_flag == true)
            {
                timer_delay++;
                if (timer_delay > 10)
                {
                    timer_delay = 0;
                    timer_flag = false;
                }
            }
            else
            {
                //右砍判斷
                if (right_flag == true && y > 320 && z < 240)
                {
                    right++;
                    right_flag = false;
                    f1 = 0;
                    timer_flag = true;
                }
                else if (right_flag == true)
                {
                    f1++;
                    if (f1 > 10)
                    {
                        f1 = 0;
                        right_flag = false;
                    }
                }
                //直砍判斷
                else if (straight_flag == true && z > 260)
                {
                    straight++;
                    straight_flag = false;
                    f2 = 0;
                    timer_flag = true;
                }
                else if (straight_flag == true)
                {
                    f2++;
                    if (f2 > 10)
                    {
                        f2 = 0;
                        straight_flag = false;
                    }
                }
                //右砍判斷
                else if (x >= 320 && z < 240)
                {
                    right_flag = true;
                }
                else if (x > 350 && y > 350)
                {
                    right++;
                    timer_flag = true;
                }
                //直砍判斷
                else if (x > 260)
                {
                    straight_flag = true;
                }
                //刺判斷
                else if (y > 300 && x < 260 && z < 200)
                {
                    thorn++;
                    timer_flag = true;
                }
                //直砍判斷
                else if (x > 260 && z > 260)
                {
                    straight++;
                    timer_flag = true;
                }

            }
            label4.Text = right.ToString();
            label5.Text = thorn.ToString();
            label6.Text = straight.ToString();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            label7.Text = timer.ToString();
            timer = 0;
        }
    }
}