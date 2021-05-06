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

        private void button1_Click(object sender, EventArgs e)
        {
            
            System.Diagnostics.Process.Start("CMD.exe", scanItem("8000570550075"));    

        }

        private void button2_Click(object sender, EventArgs e)
        {

            System.Diagnostics.Process.Start("CMD.exe", scanItem("8000570550082"));

        }

        private void button3_Click(object sender, EventArgs e)
        {

            string anyCommand = "/C adb shell am broadcast -a 'com.symbol.intent.device.UNDOCKED'";
            System.Diagnostics.Process.Start("CMD.exe", anyCommand);

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
