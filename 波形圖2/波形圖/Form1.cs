using System;
using System.Management;
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
using System.Text.RegularExpressions;

namespace 波形圖
{
    public partial class Form1 : Form
    {
        private Queue<double> dataQueue1 = new Queue<double>(100);
        private Queue<double> dataQueue2 = new Queue<double>(100);
        private Queue<double> dataQueue3 = new Queue<double>(100);
        private Queue<double> dataQueue1_2 = new Queue<double>(100);
        private Queue<double> dataQueue2_2 = new Queue<double>(100);
        private Queue<double> dataQueue3_2 = new Queue<double>(100);
        private Queue<double> dataQueue1_3 = new Queue<double>(100);
        private Queue<double> dataQueue2_3 = new Queue<double>(100);
        private Queue<double> dataQueue3_3 = new Queue<double>(100);
        double Degree = Math.Acos(0.0) / 90.0;        //角度
        int x = 0, y = 0, z = 0;                      //加速度
        int gx = 0, gy = 0, gz = 0;                   //陀螺儀
        double hx = 0, hy = 0, hz = 0;                //磁力計
        int right = 0, left = 0, thorn = 0, straight = 0, timer = 0;   //動作
        int timer_delay = 0;                          //判定完成後延遲
        int []z1=new int[5];     //紀錄上一個z
        Boolean timer_delay_flag = false;             //延遲旗標
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
            this.chart1.ChartAreas[0].AxisY.Minimum = -250;
            this.chart1.ChartAreas[0].AxisY.Maximum = 250;
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

        private void InitChart2()
        {
            //定義圖表區域
            this.chart2.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("123");
            this.chart2.ChartAreas.Add(chartArea1);
            //定義存儲和顯示點的容器
            this.chart2.Series.Clear();
            Series series1 = new Series("gx");
            Series series2 = new Series("gy");
            Series series3 = new Series("gz");
            series1.ChartArea = "123";
            this.chart2.Series.Add(series1);
            this.chart2.Series.Add(series2);
            this.chart2.Series.Add(series3);
            //設置圖表顯示樣式
            this.chart2.ChartAreas[0].AxisY.Minimum = -300;
            this.chart2.ChartAreas[0].AxisY.Maximum = 300;
            this.chart2.ChartAreas[0].AxisX.Interval = 5;
            this.chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //設置標題
            this.chart2.Titles.Clear();
            this.chart2.Titles.Add("S01");
            this.chart2.Titles[0].Text = "陀螺儀顯示";
            this.chart2.Titles[0].ForeColor = Color.RoyalBlue;
            this.chart2.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            //設置圖表顯示樣式
            this.chart2.Series[0].Color = Color.Red;
            this.chart2.Series[1].Color = Color.Blue;
            this.chart2.Series[2].Color = Color.Green;
            //設置線寬度
            this.chart2.Series[0].BorderWidth = 3;
            this.chart2.Series[1].BorderWidth = 3;
            this.chart2.Series[2].BorderWidth = 3;

            this.chart2.Series[0].ChartType = SeriesChartType.Line;
            this.chart2.Series[1].ChartType = SeriesChartType.Line;
            this.chart2.Series[2].ChartType = SeriesChartType.Line;

            this.chart2.Series[0].Points.Clear();
            this.chart2.Series[1].Points.Clear();
            this.chart2.Series[2].Points.Clear();
        }

        private void InitChart3()
        {
            //定義圖表區域
            this.chart3.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("123");
            this.chart3.ChartAreas.Add(chartArea1);
            //定義存儲和顯示點的容器
            this.chart3.Series.Clear();
            Series series1 = new Series("hx");
            Series series2 = new Series("hy");
            Series series3 = new Series("hz");
            series1.ChartArea = "123";
            this.chart3.Series.Add(series1);
            this.chart3.Series.Add(series2);
            this.chart3.Series.Add(series3);
            //設置圖表顯示樣式
            this.chart3.ChartAreas[0].AxisY.Minimum = -100;
            this.chart3.ChartAreas[0].AxisY.Maximum = 100;
            this.chart3.ChartAreas[0].AxisX.Interval = 5;
            this.chart3.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart3.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //設置標題
            this.chart3.Titles.Clear();
            this.chart3.Titles.Add("S01");
            this.chart3.Titles[0].Text = "磁力計顯示";
            this.chart3.Titles[0].ForeColor = Color.RoyalBlue;
            this.chart3.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            //設置圖表顯示樣式
            this.chart3.Series[0].Color = Color.Red;
            this.chart3.Series[1].Color = Color.Blue;
            this.chart3.Series[2].Color = Color.Green;
            //設置線寬度
            this.chart3.Series[0].BorderWidth = 3;
            this.chart3.Series[1].BorderWidth = 3;
            this.chart3.Series[2].BorderWidth = 3;

            this.chart3.Series[0].ChartType = SeriesChartType.Line;
            this.chart3.Series[1].ChartType = SeriesChartType.Line;
            this.chart3.Series[2].ChartType = SeriesChartType.Line;

            this.chart2.Series[0].Points.Clear();
            this.chart2.Series[1].Points.Clear();
            this.chart2.Series[2].Points.Clear();
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

            for (int i = 0; i < num; i++)
            {
                dataQueue1.Enqueue(x);
                dataQueue2.Enqueue(y);
                dataQueue3.Enqueue(z);
            }
        }
        //更新隊列中的值
        private void UpdateQueueValue2()
        {
            if (dataQueue1_2.Count > 50)
            {
                //先出列
                for (int i = 0; i < num; i++)
                {
                    dataQueue1_2.Dequeue();
                    dataQueue2_2.Dequeue();
                    dataQueue3_2.Dequeue();
                }
            }

            for (int i = 0; i < num; i++)
            {
                dataQueue1_2.Enqueue(gx);
                dataQueue2_2.Enqueue(gy);
                dataQueue3_2.Enqueue(gz);
            }
        }
        //更新隊列中的值
        private void UpdateQueueValue3()
        {
            if (dataQueue1_3.Count > 50)
            {
                //先出列
                for (int i = 0; i < num; i++)
                {
                    dataQueue1_3.Dequeue();
                    dataQueue2_3.Dequeue();
                    dataQueue3_3.Dequeue();
                }
            }

            for (int i = 0; i < num; i++)
            {
                dataQueue1_3.Enqueue((int)hx);
                dataQueue2_3.Enqueue((int)hy);
                dataQueue3_3.Enqueue((int)hz);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InitChart();
            InitChart2();
            InitChart3();
            this.timer1_a.Start();
            this.RX.Start();
            this.update_s.Start();
            this.timer4_g.Start();
            this.timer5_h.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.timer1_a.Stop();
            this.RX.Stop();
            this.update_s.Stop();
            this.timer4_g.Stop();
            this.timer5_h.Stop();
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
            //port = new SerialPort(Convert.ToString(ports[0]), 115200, Parity.None, 8, StopBits.One);
            port = new SerialPort("COM22", 115200, Parity.None, 8, StopBits.One);
            port.Open();
            InitChart();
            InitChart2();
            InitChart3();
            //GetSerialPort();
        }

        private void GetSerialPort()
        {
            try
            {
                ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_PnPEntity");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string s = string.Format("{0}", queryObj["Name"]);
                    if (s.Length >= 8)
                        s = s.Substring(0, s.Length - 8);
                    if (s == "Prolific USB-to-Serial Comm Port")
                        Console.WriteLine(s);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int count = 0;
            int c, ss;
            char[] str1 = new char[5];
            char[] str2 = new char[5];
            char[] str3 = new char[5];
            char[] str4 = new char[5];
            char[] str5 = new char[5];
            char[] str6 = new char[5];
            char[] str7 = new char[5];
            char[] str8 = new char[5];
            char[] str9 = new char[5];
            //port.ReadTimeout = 300;
            if (port.BytesToRead > 0)
            {
                try
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
                        if ((char)c != 'a')
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
                        c = (port.ReadChar());
                        if ((char)c != 'b')
                        {
                            str4[count] = (char)c;
                            count++;
                        }
                        else
                            break;
                    }
                    count = 0;
                    while (true)
                    {
                        c = (port.ReadChar());
                        if ((char)c != 'c')
                        {
                            str5[count] = (char)c;
                            count++;
                        }
                        else
                            break;
                    }
                    count = 0;
                    while (true)
                    {
                        c = (port.ReadChar());
                        if ((char)c != 'q')
                        {
                            str6[count] = (char)c;
                            count++;
                        }
                        else
                            break;
                    }
                    count = 0;
                    while (true)
                    {
                        c = (port.ReadChar());
                        if ((char)c != 'w')
                        {
                            str7[count] = (char)c;
                            count++;
                        }
                        else
                            break;
                    }
                    count = 0;
                    while (true)
                    {
                        c = (port.ReadChar());
                        if ((char)c != 'e')
                        {
                            str8[count] = (char)c;
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
                            str9[count] = (char)c;
                            count++;
                        }
                        else
                            break;
                    }
                    count = 0;
                }
                catch { }
            }
            textBox1.Text = String.Concat(str1);
            textBox2.Text = String.Concat(str2);
            textBox3.Text = String.Concat(str3);
            textBox4.Text = String.Concat(str4);
            textBox5.Text = String.Concat(str5);
            textBox6.Text = String.Concat(str6);
            textBox7.Text = String.Concat(str7);
            textBox8.Text = String.Concat(str8);
            textBox9.Text = String.Concat(str9);
            if (str1[0] != '\0')
            {
                try
                {
                    x = Convert.ToInt32(String.Concat(str1));
                    y = Convert.ToInt32(String.Concat(str2));
                    z = Convert.ToInt32(String.Concat(str3));
                    gx = Convert.ToInt32(String.Concat(str4));
                    gy = Convert.ToInt32(String.Concat(str5));
                    gz = Convert.ToInt32(String.Concat(str6));
                    hx = Convert.ToDouble(String.Concat(str7));
                    hy = Convert.ToDouble(String.Concat(str8));
                    hz = Convert.ToDouble(String.Concat(str9));
                }
                catch { }
                timer++;
            }
            if (timer_delay_flag == true)
            {
                timer_delay++;
                if (timer_delay > 20)
                {
                    timer_delay = 0;
                    timer_delay_flag = false;
                }
            }
            else
            {
                action();
            }
            label21.Text = left.ToString();
            label4.Text = right.ToString();
            label5.Text = thorn.ToString();
            label6.Text = straight.ToString();
            if (hx == 0)
            {
                if (hy > 0)
                    label9.Text = "90度";
                else if (hy < 0)
                    label9.Text = "-90度";
            }
            else
            {
                label9.Text = ((int)((Math.Atan(hy / hx)) / Degree)).ToString() + "度";
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            label7.Text = timer.ToString();
            timer = 0;
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            UpdateQueueValue2();
            this.chart2.Series[0].Points.Clear();
            this.chart2.Series[1].Points.Clear();
            this.chart2.Series[2].Points.Clear();
            for (int i = 0; i < dataQueue1_2.Count; i++)
            {
                this.chart2.Series[0].Points.AddXY((i + 1), dataQueue1_2.ElementAt(i));
            }
            for (int i = 0; i < dataQueue2_2.Count; i++)
            {
                this.chart2.Series[1].Points.AddXY((i + 1), dataQueue2_2.ElementAt(i));
            }
            for (int i = 0; i < dataQueue3_2.Count; i++)
            {
                this.chart2.Series[2].Points.AddXY((i + 1), dataQueue3_2.ElementAt(i));
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            UpdateQueueValue3();
            this.chart3.Series[0].Points.Clear();
            this.chart3.Series[1].Points.Clear();
            this.chart3.Series[2].Points.Clear();
            for (int i = 0; i < dataQueue1_3.Count; i++)
            {
                this.chart3.Series[0].Points.AddXY((i + 1), dataQueue1_3.ElementAt(i));
            }
            for (int i = 0; i < dataQueue2_3.Count; i++)
            {
                this.chart3.Series[1].Points.AddXY((i + 1), dataQueue2_3.ElementAt(i));
            }
            for (int i = 0; i < dataQueue3_3.Count; i++)
            {
                this.chart3.Series[2].Points.AddXY((i + 1), dataQueue3_3.ElementAt(i));
            }
        }
        void action()
        {
            z1[4] = z1[3];
            z1[3] = z1[2];
            z1[2] = z1[1];
            z1[1] = z1[0];
            z1[0] = z;
            //刺
            if (y < -130)
            {
                thorn++;
                timer_delay_flag = true;
            }
            //右砍,直砍
            else if (x < -100 && gz < -200)
            {
                if (z1[4] > 50)
                {
                    right++;
                    timer_delay_flag = true;
                }
                else if (z1[4] < -50)
                {
                    left++;
                    timer_delay_flag = true;
                }
                else
                {
                    straight++;
                    timer_delay_flag = true;
                }
            }
        }
    }
}