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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button1 = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            listBox1 = new ListBox();
            tabPage2 = new TabPage();
            checkBox1 = new CheckBox();
            button5 = new Button();
            button4 = new Button();
            tabPage3 = new TabPage();
            button12 = new Button();
            button8 = new Button();
            groupBox2 = new GroupBox();
            button7 = new Button();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            groupBox1 = new GroupBox();
            listBox3 = new ListBox();
            button2 = new Button();
            listBox2 = new ListBox();
            button3 = new Button();
            tabPage4 = new TabPage();
            button15 = new Button();
            button13 = new Button();
            button11 = new Button();
            button9 = new Button();
            button6 = new Button();
            tabPage6 = new TabPage();
            groupBox4 = new GroupBox();
            textBox4 = new TextBox();
            button17 = new Button();
            listBox5 = new ListBox();
            button16 = new Button();
            tabPage5 = new TabPage();
            groupBox3 = new GroupBox();
            button14 = new Button();
            textBox1 = new TextBox();
            button10 = new Button();
            listBox4 = new ListBox();
            bindingSource1 = new BindingSource(components);
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage6.SuspendLayout();
            groupBox4.SuspendLayout();
            tabPage5.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(177, 15);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(100, 50);
            button1.TabIndex = 0;
            button1.Text = "Add Item";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click;
            // 
            // tabControl1
            // 
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage6);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Dock = DockStyle.Top;
            tabControl1.HotTrack = true;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 3;
            tabControl1.Size = new System.Drawing.Size(799, 388);
            tabControl1.SizeMode = TabSizeMode.FillToRight;
            tabControl1.TabIndex = 2;
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(button1);
            tabPage1.Controls.Add(listBox1);
            tabPage1.Location = new System.Drawing.Point(4, 27);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new System.Drawing.Size(791, 357);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Item Scan";
            tabPage1.UseVisualStyleBackColor = true;
            tabPage1.Click += TabPage1_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new System.Drawing.Point(13, 15);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(158, 319);
            listBox1.TabIndex = 1;
            listBox1.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(checkBox1);
            tabPage2.Controls.Add(button5);
            tabPage2.Controls.Add(button4);
            tabPage2.Location = new System.Drawing.Point(4, 27);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new System.Drawing.Size(791, 357);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Logs";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(137, 73);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(267, 19);
            checkBox1.TabIndex = 2;
            checkBox1.Text = "Save myscan40 folder as well (Handheld only)";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
            // 
            // button5
            // 
            button5.Location = new System.Drawing.Point(12, 73);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(100, 50);
            button5.TabIndex = 1;
            button5.Text = "Save logcat";
            button5.UseVisualStyleBackColor = true;
            button5.Click += Button5_Click;
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(12, 17);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(100, 50);
            button4.TabIndex = 0;
            button4.Text = "Clear logcat";
            button4.UseVisualStyleBackColor = true;
            button4.Click += Button4_Click;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(button12);
            tabPage3.Controls.Add(button8);
            tabPage3.Controls.Add(groupBox2);
            tabPage3.Controls.Add(button2);
            tabPage3.Controls.Add(listBox2);
            tabPage3.Controls.Add(button3);
            tabPage3.Location = new System.Drawing.Point(4, 27);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new System.Drawing.Size(791, 357);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "TripStart & EOT";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            button12.Location = new System.Drawing.Point(237, 19);
            button12.Name = "button12";
            button12.Size = new System.Drawing.Size(100, 50);
            button12.TabIndex = 6;
            button12.Text = "Restart App";
            button12.UseVisualStyleBackColor = true;
            button12.Click += Button12_Click;
            // 
            // button8
            // 
            button8.Location = new System.Drawing.Point(130, 19);
            button8.Name = "button8";
            button8.Size = new System.Drawing.Size(100, 50);
            button8.TabIndex = 5;
            button8.Text = "Close App";
            button8.UseVisualStyleBackColor = true;
            button8.Click += Button8_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button7);
            groupBox2.Controls.Add(textBox3);
            groupBox2.Controls.Add(textBox2);
            groupBox2.Controls.Add(groupBox1);
            groupBox2.Location = new System.Drawing.Point(492, 87);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(291, 153);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "Manual Intent";
            // 
            // button7
            // 
            button7.Location = new System.Drawing.Point(113, 92);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(170, 51);
            button7.TabIndex = 6;
            button7.Text = "Send Intent";
            button7.UseVisualStyleBackColor = true;
            button7.Click += Button7_Click;
            // 
            // textBox3
            // 
            textBox3.Location = new System.Drawing.Point(6, 120);
            textBox3.Name = "textBox3";
            textBox3.PlaceholderText = "terminalid";
            textBox3.Size = new System.Drawing.Size(100, 23);
            textBox3.TabIndex = 5;
            textBox3.KeyPress += TextBox3_KeyPress;
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(6, 91);
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "storenumber";
            textBox2.Size = new System.Drawing.Size(100, 23);
            textBox2.TabIndex = 4;
            textBox2.KeyPress += TextBox2_KeyPress;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(listBox3);
            groupBox1.Location = new System.Drawing.Point(6, 22);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(277, 63);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "SelfScanEnginePlugin Address";
            groupBox1.Enter += GroupBox1_Enter_1;
            // 
            // listBox3
            // 
            listBox3.FormattingEnabled = true;
            listBox3.ItemHeight = 15;
            listBox3.Location = new System.Drawing.Point(6, 22);
            listBox3.Name = "listBox3";
            listBox3.Size = new System.Drawing.Size(265, 34);
            listBox3.TabIndex = 0;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(173, 87);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(100, 50);
            button2.TabIndex = 1;
            button2.Text = "EOT";
            button2.UseVisualStyleBackColor = true;
            button2.Click += Button2_Click;
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 15;
            listBox2.Location = new System.Drawing.Point(23, 87);
            listBox2.Name = "listBox2";
            listBox2.Size = new System.Drawing.Size(144, 259);
            listBox2.TabIndex = 2;
            listBox2.Tag = "";
            listBox2.SelectedIndexChanged += ListBox2_SelectedIndexChanged;
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(24, 19);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(100, 50);
            button3.TabIndex = 0;
            button3.Text = "Device Taken";
            button3.UseVisualStyleBackColor = true;
            button3.Click += Button3_Click;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(button15);
            tabPage4.Controls.Add(button13);
            tabPage4.Controls.Add(button11);
            tabPage4.Controls.Add(button9);
            tabPage4.Controls.Add(button6);
            tabPage4.Location = new System.Drawing.Point(4, 27);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new System.Drawing.Size(791, 357);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Device";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // button15
            // 
            button15.Location = new System.Drawing.Point(135, 77);
            button15.Name = "button15";
            button15.Size = new System.Drawing.Size(100, 50);
            button15.TabIndex = 4;
            button15.Text = "Record";
            button15.UseVisualStyleBackColor = true;
            button15.Click += Button15_Click;
            // 
            // button13
            // 
            button13.Location = new System.Drawing.Point(28, 134);
            button13.Name = "button13";
            button13.Size = new System.Drawing.Size(100, 50);
            button13.TabIndex = 3;
            button13.Text = "Connect over IP";
            button13.UseVisualStyleBackColor = true;
            button13.Click += Button13_Click;
            // 
            // button11
            // 
            button11.Location = new System.Drawing.Point(134, 20);
            button11.Name = "button11";
            button11.Size = new System.Drawing.Size(100, 50);
            button11.TabIndex = 2;
            button11.Text = "ScreenCap";
            button11.UseVisualStyleBackColor = true;
            button11.Click += Button11_Click;
            // 
            // button9
            // 
            button9.FlatAppearance.BorderSize = 0;
            button9.Location = new System.Drawing.Point(28, 77);
            button9.Name = "button9";
            button9.Size = new System.Drawing.Size(100, 50);
            button9.TabIndex = 1;
            button9.Text = "Device Details";
            button9.UseVisualStyleBackColor = true;
            button9.Click += Button9_Click;
            // 
            // button6
            // 
            button6.Location = new System.Drawing.Point(28, 20);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(100, 50);
            button6.TabIndex = 0;
            button6.Text = "Reboot";
            button6.UseVisualStyleBackColor = true;
            button6.Click += Button6_Click;
            // 
            // tabPage6
            // 
            tabPage6.Controls.Add(groupBox4);
            tabPage6.Controls.Add(button16);
            tabPage6.Location = new System.Drawing.Point(4, 27);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new System.Drawing.Size(791, 357);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "Add/Remove Apps";
            tabPage6.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(textBox4);
            groupBox4.Controls.Add(button17);
            groupBox4.Controls.Add(listBox5);
            groupBox4.Location = new System.Drawing.Point(8, 9);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new System.Drawing.Size(379, 330);
            groupBox4.TabIndex = 2;
            groupBox4.TabStop = false;
            groupBox4.Text = "Current apps";
            // 
            // textBox4
            // 
            textBox4.Location = new System.Drawing.Point(6, 289);
            textBox4.Name = "textBox4";
            textBox4.PlaceholderText = "Search bar";
            textBox4.Size = new System.Drawing.Size(261, 23);
            textBox4.TabIndex = 3;
            textBox4.TextChanged += textBox4_TextChanged;
            textBox4.KeyUp += textBox4_KeyUp;
            // 
            // button17
            // 
            button17.Location = new System.Drawing.Point(273, 274);
            button17.Name = "button17";
            button17.Size = new System.Drawing.Size(100, 50);
            button17.TabIndex = 2;
            button17.Text = "Uninstall";
            button17.UseVisualStyleBackColor = true;
            button17.Click += button17_Click;
            // 
            // listBox5
            // 
            listBox5.FormattingEnabled = true;
            listBox5.ItemHeight = 15;
            listBox5.Location = new System.Drawing.Point(6, 22);
            listBox5.Name = "listBox5";
            listBox5.Size = new System.Drawing.Size(367, 244);
            listBox5.TabIndex = 1;
            listBox5.SelectedIndexChanged += listBox5_SelectedIndexChanged;
            // 
            // button16
            // 
            button16.Location = new System.Drawing.Point(409, 9);
            button16.Name = "button16";
            button16.Size = new System.Drawing.Size(100, 50);
            button16.TabIndex = 0;
            button16.Text = "Install APK";
            button16.UseVisualStyleBackColor = true;
            button16.Click += button16_Click;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(groupBox3);
            tabPage5.Controls.Add(button10);
            tabPage5.Controls.Add(listBox4);
            tabPage5.Location = new System.Drawing.Point(4, 27);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new System.Drawing.Size(791, 357);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Others";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(button14);
            groupBox3.Controls.Add(textBox1);
            groupBox3.Location = new System.Drawing.Point(369, 108);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(399, 116);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "ADB Command";
            groupBox3.Enter += GroupBox3_Enter;
            // 
            // button14
            // 
            button14.Location = new System.Drawing.Point(7, 52);
            button14.Name = "button14";
            button14.Size = new System.Drawing.Size(100, 50);
            button14.TabIndex = 1;
            button14.Text = "Send";
            button14.UseVisualStyleBackColor = true;
            button14.Click += Button14_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(7, 23);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Type your ADB command here (eg. adb disconnect)";
            textBox1.Size = new System.Drawing.Size(386, 23);
            textBox1.TabIndex = 0;
            textBox1.TextChanged += TextBox1_TextChanged;
            // 
            // button10
            // 
            button10.Location = new System.Drawing.Point(215, 18);
            button10.Name = "button10";
            button10.Size = new System.Drawing.Size(100, 50);
            button10.TabIndex = 1;
            button10.Text = "Run";
            button10.UseVisualStyleBackColor = true;
            button10.Click += Button10_Click;
            // 
            // listBox4
            // 
            listBox4.FormattingEnabled = true;
            listBox4.ItemHeight = 15;
            listBox4.Location = new System.Drawing.Point(29, 18);
            listBox4.Name = "listBox4";
            listBox4.Size = new System.Drawing.Size(180, 274);
            listBox4.TabIndex = 0;
            listBox4.SelectedIndexChanged += ListBox4_SelectedIndexChanged;
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(799, 406);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MDC Android Tool";
            Load += Form1_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            tabPage6.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            tabPage5.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ResumeLayout(false);
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

