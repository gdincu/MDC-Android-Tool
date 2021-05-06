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

        private IDictionary<string, string> tempList = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
            tempList.Clear();
            tempList.Add("Regular_Item", "16000275270");
            tempList.Add("​Promotion_Item", "70847811299");
            tempList.Add("​Age_Restriction_21", "80660957159");
            tempList.Add("LidItemMessage_Item", "7000570550014");
            tempList.Add("​Liquidation_Discount", "8000570550020​");
            tempList.Add("​Forbidden_Item", "8000570550181​");
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

            if (tempList.ContainsKey(listBox1.SelectedItem.ToString()))
                runCommand(scanItem(tempList[listBox1.SelectedItem.ToString()]));
            else
                MessageBox.Show("Item not found!");

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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            runCommand(scanItem(listBox2.SelectedItem.ToString()));
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            String _intent = "/C adb shell am broadcast -a 'com.mdcinternational.selfscanner.sendselfscannerconfig' --es 'storenumber' '"
                + textBox2.Text
                + "' --es 'soapendpoint' '"
                + listBox3.SelectedItem.ToString()
                + "' --es 'terminalid' '"
                + textBox3.Text
                + "'";
                
            runCommand(_intent);
        }
    }
}
