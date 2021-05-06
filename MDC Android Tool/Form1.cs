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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private String scanItem(String ItemBarcode)
        {
            return "/C adb shell am broadcast -a 'barcodescanner.RECVR' --es 'com.motorolasolutions.emdk.datawedge.data_string' '" + ItemBarcode + "' --es 'com.motorolasolutions.emdk.datawedge.source' 'scanner' --es 'com.motorolasolutions.emdk.datawedge.label_type' 'LABEL - TYPE - EAN13'";
        }

        private void runCommand(String command)
        {
            System.Diagnostics.Process.Start("CMD.exe", command);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            runCommand(scanItem("8000570550075"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            runCommand(scanItem("8000570550082"));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            runCommand("/C adb shell am broadcast -a 'com.symbol.intent.device.UNDOCKED'");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            runCommand("/C adb logcat -c & adb logcat -c & EXIT");
            MessageBox.Show("Cleared!", "Clear logcat");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("File saved to ....", "Save logcat");
            //System.Diagnostics.Process.Start("CMD.exe", "adb logcat -d > logcat.log");

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox1.Text = "TTT";

            //string anyCommand = "/C adb devices -l | find 'model: '";
            //textBox1.Text += System.Diagnostics.Process.Start("CMD.exe", anyCommand).StandardOutput.ReadToEnd();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            runCommand("/C adb reboot");
        }
    }
}
