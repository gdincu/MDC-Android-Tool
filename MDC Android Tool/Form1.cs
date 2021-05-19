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
using System.Data.SqlClient;
using SharpAdbClient;

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
        Process _myProcess = new Process();
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

            AdbServer server = new AdbServer();
            var result = server.StartServer(@"C:\Users\i.g.dincu\Downloads\platform-tools_r30.0.4-windows\platform-tools\adb.exe", restartServerIfNewer: false);

            // CheckHowManyDevicesAreCurrentlyConnected            
            int nrOfDevices = await NumberOfDevicesConnected();

            while (nrOfDevices != 1)
            {

                if (nrOfDevices == 0)
                    if (MessageBox.Show("Please connect a device to continue!", "Connect a device!", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        Environment.Exit(0);

                if (nrOfDevices > 1)
                    if (MessageBox.Show("Please ensure that only one device is connected!", "Check number of connected devices!", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        Environment.Exit(0);

                if (nrOfDevices == 1)
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

            //List<XElement> XMLSettings = XElement.Load(Settings).Elements(TagName).ToList();
            //XMLSettings.ForEach(x => 
            //    x.Elements(TagName[..^1]).ToList().ForEach(y => 
            //        Dictionary.Add(y.Attribute("name").Value, y.Value)));
        }

        private async Task<String> scanItem(String ItemBarcode)
        {
            return Commands["ScanItem1"] + ItemBarcode + Commands["ScanItem2"];
        }

        //Used to run cmd commands
        private void RunCommand(String command)
        {
            var device = AdbClient.Instance.GetDevices().First();
            var receiver = new ConsoleOutputReceiver();
            AdbClient.Instance.ExecuteRemoteCommand(command, device, receiver);
        }

        //Used to run cmd commands
        private async Task<String> GetOutputFromCommand(String command)
        {
            var device = AdbClient.Instance.GetDevices().First();
            var receiver = new ConsoleOutputReceiver();
            AdbClient.Instance.ExecuteRemoteCommand(command, device, receiver);
            return receiver.ToString();
        }

        //Used to run an external cmd script
        private void StartProcess(String filename)
        {
            _myProcess.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = Path.Combine(Environment.CurrentDirectory, filename),
                
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            _myProcess.Start();
        }

        //Returns the number of connected devices
        private async Task<int> NumberOfDevicesConnected()
        {
            return AdbClient.Instance.GetDevices().Count();
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            if (Items.ContainsKey(listBox1.SelectedItem.ToString()))
                RunCommand(scanItem(Items[listBox1.SelectedItem.ToString()]).Result);
            else
                MessageBox.Show("Item not found!");

        }

        private async void button3_Click(object sender, EventArgs e)
        {
             RunCommand(Commands["Undocked"]);
            
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            RunCommand(Commands["ClearLogcat"]);
            MessageBox.Show("Cleared!", "Clear logcat");
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            StartProcess("DeviceDetails.bat");
            String DeviceDetails = _myProcess.StandardOutput.ReadToEnd();
            _myProcess.Close();

            //https://www.c-sharpcorner.com/UploadFile/mahesh/savefiledialog-in-C-Sharp/
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                InitialDirectory = @Environment.CurrentDirectory,
                Title = "Save logcat",
                //saveFileDialog1.CheckFileExists = true;
                //saveFileDialog1.CheckPathExists = true;
                DefaultExt = "log",
                Filter = "Logcat (*.log)|*.log|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
                FileName = DeviceDetails.Trim() + "_"
            };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                System.Diagnostics.Process.Start("CMD.exe", @$"/C adb logcat -d > ""{saveFileDialog1.FileName}"" ");

            //Checks whether the device details include any of the handheld names recorded in the MDCAndroidTool.xml file
            //Also checks whether the user ticked the SaveMyScan40Folder checkbox
            //Downloads the entire /sdcard/mdc/myscan40 folder to the specified path and renames it using the device details
            if (HandheldDevices.Any(y => DeviceDetails.Contains(y.Key)) && SaveMyScan40Folder) { 
                //Checks whether the folder already exists and creates it if needed
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(saveFileDialog1.FileName) + DeviceDetails.Trim());
                //Saves the entire myscan40 folder to the above folder
                 RunCommand(Commands["PullFolder"] + Path.GetDirectoryName(saveFileDialog1.FileName) + DeviceDetails.Trim());
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
             RunCommand(Commands["Reboot"]);
        }

        private async void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Do asynchronous work.
            await Task.Delay(100);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
             RunCommand(scanItem(EOTBarcodes[listBox2.SelectedItem.ToString()]).Result);
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

             RunCommand(_intent);
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(Path.Combine(Environment.CurrentDirectory, "CloseCurrentApp.bat"));
            var resultTemp = GetOutputFromCommand(@"dumpsys activity recents | find ""Recent #0""");
            MessageBox.Show(resultTemp.Result);
            //StartProcess("CloseCurrentApp.bat");
            //_myProcess.Close();

        }

        private async void button9_Click(object sender, EventArgs e)
        {
            
            String DeviceDetails = AdbClient.Instance.GetDevices().First().Model + "_Android_" + await GetOutputFromCommand(Commands["AndroidVersion"]);
            MessageBox.Show(DeviceDetails, "Paste these details where needed!");
            Clipboard.SetText(DeviceDetails);

        }

        private async void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Do asynchronous work.
            await Task.Delay(100);
        }

        private async void button10_Click(object sender, EventArgs e)
        {
             RunCommand (Commands[listBox4.SelectedItem.ToString()]);
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            String DeviceDetails = AdbClient.Instance.GetDevices().First().Model + "_Android_" + await GetOutputFromCommand(Commands["AndroidVersion"]);

            //https://www.c-sharpcorner.com/UploadFile/mahesh/savefiledialog-in-C-Sharp/
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                InitialDirectory = @Environment.CurrentDirectory,
                Title = "Save screenshot",
                //saveFileDialog1.CheckFileExists = true;
                //saveFileDialog1.CheckPathExists = true;
                DefaultExt = "png",
                Filter = "PNG (*.png)|*.png|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
                FileName = DeviceDetails.Trim() + "_"
            };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                //https://stackoverflow.com/questions/27766712/using-adb-to-capture-the-screen
                 RunCommand (Commands["ScreenCap"] + saveFileDialog1.FileName + ".png");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SaveMyScan40Folder = checkBox1.Checked;
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            //Close the current app
            StartProcess ("CloseCurrentApp.bat");
            _myProcess.Close();

             RunCommand (Commands["GetCurrentApp"]);
            String AppDetails = _myProcess.StandardOutput.ReadToEnd();
            //https://www.c-sharpcorner.com/article/c-sharp-regex-examples/
            //Might need replacing
            string PackageName = Regex.Replace(AppDetails.Split(" ")[7], "\\=+", " ").Split(" ")[1];
            _myProcess.Close();
            //Wait 2 seconds
            System.Threading.Thread.Sleep(2000);
            //The below is a short version of adb shell monkey -p com.package.name -c android.intent.category.LAUNCHER 1
             RunCommand (Commands["StartApp"] + PackageName + " 1");
            _myProcess.Close();

        }

        private async void button13_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Is the device connected via USB?", "Device connected?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //Tcpip 5555 command
                 RunCommand (Commands["Tcpip"]);
                _myProcess.Close();

                //Wait 1 second -- needs to be here as otherwise the StandardOutput.ReadToEnd() functionality does not seem to work
                System.Threading.Thread.Sleep(1000);

                //IP route command
                 RunCommand (Commands["Ip"]);
                string[] tempResult = _myProcess.StandardOutput.ReadToEnd().Split(" ");
                string IpAddress = (tempResult.Length >= 11) ? tempResult[11] : tempResult.ToString();
                MessageBox.Show(IpAddress);
                _myProcess.Close();

                if (MessageBox.Show("Please disconnect the device from the USB port and tap OK when ready!", "Disconnect the device!", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                    //Disconnects all devices
                     RunCommand (Commands["Disconnect"]);
                    _myProcess.Close();
                    
                    //Wait 1 second 
                    System.Threading.Thread.Sleep(1000);

                    //Connects over IP to the selected device
                     RunCommand (Commands["Connect"] + IpAddress);
                    MessageBox.Show("Connected to "+ IpAddress);
                    _myProcess.Close();
                }
            }

            
        }
    }
}
