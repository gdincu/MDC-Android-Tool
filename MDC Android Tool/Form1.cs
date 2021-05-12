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
        public IDictionary<string, string> HandheldDevices = new Dictionary<string, string>();
        String Settings = Path.Combine(Environment.CurrentDirectory, "MDCAndroidTool.xml");
        Process myProcess = new Process();
        bool SaveMyScan40Folder = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReadValues(Items, "Items");
            ReadValues(EOTBarcodes, "EOTBarcodes");
            ReadValues(URIs, "URIs");
            ReadValues(Commands, "Commands");
            ReadValues(HandheldDevices, "HandheldDevices");

            Items.ToList().ForEach(x => this.listBox1.Items.Add(x.Key));
            EOTBarcodes.ToList().ForEach(x => this.listBox2.Items.Add(x.Key));
            URIs.ToList().ForEach(x => this.listBox3.Items.Add(x.Key));
            Commands.ToList().ForEach(x => this.listBox4.Items.Add(x.Key));
            
        }

        private void ReadValues (IDictionary<string, string> Dictionary, string TagName)
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/linq/retrieve-value-element
            //foreach (XElement level1Element in XElement.Load(Settings).Elements(TagName))
            //    foreach (XElement level2Element in level1Element.Elements(TagName[..^1]))
            //        Dictionary.Add(level2Element.Attribute("name").Value, level2Element.Value);

            List<XElement> XMLSettings = XElement.Load(Settings).Elements(TagName).ToList();
            XMLSettings.ForEach(x => 
                x.Elements(TagName[..^1]).ToList().ForEach(y => 
                    Dictionary.Add(y.Attribute("name").Value, y.Value)));
        }

        private String scanItem(String ItemBarcode)
        {
            return Commands["ScanItem1"] + ItemBarcode + Commands["ScanItem2"];
        }

        private void runCommand(String command)
        {
            //https://stackoverflow.com/questions/19257041/run-cmd-command-without-displaying-it
            myProcess.StartInfo.FileName = "CMD.exe";
            myProcess.StartInfo.Arguments = "/C " + command;
            myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            myProcess.Start();
            myProcess.WaitForExit();
            //Initial version -- System.Diagnostics.Process.Start("CMD.exe", "/C " + command);
        }

        private void startProcess(String filename)
        {
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, filename);
            myProcess.StartInfo.RedirectStandardOutput = true;
            myProcess.Start();
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
            startProcess("DeviceDetails.bat");
            String DeviceDetails = myProcess.StandardOutput.ReadToEnd();
            myProcess.Close();

            //https://www.c-sharpcorner.com/UploadFile/mahesh/savefiledialog-in-C-Sharp/
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @Environment.CurrentDirectory;
            saveFileDialog1.Title = "Save logcat";
            //saveFileDialog1.CheckFileExists = true;
            //saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "log";
            saveFileDialog1.Filter = "Logcat (*.log)|*.log|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = DeviceDetails.Trim() + "_";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                System.Diagnostics.Process.Start("CMD.exe", @$"/C adb logcat -d > ""{saveFileDialog1.FileName}"" ");

            //Checks whether the device details include any of the handheld names recorded in the MDCAndroidTool.xml file
            //Also checks whether the user ticked the SaveMyScan40Folder checkbox
            //Downloads the entire /sdcard/mdc/myscan40 folder to the specified path and renames it using the device details
            if (HandheldDevices.Any(y => DeviceDetails.Contains(y.Key)) && SaveMyScan40Folder) { 
                //Checks whether the folder already exists and creates it if needed
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(saveFileDialog1.FileName) + DeviceDetails.Trim());
                //Saves the entire myscan40 folder to the above folder
                runCommand(Commands["PullFolder"] + Path.GetDirectoryName(saveFileDialog1.FileName) + DeviceDetails.Trim());
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

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
            //System.Diagnostics.Process.Start(Path.Combine(Environment.CurrentDirectory, "CloseCurrentApp.bat"));
            startProcess("CloseCurrentApp.bat");
            myProcess.Close();

        }

        private void button9_Click(object sender, EventArgs e)
        {

            startProcess("DeviceDetails.bat");
            String DeviceDetails = myProcess.StandardOutput.ReadToEnd();
            myProcess.Close();
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

        private void button11_Click(object sender, EventArgs e)
        {
            startProcess("DeviceDetails.bat");
            String DeviceDetails = myProcess.StandardOutput.ReadToEnd();
            myProcess.Close();

            //https://www.c-sharpcorner.com/UploadFile/mahesh/savefiledialog-in-C-Sharp/
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @Environment.CurrentDirectory;
            saveFileDialog1.Title = "Save screenshot";
            //saveFileDialog1.CheckFileExists = true;
            //saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "png";
            saveFileDialog1.Filter = "PNG (*.png)|*.png|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = DeviceDetails.Trim() + "_";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                //https://stackoverflow.com/questions/27766712/using-adb-to-capture-the-screen
                runCommand(Commands["ScreenCap"] + saveFileDialog1.FileName + ".png");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SaveMyScan40Folder = checkBox1.Checked;
        }
    }
}
