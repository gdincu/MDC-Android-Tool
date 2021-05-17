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
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        IDictionary<string, string> Items = new Dictionary<string, string>();
        IDictionary<string, string> Commands = new Dictionary<string, string>();
        IDictionary<string, string> EOTBarcodes = new Dictionary<string, string>();
        IDictionary<string, string> URIs = new Dictionary<string, string>();
        IDictionary<string, string> HandheldDevices = new Dictionary<string, string>();
        //Path to the settings file
        String Settings = Path.Combine(Environment.CurrentDirectory, "MDCAndroidTool.xml");
        Process myProcess = new Process();
        //Check used to see whether the handheld filelogs are to be saved
        bool SaveMyScan40Folder = false;

        public Form1()
        {
            InitializeComponent();
        }

        //https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming
        private async void Form1_Load(object sender, EventArgs e)
        {
            await ReadValues(Commands, "Commands");

            // CheckHowManyDevicesAreCurrentlyConnected            
            int nrOfDevices = await NumberOfDevicesConnected();

            while (nrOfDevices != 3)
            {

                if (nrOfDevices <= 2)
                    if (MessageBox.Show("Please connect a device to continue!", "Connect a device!", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        Environment.Exit(0);

                if (nrOfDevices > 3)
                    if (MessageBox.Show("Please ensure that only one device is connected!", "Check number of connected devices!", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        Environment.Exit(0);

                if (nrOfDevices == 3)
                    break;

                //Updates the nrOfDevices value
                nrOfDevices = await NumberOfDevicesConnected();
            }

            ////Workaround - to be investigated
            //RunCommand("");

            await ReadValues(Items, "Items");
            await ReadValues(EOTBarcodes, "EOTBarcodes");
            await ReadValues(URIs, "URIs");
            await ReadValues(HandheldDevices, "HandheldDevices");

            Items.ToList().ForEach(x => this.listBox1.Items.Add(x.Key));
            EOTBarcodes.ToList().ForEach(x => this.listBox2.Items.Add(x.Key));
            URIs.ToList().ForEach(x => this.listBox3.Items.Add(x.Key));
            Commands.ToList().ForEach(x => this.listBox4.Items.Add(x.Key));

        }

        private async Task ReadValues (IDictionary<string, string> Dictionary, string TagName)
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/linq/retrieve-value-element
            foreach (XElement level1Element in XElement.Load(Settings).Elements(TagName))
                foreach (XElement level2Element in level1Element.Elements(TagName[..^1]))
                    Dictionary.Add(level2Element.Attribute("name").Value, level2Element.Value);

            // Do asynchronous work.
            await Task.Delay(100);

            //List<XElement> XMLSettings = XElement.Load(Settings).Elements(TagName).ToList();
            //XMLSettings.ForEach(x => 
            //    x.Elements(TagName[..^1]).ToList().ForEach(y => 
            //        Dictionary.Add(y.Attribute("name").Value, y.Value)));
        }

        private async Task<String> scanItem(String ItemBarcode)
        {
            // Do asynchronous work.
            await Task.Delay(100);
            return Commands["ScanItem1"] + ItemBarcode + Commands["ScanItem2"];
        }

        //Used to run cmd commands
        private async Task RunCommand(String command)
        {
            //https://stackoverflow.com/questions/19257041/run-cmd-command-without-displaying-it
            myProcess.StartInfo.FileName = "CMD.exe";
            myProcess.StartInfo.Arguments = "/C " + command;
            myProcess.StartInfo.RedirectStandardOutput = true;
            myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            myProcess.Start();
            
            // Do asynchronous work.
            await Task.Delay(100);
            //myProcess.WaitForExit();
        }

        //Used to run an external cmd script
        private async Task StartProcess(String filename)
        {
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, filename);
            myProcess.StartInfo.RedirectStandardOutput = true;
            myProcess.Start();

            // Do asynchronous work.
            await Task.Delay(100);
        }

        //Returns the number of connected devices
        private async Task<int> NumberOfDevicesConnected()
        {
            await RunCommand(Commands["NrOfDevices"]);
            int res = Int32.Parse(myProcess.StandardOutput.ReadToEnd());
            myProcess.Close();
            return res;
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            if (Items.ContainsKey(listBox1.SelectedItem.ToString()))
                await RunCommand(scanItem(Items[listBox1.SelectedItem.ToString()]).Result);
            else
                MessageBox.Show("Item not found!");

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            await RunCommand(Commands["Undocked"]);
            
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            await RunCommand(Commands["ClearLogcat"]);
            MessageBox.Show("Cleared!", "Clear logcat");
           
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            await StartProcess("DeviceDetails.bat");
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
                await RunCommand(Commands["PullFolder"] + Path.GetDirectoryName(saveFileDialog1.FileName) + DeviceDetails.Trim());
            }

        }

        private async void groupBox1_Enter(object sender, EventArgs e)
        {
            // Do asynchronous work.
            await Task.Delay(100);
        }

        private async void tabPage1_Click(object sender, EventArgs e)
        {
            // Do asynchronous work.
            await Task.Delay(100);
        }

        private async void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Do asynchronous work.
            await Task.Delay(100);
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            await RunCommand(Commands["Reboot"]);
        }

        private async void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Do asynchronous work.
            await Task.Delay(100);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await RunCommand(scanItem(EOTBarcodes[listBox2.SelectedItem.ToString()]).Result);
        }

        private async void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Do asynchronous work.
            await Task.Delay(100);
        }

        private async void groupBox1_Enter_1(object sender, EventArgs e)
        {
            // Do asynchronous work.
            await Task.Delay(100);
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            String _intent = Commands["Intent1"]
                + textBox2.Text
                + Commands["Intent2"]
                + URIs[listBox3.SelectedItem.ToString()]
                + Commands["Intent3"]
                + textBox3.Text+ "'";

            await RunCommand(_intent);
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(Path.Combine(Environment.CurrentDirectory, "CloseCurrentApp.bat"));
            await StartProcess("CloseCurrentApp.bat");
            myProcess.Close();

        }

        private async void button9_Click(object sender, EventArgs e)
        {
            await StartProcess("DeviceDetails.bat");
            String DeviceDetails = myProcess.StandardOutput.ReadToEnd();
            myProcess.Close();
            MessageBox.Show(DeviceDetails,"Paste these details where needed!");
            Clipboard.SetText(DeviceDetails);
        }

        private async void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Do asynchronous work.
            await Task.Delay(100);
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            await RunCommand (Commands[listBox4.SelectedItem.ToString()]);
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            await StartProcess("DeviceDetails.bat");
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
                await RunCommand (Commands["ScreenCap"] + saveFileDialog1.FileName + ".png");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SaveMyScan40Folder = checkBox1.Checked;
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            //Close the current app
            await StartProcess ("CloseCurrentApp.bat");
            myProcess.Close();

            await RunCommand (Commands["GetCurrentApp"]);
            String AppDetails = myProcess.StandardOutput.ReadToEnd();
            //https://www.c-sharpcorner.com/article/c-sharp-regex-examples/
            //Might need replacing
            string PackageName = Regex.Replace(AppDetails.Split(" ")[7], "\\=+", " ").Split(" ")[1];
            myProcess.Close();
            //Wait 2 seconds
            System.Threading.Thread.Sleep(2000);
            //The below is a short version of adb shell monkey -p com.package.name -c android.intent.category.LAUNCHER 1
            await RunCommand (Commands["StartApp"] + PackageName + " 1");
            myProcess.Close();

        }

        private async void button13_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Is the device connected via USB?", "Device connected?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //Tcpip 5555 command
                await RunCommand (Commands["Tcpip"]);
                myProcess.Close();

                //Wait 1 second -- needs to be here as otherwise the StandardOutput.ReadToEnd() functionality does not seem to work
                System.Threading.Thread.Sleep(1000);

                //IP route command
                await RunCommand (Commands["Ip"]);
                string[] tempResult = myProcess.StandardOutput.ReadToEnd().Split(" ");
                string IpAddress = (tempResult.Length >= 11) ? tempResult[11] : tempResult.ToString();
                MessageBox.Show(IpAddress);
                myProcess.Close();

                if (MessageBox.Show("Please disconnect the device from the USB port and tap OK when ready!", "Disconnect the device!", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                    //Disconnects all devices
                    await RunCommand (Commands["Disconnect"]);
                    myProcess.Close();
                    
                    //Wait 1 second 
                    System.Threading.Thread.Sleep(1000);

                    //Connects over IP to the selected device
                    await RunCommand (Commands["Connect"] + IpAddress);
                    MessageBox.Show("Connected to "+ IpAddress);
                    myProcess.Close();
                }
            }

            
        }
    }
}
