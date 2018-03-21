using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 程式方塊
{
    public partial class Form1 : Form
    {
        private MouseEventArgs _pos = null;
        Control _ctrl = null;//存放右鍵選擇的控制項
        public Form1()
        {
            InitializeComponent();
        }

        private void bt_dim_Click(object sender, EventArgs e)
        {
            int x=0, y=0;
            redo:
            foreach (Control control in this.Controls)
            {
                if (control.GetType().Name == "GroupBox")
                {
                    if (control.Location.X == x && control.Location.Y == y)
                    {
                        x += 15;
                        y += 15;
                        goto redo;
                    }
                }
            }
            GroupBox g = new GroupBox();
            TextBox t1 = new TextBox();
            ComboBox c1 = new ComboBox();
            g.BackColor = Color.LightGray;
            //創GroupBox
            this.Controls.Add(g);
            g.Location = new System.Drawing.Point(x, y);
            g.Size = new System.Drawing.Size(130, 50);
            g.Text = "Dim";
            //t1
            g.Controls.Add(t1);
            t1.Location = new System.Drawing.Point(15, 15);
            t1.Size = new System.Drawing.Size(40, 20);
            //c1
            g.Controls.Add(c1);
            c1.Items.AddRange(new object[] { "int", "float", "char", "string" });
            c1.Location = new System.Drawing.Point(65, 15);
            c1.Size = new System.Drawing.Size(60, 20);
            //右鍵功能表
            g.ContextMenuStrip = this.contextMenuStrip1;
            
            g.MouseMove += ctrl_MouseMove;
            g.MouseDown += ctrl_MouseDown;
            g.BringToFront();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(bt_dim, "定義變數");
            toolTip1.SetToolTip(bt_for, "迴圈4r43r43rewr");
            toolTip1.SetToolTip(bt_if, "ewrerwrwr");
            toolTip1.SetToolTip(bt_while, "fregergerve");
        }

        private void ctrl_MouseMove(object sender, MouseEventArgs e)
        {
            Control ctrl = sender as Control;
            if (ctrl.Capture && e.Button == MouseButtons.Left)
            {
                //DoDragDrop(ctrl, DragDropEffects.Move);//定義拖曳圖示
                ctrl.Top = e.Y + ctrl.Location.Y - _pos.Y;
                ctrl.Left = e.X + ctrl.Location.X - _pos.X;
            }
        }

        private void ctrl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _pos = e;//按下時記錄位置
                ((GroupBox)sender).BringToFront();
            }
            if (e.Button == MouseButtons.Right)
            {
                _ctrl = (GroupBox)sender;
            }
        }

        private void bt_for_Click(object sender, EventArgs e)
        {
            int x = 0, y = 0;
            redo:
            foreach (Control control in this.Controls)
            {
                if (control.GetType().Name == "GroupBox")
                {
                    if (control.Location.X == x && control.Location.Y == y)
                    {
                        x += 15;
                        y += 15;
                        goto redo;
                    }
                }
            }
            GroupBox g = new GroupBox();
            TextBox t1 = new TextBox();
            TextBox t2 = new TextBox();
            TextBox t3 = new TextBox();
            g.BackColor = Color.FromArgb(120,40,40,40);
            //創GroupBox
            this.Controls.Add(g);
            g.Location = new System.Drawing.Point(x, y);
            g.Size = new System.Drawing.Size(160, 50);
            g.Text = "For";
            //t1
            g.Controls.Add(t1);
            t1.Location = new System.Drawing.Point(15, 15);
            t1.Size = new System.Drawing.Size(40, 20);
            //t2
            g.Controls.Add(t2);
            t2.Location = new System.Drawing.Point(60, 15);
            t2.Size = new System.Drawing.Size(40, 20);
            //t3
            g.Controls.Add(t3);
            t3.Location = new System.Drawing.Point(105, 15);
            t3.Size = new System.Drawing.Size(40, 20);
            //右鍵功能表
            g.ContextMenuStrip = this.contextMenuStrip1;

            g.MouseMove += ctrl_MouseMove;
            g.MouseDown += ctrl_MouseDown;
            g.BringToFront();
        }

        private void 刪除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controls.Remove(_ctrl);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Transparent red, green, and blue brushes.
            SolidBrush trnsRedBrush = new SolidBrush(Color.FromArgb(120, 255, 0, 0));
            SolidBrush trnsGreenBrush = new SolidBrush(Color.FromArgb(120, 0, 255, 0));
            SolidBrush trnsBlueBrush = new SolidBrush(Color.FromArgb(120, 0, 0, 255));

            // Base and height of the triangle that is used to position the
            // circles. Each vertex of the triangle is at the center of one of the
            // 3 circles. The base is equal to the diameter of the circles.
            float triBase = 100;
            float triHeight = (float)Math.Sqrt(3 * (triBase * triBase) / 4);

            // Coordinates of first circle's bounding rectangle.
            float x1 = 40;
            float y1 = 40;

            // Fill 3 over-lapping circles. Each circle is a different color.
            g.FillEllipse(trnsRedBrush, x1, y1, 2 * triHeight, 2 * triHeight);
            g.FillEllipse(trnsGreenBrush, x1 + triBase / 2, y1 + triHeight,
                2 * triHeight, 2 * triHeight);
            g.FillEllipse(trnsBlueBrush, x1 + triBase, y1, 2 * triHeight, 2 * triHeight);
        }
    }
}
