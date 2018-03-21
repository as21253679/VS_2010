namespace 程式方塊
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.bt_dim = new System.Windows.Forms.Button();
            this.bt_for = new System.Windows.Forms.Button();
            this.bt_if = new System.Windows.Forms.Button();
            this.bt_while = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.bt_output = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.刪除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bt_dim
            // 
            this.bt_dim.AccessibleDescription = "";
            this.bt_dim.AccessibleName = "";
            this.bt_dim.Cursor = System.Windows.Forms.Cursors.Default;
            this.bt_dim.Location = new System.Drawing.Point(470, 22);
            this.bt_dim.Name = "bt_dim";
            this.bt_dim.Size = new System.Drawing.Size(75, 23);
            this.bt_dim.TabIndex = 0;
            this.bt_dim.Text = "bt_dim";
            this.bt_dim.UseVisualStyleBackColor = true;
            this.bt_dim.Click += new System.EventHandler(this.bt_dim_Click);
            // 
            // bt_for
            // 
            this.bt_for.Location = new System.Drawing.Point(470, 69);
            this.bt_for.Name = "bt_for";
            this.bt_for.Size = new System.Drawing.Size(75, 23);
            this.bt_for.TabIndex = 1;
            this.bt_for.Text = "bt_for";
            this.bt_for.UseVisualStyleBackColor = true;
            this.bt_for.Click += new System.EventHandler(this.bt_for_Click);
            // 
            // bt_if
            // 
            this.bt_if.Location = new System.Drawing.Point(470, 112);
            this.bt_if.Name = "bt_if";
            this.bt_if.Size = new System.Drawing.Size(75, 23);
            this.bt_if.TabIndex = 2;
            this.bt_if.Text = "bt_if";
            this.bt_if.UseVisualStyleBackColor = true;
            // 
            // bt_while
            // 
            this.bt_while.Location = new System.Drawing.Point(470, 158);
            this.bt_while.Name = "bt_while";
            this.bt_while.Size = new System.Drawing.Size(75, 23);
            this.bt_while.TabIndex = 3;
            this.bt_while.Text = "bt_while";
            this.bt_while.UseVisualStyleBackColor = true;
            // 
            // bt_output
            // 
            this.bt_output.Location = new System.Drawing.Point(470, 271);
            this.bt_output.Name = "bt_output";
            this.bt_output.Size = new System.Drawing.Size(75, 23);
            this.bt_output.TabIndex = 4;
            this.bt_output.Text = "匯出程式碼";
            this.bt_output.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刪除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(99, 26);
            // 
            // 刪除ToolStripMenuItem
            // 
            this.刪除ToolStripMenuItem.Name = "刪除ToolStripMenuItem";
            this.刪除ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.刪除ToolStripMenuItem.Text = "刪除";
            this.刪除ToolStripMenuItem.Click += new System.EventHandler(this.刪除ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 344);
            this.Controls.Add(this.bt_output);
            this.Controls.Add(this.bt_while);
            this.Controls.Add(this.bt_if);
            this.Controls.Add(this.bt_for);
            this.Controls.Add(this.bt_dim);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bt_dim;
        private System.Windows.Forms.Button bt_for;
        private System.Windows.Forms.Button bt_if;
        private System.Windows.Forms.Button bt_while;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button bt_output;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 刪除ToolStripMenuItem;
    }
}

