using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

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
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button12 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button15 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button17 = new System.Windows.Forms.Button();
            this.listBox5 = new System.Windows.Forms.ListBox();
            this.button16 = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button14 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button10 = new System.Windows.Forms.Button();
            this.listBox4 = new System.Windows.Forms.ListBox();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(177, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 50);
            this.button1.TabIndex = 0;
            this.button1.Text = "Add Item";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 3;
            this.tabControl1.Size = new System.Drawing.Size(799, 388);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.TabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.listBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(791, 357);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Item Scan";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.TabPage1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(13, 15);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(158, 319);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.ListBox1_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBox1);
            this.tabPage2.Controls.Add(this.button5);
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(791, 357);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Logs";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(137, 73);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(267, 19);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Save myscan40 folder as well (Handheld only)";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 73);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(100, 50);
            this.button5.TabIndex = 1;
            this.button5.Text = "Save logcat";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 17);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 50);
            this.button4.TabIndex = 0;
            this.button4.Text = "Clear logcat";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.button12);
            this.tabPage3.Controls.Add(this.button8);
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.button2);
            this.tabPage3.Controls.Add(this.listBox2);
            this.tabPage3.Controls.Add(this.button3);
            this.tabPage3.Location = new System.Drawing.Point(4, 27);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(791, 357);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "TripStart & EOT";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(237, 19);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(100, 50);
            this.button12.TabIndex = 6;
            this.button12.Text = "Restart App";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.Button12_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(130, 19);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(100, 50);
            this.button8.TabIndex = 5;
            this.button8.Text = "Close App";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.Button8_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button7);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Location = new System.Drawing.Point(492, 87);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(291, 153);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Manual Intent";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(113, 92);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(170, 51);
            this.button7.TabIndex = 6;
            this.button7.Text = "Send Intent";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.Button7_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(6, 120);
            this.textBox3.Name = "textBox3";
            this.textBox3.PlaceholderText = "terminalid";
            this.textBox3.Size = new System.Drawing.Size(100, 23);
            this.textBox3.TabIndex = 5;
            this.textBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox3_KeyPress);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(6, 91);
            this.textBox2.Name = "textBox2";
            this.textBox2.PlaceholderText = "storenumber";
            this.textBox2.Size = new System.Drawing.Size(100, 23);
            this.textBox2.TabIndex = 4;
            this.textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox2_KeyPress);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox3);
            this.groupBox1.Location = new System.Drawing.Point(6, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(277, 63);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SelfScanEnginePlugin Address";
            this.groupBox1.Enter += new System.EventHandler(this.GroupBox1_Enter_1);
            // 
            // listBox3
            // 
            this.listBox3.FormattingEnabled = true;
            this.listBox3.ItemHeight = 15;
            this.listBox3.Location = new System.Drawing.Point(6, 22);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(265, 34);
            this.listBox3.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(173, 87);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 50);
            this.button2.TabIndex = 1;
            this.button2.Text = "EOT";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 15;
            this.listBox2.Location = new System.Drawing.Point(23, 87);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(144, 259);
            this.listBox2.TabIndex = 2;
            this.listBox2.Tag = "";
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.ListBox2_SelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(24, 19);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 50);
            this.button3.TabIndex = 0;
            this.button3.Text = "Device Taken";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button15);
            this.tabPage4.Controls.Add(this.button13);
            this.tabPage4.Controls.Add(this.button11);
            this.tabPage4.Controls.Add(this.button9);
            this.tabPage4.Controls.Add(this.button6);
            this.tabPage4.Location = new System.Drawing.Point(4, 27);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(791, 357);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Device";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(135, 77);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(100, 50);
            this.button15.TabIndex = 4;
            this.button15.Text = "Record";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.Button15_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(28, 134);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(100, 50);
            this.button13.TabIndex = 3;
            this.button13.Text = "Connect over IP";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.Button13_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(134, 20);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(100, 50);
            this.button11.TabIndex = 2;
            this.button11.Text = "ScreenCap";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.Button11_Click);
            // 
            // button9
            // 
            this.button9.FlatAppearance.BorderSize = 0;
            this.button9.Location = new System.Drawing.Point(28, 77);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(100, 50);
            this.button9.TabIndex = 1;
            this.button9.Text = "Device Details";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.Button9_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(28, 20);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(100, 50);
            this.button6.TabIndex = 0;
            this.button6.Text = "Reboot";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Button6_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.groupBox4);
            this.tabPage6.Controls.Add(this.button16);
            this.tabPage6.Location = new System.Drawing.Point(4, 27);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(791, 357);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Add/Remove Apps";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox4);
            this.groupBox4.Controls.Add(this.button17);
            this.groupBox4.Controls.Add(this.listBox5);
            this.groupBox4.Location = new System.Drawing.Point(8, 9);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(379, 330);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Current apps";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(6, 289);
            this.textBox4.Name = "textBox4";
            this.textBox4.PlaceholderText = "Search bar";
            this.textBox4.Size = new System.Drawing.Size(261, 23);
            this.textBox4.TabIndex = 3;
            this.textBox4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox4_KeyUp);
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(273, 274);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(100, 50);
            this.button17.TabIndex = 2;
            this.button17.Text = "Uninstall";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // listBox5
            // 
            this.listBox5.FormattingEnabled = true;
            this.listBox5.ItemHeight = 15;
            this.listBox5.Location = new System.Drawing.Point(6, 22);
            this.listBox5.Name = "listBox5";
            this.listBox5.Size = new System.Drawing.Size(367, 244);
            this.listBox5.TabIndex = 1;
            this.listBox5.SelectedIndexChanged += new System.EventHandler(this.listBox5_SelectedIndexChanged);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(409, 9);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(100, 50);
            this.button16.TabIndex = 0;
            this.button16.Text = "Install APK";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.groupBox3);
            this.tabPage5.Controls.Add(this.button10);
            this.tabPage5.Controls.Add(this.listBox4);
            this.tabPage5.Location = new System.Drawing.Point(4, 27);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(791, 357);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Others";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button14);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Location = new System.Drawing.Point(369, 108);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(399, 116);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "ADB Command";
            this.groupBox3.Enter += new System.EventHandler(this.GroupBox3_Enter);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(7, 52);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(100, 50);
            this.button14.TabIndex = 1;
            this.button14.Text = "Send";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.Button14_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(7, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.PlaceholderText = "Type your ADB command here (eg. adb disconnect)";
            this.textBox1.Size = new System.Drawing.Size(386, 23);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(215, 18);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(100, 50);
            this.button10.TabIndex = 1;
            this.button10.Text = "Run";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.Button10_Click);
            // 
            // listBox4
            // 
            this.listBox4.FormattingEnabled = true;
            this.listBox4.ItemHeight = 15;
            this.listBox4.Location = new System.Drawing.Point(29, 18);
            this.listBox4.Name = "listBox4";
            this.listBox4.Size = new System.Drawing.Size(180, 274);
            this.listBox4.TabIndex = 0;
            this.listBox4.SelectedIndexChanged += new System.EventHandler(this.ListBox4_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 406);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MDC Android Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private TabPage tabPage5;
        private Button button10;
        private ListBox listBox4;
        private CheckBox checkBox1;
        private Button button11;
        private Button button12;
        private Button button13;
        private GroupBox groupBox3;
        private Button button14;
        private TextBox textBox1;
        private Button button15;
        private TabPage tabPage6;
        private Button button16;
        private GroupBox groupBox4;
        private TextBox textBox4;
        private Button button17;
        private ListBox listBox5;
    }
}

