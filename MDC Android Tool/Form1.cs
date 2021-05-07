using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using MDC_Android_Tool;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox1.Text = "TTT";

            //string anyCommand = "/C adb devices -l | find 'model: '";
            //textBox1.Text += System.Diagnostics.Process.Start("CMD.exe", anyCommand).StandardOutput.ReadToEnd();
        }
        private void GroupBox1_Enter_1(object sender, EventArgs e)
        {

        }
        private void Button9_Click(object sender, EventArgs e)
        {
            
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            logsControl1.BringToFront();
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            tripStartControl1.BringToFront();
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            deviceControl1.BringToFront();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
