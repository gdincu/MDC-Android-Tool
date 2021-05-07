namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.logsControl1 = new MDC_Android_Tool.LogsControl();
            this.deviceControl1 = new MDC_Android_Tool.DeviceControl();
            this.tripStartControl1 = new MDC_Android_Tool.TripStartControl();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(10, 412);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(175, 23);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(47)))), ((int)(((byte)(129)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.button12);
            this.panel1.Controls.Add(this.button11);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.button10);
            this.panel1.Controls.Add(this.button9);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 400);
            this.panel1.TabIndex = 4;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // button12
            // 
            this.button12.FlatAppearance.BorderSize = 0;
            this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button12.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button12.ForeColor = System.Drawing.Color.White;
            this.button12.Image = ((System.Drawing.Image)(resources.GetObject("button12.Image")));
            this.button12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button12.Location = new System.Drawing.Point(0, 207);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(200, 54);
            this.button12.TabIndex = 6;
            this.button12.Text = "     Device";
            this.button12.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.Button12_Click);
            // 
            // button11
            // 
            this.button11.FlatAppearance.BorderSize = 0;
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button11.ForeColor = System.Drawing.Color.White;
            this.button11.Image = ((System.Drawing.Image)(resources.GetObject("button11.Image")));
            this.button11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button11.Location = new System.Drawing.Point(0, 148);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(200, 54);
            this.button11.TabIndex = 6;
            this.button11.Text = "     Trip Start && EOT";
            this.button11.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.Button11_Click);
            // 
            // button10
            // 
            this.button10.FlatAppearance.BorderSize = 0;
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button10.ForeColor = System.Drawing.Color.White;
            this.button10.Image = ((System.Drawing.Image)(resources.GetObject("button10.Image")));
            this.button10.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button10.Location = new System.Drawing.Point(0, 88);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(200, 54);
            this.button10.TabIndex = 6;
            this.button10.Text = "     Logs";
            this.button10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.Button10_Click);
            // 
            // button9
            // 
            this.button9.FlatAppearance.BorderSize = 0;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button9.ForeColor = System.Drawing.Color.White;
            this.button9.Image = ((System.Drawing.Image)(resources.GetObject("button9.Image")));
            this.button9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button9.Location = new System.Drawing.Point(0, 35);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(200, 54);
            this.button9.TabIndex = 6;
            this.button9.Text = "     Item Scan";
            this.button9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.Button9_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(8)))), ((int)(((byte)(55)))));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(200, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(600, 10);
            this.panel2.TabIndex = 5;
            // 
            // logsControl1
            // 
            this.logsControl1.Location = new System.Drawing.Point(208, 16);
            this.logsControl1.Name = "logsControl1";
            this.logsControl1.Size = new System.Drawing.Size(550, 400);
            this.logsControl1.TabIndex = 0;
            // 
            // deviceControl1
            // 
            this.deviceControl1.Location = new System.Drawing.Point(208, 16);
            this.deviceControl1.Name = "deviceControl1";
            this.deviceControl1.Size = new System.Drawing.Size(550, 400);
            this.deviceControl1.TabIndex = 6;
            // 
            // tripStartControl1
            // 
            this.tripStartControl1.Location = new System.Drawing.Point(208, 16);
            this.tripStartControl1.Name = "tripStartControl1";
            this.tripStartControl1.Size = new System.Drawing.Size(550, 400);
            this.tripStartControl1.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 400);
            this.Controls.Add(this.logsControl1);
            this.Controls.Add(this.tripStartControl1);
            this.Controls.Add(this.deviceControl1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(208, 16);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MDC Android Tool";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button10;
        private MDC_Android_Tool.LogsControl logsControl1;
        private MDC_Android_Tool.DeviceControl deviceControl1;
        private MDC_Android_Tool.TripStartControl tripStartControl1;
    }
}

