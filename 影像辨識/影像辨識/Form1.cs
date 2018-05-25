using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Emgu.CV.CvEnum;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Runtime.InteropServices;
using Emgu.CV.VideoSurveillance;

namespace 影像辨識
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog filename = new OpenFileDialog();
            filename.InitialDirectory = Environment.CurrentDirectory;
            filename.Filter = "files (*.jpg;*.jpge;*.png)|*.jpg;*.jpge;*.png";
            if (filename.ShowDialog() == DialogResult.OK)
            {
                string name = filename.FileName;
                label1.Text = name;
                pictureBox1.Image = new Bitmap(Image.FromFile(label1.Text), 144, 144);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (label1.Text != "")
            {
                Bitmap imgOutput = new Bitmap(Image.FromFile(label1.Text), 144, 144);//圖片位址與尺寸
                Image<Bgr, Byte> m_BgrImage = new Image<Bgr, byte>(imgOutput);//彩色圖片
                Image<Gray, Byte> m_SourceImage = new Image<Gray, byte>(m_BgrImage.ToBitmap());//灰階圖片
                Image<Gray, Byte> m_ThresholdImage = new Image<Gray, Byte>(m_SourceImage.Size);//二值化圖片
                Image<Gray, Byte> m_CannyImage = new Image<Gray, Byte>(m_SourceImage.Size);//繪出線條
                Image<Gray, Byte> m_ContoursImage = new Image<Gray, Byte>(m_SourceImage.Size);//繪出邊緣
                Image<Gray, Byte> m_ErodeImage = new Image<Gray, byte>(m_BgrImage.ToBitmap());//擴張像素
                Image<Bgr, Byte> m_FrameImage = new Image<Bgr, Byte>(imgOutput);//加邊框圖

                CvInvoke.cvThreshold(m_SourceImage, m_ThresholdImage, 0.0001, 255.0, Emgu.CV.CvEnum.THRESH.CV_THRESH_OTSU);//二值化圖片
                CvInvoke.cvCanny(m_SourceImage, m_CannyImage, 0, 255, 3);//繪出線條

                /////繪出邊緣/////
                IntPtr Dynstorage = CvInvoke.cvCreateMemStorage(0);//圖片暫存
                IntPtr Dyncontour = new IntPtr();//存放检测到的图像块的首地址
                MCvContour con = new MCvContour();
                CvInvoke.cvFindContours(m_CannyImage, Dynstorage, ref Dyncontour, Marshal.SizeOf(con), Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE, Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_LINK_RUNS, new Point(0, 0));
                CvInvoke.cvDrawContours(m_ContoursImage, Dyncontour, new MCvScalar(255, 255, 255), new MCvScalar(255, 255, 255), 2, 1, Emgu.CV.CvEnum.LINE_TYPE.CV_AA, new Point(0, 0));

                CvInvoke.cvThreshold(m_CannyImage, m_ErodeImage, 0.0001, 255.0, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY_INV);
                m_ErodeImage = m_ErodeImage.Erode(1);//擴張處理(擴張x像素)

                /////畫外框/////
                Rectangle BoundingBox = CvInvoke.cvBoundingRect(m_CannyImage, true);
                CvInvoke.cvRectangleR(m_ContoursImage, BoundingBox, new MCvScalar(255, 0, 255, 255), 3, LINE_TYPE.CV_AA, 0);

                /////去背/////
                Image<Gray, Byte> foregroundMask = m_BgrImage.GrabCut(BoundingBox, 5);
                for (int w = 0; w < foregroundMask.Width; w++)
                {
                    for (int h = 0; h < foregroundMask.Height; h++)
                    {
                        if (foregroundMask[h, w].Intensity == 1)
                        {
                            foregroundMask[h, w] = new Gray(50);
                        }
                        else if (foregroundMask[h, w].Intensity == 2)
                        {
                            foregroundMask[h, w] = new Gray(100);
                        }
                        else if (foregroundMask[h, w].Intensity == 3)
                        {
                            foregroundMask[h, w] = new Gray(150);
                        }
                    }
                }
                //去背2
                Image<Gray, Byte> foregroundMask2 = foregroundMask;
                foregroundMask2 = foregroundMask.And(new Gray(2));
                Image<Bgr, Byte> result = m_BgrImage.Copy(foregroundMask2);

                //兩圖相加
                /*Image<Bgr, byte> m_addImage = new Image<Bgr, byte>(144, 144);
                Image<Gray, byte> m_addImage2 = new Image<Gray, byte>(144, 144,new Gray(255));
                CvInvoke.cvAdd(new Image<Bgr, byte>(new Bitmap(Image.FromFile("人.jpg"), 144, 144)), new Image<Bgr, byte>(new Bitmap(Image.FromFile("b.png"), 144, 144)), m_addImage, m_addImage2);*/

                //pictureBox_Image
                pictureBox1.Image = m_BgrImage.ToBitmap();
                pictureBox2.Image = result.ToBitmap();
                pictureBox3.Image = m_SourceImage.ToBitmap();
                pictureBox4.Image = m_ThresholdImage.ToBitmap();
                pictureBox5.Image = m_CannyImage.ToBitmap();
                pictureBox6.Image = m_ContoursImage.ToBitmap();
                pictureBox7.Image = m_ErodeImage.ToBitmap();
                pictureBox8.Image = foregroundMask.ToBitmap();
            }
        }

        public static Bitmap GrayReverse(Bitmap bmp)//黑白反轉
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //获取该点的像素的RGB的颜色  
                    Color color = bmp.GetPixel(i, j);
                    Color newColor = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
                    bmp.SetPixel(i, j, newColor);
                }
            }
            return bmp;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Image<Bgr, byte> m_diffImage = new Image<Bgr, byte>(144, 144);
            CvInvoke.cvAbsDiff(new Image<Bgr, byte>(new Bitmap(Image.FromFile("a.jpg"), 144, 144)), new Image<Bgr, byte>(new Bitmap(Image.FromFile("a2.jpg"), 144, 144)), m_diffImage);//比對兩張圖差異
            
            pictureBox1.Image = m_diffImage.ToBitmap();
        }
    }
}