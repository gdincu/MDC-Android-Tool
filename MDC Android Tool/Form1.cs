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
using System.IO;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        public IDictionary<string, string> Items = new Dictionary<string, string>();
        public IDictionary<string, string> Commands = new Dictionary<string, string>();
        public IDictionary<string, string> EOTBarcodes = new Dictionary<string, string>();
        public IDictionary<string, string> URIs = new Dictionary<string, string>();
        String Settings = Path.Combine(Environment.CurrentDirectory, "MDCAndroidTool.xml");        
            
        public Form1()
        {
            ReadValues(Items, "Items");
            ReadValues(Commands, "Commands");
            ReadValues(EOTBarcodes, "EOTBarcodes");
            ReadValues(URIs, "URIs");
            InitializeComponent();            
        }

        private void ReadValues (IDictionary<string, string> Dictionary, string TagName)
        {
            Dictionary.Clear();
            foreach (XElement level1Element in XElement.Load(Settings).Elements(TagName))
                foreach (XElement level2Element in level1Element.Elements(TagName[..^1]))
                    Dictionary.Add(level2Element.Attribute("name").Value, level2Element.Attribute("value").Value);
        }

        private String scanItem(String ItemBarcode)
        {
            return Commands["ScanItem1"] + ItemBarcode + Commands["ScanItem2"];
        }

        private void runCommand(String command)
        {
            System.Diagnostics.Process.Start("CMD.exe", command);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (Items.ContainsKey(listBox1.SelectedItem.ToString()))
                runCommand(scanItem(Items[listBox1.SelectedItem.ToString()]));
            else
                MessageBox.Show("Item not found!");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            runCommand(Commands["Undocked"]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            runCommand(Commands["ClearLogcat"]);
            MessageBox.Show("Cleared!", "Clear logcat");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("CMD.exe", "/C adb logcat -d > logcat.log");
            MessageBox.Show("File saved to " + Path.Combine(Environment.CurrentDirectory, "logcat.log"), "Save logcat");
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //string anyCommand = "/C adb devices -l | find 'model: '";
            //textBox1.Text += System.Diagnostics.Process.Start("CMD.exe", anyCommand).StandardOutput.ReadToEnd();
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            runCommand(Commands["Reboot"]);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            runCommand(scanItem(EOTBarcodes[listBox2.SelectedItem.ToString()]));
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            String _intent = Commands["Intent1"]
                + textBox2.Text
                + Commands["Intent2"]
                + URIs[listBox3.SelectedItem.ToString()]
                + Commands["Intent3"]
                + textBox3.Text+ "'";

            runCommand(_intent);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Path.Combine(Environment.CurrentDirectory, "CloseCurrentApp.bat"));

        }

        private void button9_Click(object sender, EventArgs e)
        {

            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "DeviceDetails.bat");
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.RedirectStandardOutput = true;
            myProcess.Start();
            String DeviceDetails = myProcess.StandardOutput.ReadToEnd();
            MessageBox.Show(DeviceDetails,"Paste these details where needed!");
            Clipboard.SetText(DeviceDetails);

        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            runCommand(Commands[listBox4.SelectedItem.ToString()]);
        }
    }
}
