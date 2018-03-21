using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 模擬鍵盤_彎曲九軸_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int U=0,D=0,R=0,L=0;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    U = 1;
                    label1.Text = "↑";
                    break;
                case Keys.Down:
                    D = 1;
                    label1.Text = "↓";
                    break;
                case Keys.Right:
                    R = 1;
                    label1.Text = "→";
                    break;
                case Keys.Left:
                    L = 1;
                    label1.Text = "←";
                    break;
                case Keys.A:
                    label1.Text = "A";
                    break;
                case Keys.B:
                    label1.Text = "B";
                    break;
                case Keys.C:
                    label1.Text = "C";
                    break;
            }
            if(D==1 && R==1)
                label1.Text = "↘";
            else if(U==1 && R==1)
                label1.Text = "↗";
            else if(D==1 && L==1)
                label1.Text = "↙";
            else if(U==1 && L==1)
                label1.Text = "↖";
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            label1.Text = "";
            switch (e.KeyCode)
            {
                case Keys.Up:
                    U = 0;
                    break;
                case Keys.Down:
                    D = 0;
                    break;
                case Keys.Right:
                    R = 0;
                    break;
                case Keys.Left:
                    L = 0;
                    break;
            }
        }
    }
}
