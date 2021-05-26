using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using SharpAdbClient;
using System.Net;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        //Used to store values from the XML settings file
        IDictionary<string, string> Items = new Dictionary<string, string>();
        IDictionary<string, string> Commands = new Dictionary<string, string>();
        IDictionary<string, string> EOTBarcodes = new Dictionary<string, string>();
        IDictionary<string, string> URIs = new Dictionary<string, string>();
        IDictionary<string, string> HandheldDevices = new Dictionary<string, string>();
        //Path to the settings file
        static String Settings = Path.Combine(Environment.CurrentDirectory, "MDCAndroidTool.xml");
        //Extracts the ADB path
        string ADBPath = XElement.Load(Settings).Element("ADBPath").Value;
        //Used to see whether the handheld filelogs are to be saved
        bool SaveMyScan40Folder = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Retrieve commands from the settings.xml file
            ReadValues(Commands, "Commands");

            //Starts the AdbServer
            AdbServer server = new AdbServer();
            var result = server.StartServer(ADBPath, restartServerIfNewer: false);

            // CheckHowManyDevicesAreCurrentlyConnected            
            int nrOfDevices = NumberOfDevicesConnected().Result;

            while (nrOfDevices != 1)
            {
                if (nrOfDevices == 0)
                    if (MessageBox.Show("Please connect a device to continue!", "Connect a device!", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        Environment.Exit(0);

                if (nrOfDevices > 1)
                    if (MessageBox.Show("Please ensure that only one device is connected!", "Check number of connected devices!", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        Environment.Exit(0);

                nrOfDevices = NumberOfDevicesConnected().Result;
            }

            ReadValues(Items, "Items");
            ReadValues(EOTBarcodes, "EOTBarcodes");
            ReadValues(URIs, "URIs");
            ReadValues(HandheldDevices, "HandheldDevices");

            Items.ToList().ForEach(x => this.listBox1.Items.Add(x.Key));
            EOTBarcodes.ToList().ForEach(x => this.listBox2.Items.Add(x.Key));
            URIs.ToList().ForEach(x => this.listBox3.Items.Add(x.Key));
            Commands.ToList().ForEach(x => this.listBox4.Items.Add(x.Key));
        }

        //Used to read data from the Settings xml file
        private void ReadValues (IDictionary<string, string> Dictionary, string TagName)
        {

            //https://docs.microsoft.com/en-us/dotnet/standard/linq/retrieve-value-element
            foreach (XElement level1Element in XElement.Load(Settings).Elements(TagName))
                foreach (XElement level2Element in level1Element.Elements(TagName[..^1]))
                    Dictionary.Add(level2Element.Attribute("name").Value, level2Element.Value);

        }

        //Used to return the scan item command
        private async Task<String> ReturnScanItemCommand(String ItemBarcode)
        {
            //Checks whether there is only one device connected and if that is the case returns the scan item command
            return NumberOfDevicesConnected().Result.Equals(1) ? Commands["ScanItem1"] + ItemBarcode + Commands["ScanItem2"] : "";
        }

        //Used to run cmd commands (using the AdbClient)
        private async void RunCommand(String command)
        {
            if(NumberOfDevicesConnected().Result.Equals(1)) { 
                var device = AdbClient.Instance.GetDevices().First();
                AdbClient.Instance.ExecuteRemoteCommand(command, device, null);
            }
        }

        //Used to run cmd commands and retrieve output (using the AdbClient)
        private async Task<String> GetOutputFromCommand(String command)
        {
            var device = AdbClient.Instance.GetDevices().First();
            var receiver = new ConsoleOutputReceiver();
            if (NumberOfDevicesConnected().Result.Equals(1))
                AdbClient.Instance.ExecuteRemoteCommand(command, device, receiver);
            return receiver.ToString();
        }

        //Returns the number of connected devices
        private async Task<int> NumberOfDevicesConnected()
        {
            return AdbClient.Instance.GetDevices().Count();
        }

        //Returns the package name for the current app
        private async Task<string> CurrentPackageName()
        {
            string resultTemp = "";
            if (NumberOfDevicesConnected().Result.Equals(1)) { 
            resultTemp = GetOutputFromCommand("dumpsys activity recents").Result;
            resultTemp = resultTemp.Substring(resultTemp.IndexOf('#'));
            }
            return NumberOfDevicesConnected().Result.Equals(1) ? resultTemp.Substring(0, resultTemp.IndexOf('\r')).Split(" ")[3].Substring(2) : "";
        }

        //Downloads a file from the device (using the AdbClient)
        void DownloadFile(string devicePath, string localPath)
        {
            var device = AdbClient.Instance.GetDevices().First();

            using (SyncService service = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device))
            using (Stream stream = File.OpenWrite(localPath))
            {
                service.Pull(devicePath, stream, null, CancellationToken.None);
            }
        }

        //Returns the selected filename
        private string SaveFileDialogFilename(string Title, string DefaultExt,string Filter,string FileName) {

            //https://www.c-sharpcorner.com/UploadFile/mahesh/savefiledialog-in-C-Sharp/
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                InitialDirectory = @Environment.CurrentDirectory,
                Title = Title,
                DefaultExt = DefaultExt,
                Filter = Filter,
                FilterIndex = 1,
                RestoreDirectory = true,
                FileName = FileName
            };

            return saveFileDialog1.ShowDialog() == DialogResult.OK ? saveFileDialog1.FileName : "";
        }

        //Runs an external command without using AdbClient (eg. adb disconnect while all adb shell ... commands would use the RunCommand method)
        private void RunExternalCMDCommand(String command)
        {
            //https://stackoverflow.com/questions/19257041/run-cmd-command-without-displaying-it
            Process _myProcess = new Process();
            _myProcess.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + command,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            if (NumberOfDevicesConnected().Result.Equals(1)) { 
                _myProcess.Start();
                _myProcess.WaitForExit();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnected().Result.Equals(1))
                RunCommand(ReturnScanItemCommand(Items[listBox1.SelectedItem.ToString()]).Result);

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnected().Result.Equals(1))
                RunCommand(Commands["Undocked"]);
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnected().Result.Equals(1)) { 
                RunCommand(Commands["ClearLogcat"]);
                MessageBox.Show("Cleared!", "Clear logcat");
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnected().Result.Equals(1))
            {
                String DeviceDetails = AdbClient.Instance.GetDevices().First().Model + "_Android_" + await GetOutputFromCommand(Commands["AndroidVersion"]);
                DeviceDetails = DeviceDetails.Trim();

                string tempFilename = SaveFileDialogFilename("Save logcat", "log", "Logcat (*.log)|*.log|All files (*.*)|*.*", DeviceDetails + "_");
                await File.WriteAllTextAsync(tempFilename, await GetOutputFromCommand("logcat -d"));
                //System.Diagnostics.Process.Start("CMD.exe", @$"/C adb logcat -d > ""{saveFileDialog1.FileName}"" ");

                /*
                 * Checks whether the device details include any of the handheld names recorded in the MDCAndroidTool.xml file
                 * Also checks whether the user ticked the SaveMyScan40Folder checkbox
                 * Downloads the entire /sdcard/mdc/myscan40 folder to the specified path and renames it using the device details
                 */
                if (HandheldDevices.Any(y => DeviceDetails.Contains(y.Key)) && SaveMyScan40Folder)
                {
                    //Builds a directory path based on the dialog box selection & the device details
                    string tempDirectoryPath = Path.Combine(Path.GetDirectoryName(tempFilename), DeviceDetails);
                    //Checks whether the folder already exists and creates it if needed
                    System.IO.Directory.CreateDirectory(tempDirectoryPath);
                    //Saves the entire myscan40 folder to the above folder
                    RunExternalCMDCommand(Commands["PullFolder"] + tempDirectoryPath);


                    //To be discussed
                    //Would need to only include filelog files
                }
            }

        }

        private async void groupBox1_Enter(object sender, EventArgs e)
        {
            
        }

        private async void tabPage1_Click(object sender, EventArgs e)
        {
            
        }

        private async void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnected().Result.Equals(1))
                RunCommand(Commands["Reboot"]);
        }

        private async void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnected().Result.Equals(1))
                RunCommand(ReturnScanItemCommand(EOTBarcodes[listBox2.SelectedItem.ToString()]).Result);
        }

        private async void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private async void button7_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnected().Result.Equals(1)) { 

                String _intent = Commands["Intent1"]
                + textBox2.Text
                + Commands["Intent2"]
                + URIs[listBox3.SelectedItem.ToString()]
                + Commands["Intent3"]
                + textBox3.Text+ "'";
            
                RunCommand(_intent);
            }
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnected().Result.Equals(1))
                RunCommand("am force-stop " + await CurrentPackageName());

        }

        private async void button9_Click(object sender, EventArgs e)
        {
            if(NumberOfDevicesConnected().Result.Equals(1)) { 
                String DeviceDetails = AdbClient.Instance.GetDevices().First().Model + "_Android_" + await GetOutputFromCommand(Commands["AndroidVersion"]);
                MessageBox.Show(DeviceDetails, "Paste these details where needed!");
                Clipboard.SetText(DeviceDetails);
            }

        }

        private async void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
       
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnected().Result.Equals(1))
                RunCommand(Commands[listBox4.SelectedItem.ToString()]);

        }

        private async void button11_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnected().Result.Equals(1))
            {
                //https://stackoverflow.com/questions/27766712/using-adb-to-capture-the-screen
                String DeviceDetails = AdbClient.Instance.GetDevices().First().Model + "_Android_" + await GetOutputFromCommand(Commands["AndroidVersion"]);
                string tempFilename = SaveFileDialogFilename("Save screenshot", "png", "PNG (*.png)|*.png|All files (*.*)|*.*", DeviceDetails.Trim() + "_");
                RunExternalCMDCommand(Commands["ScreenCap"] + tempFilename);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SaveMyScan40Folder = checkBox1.Checked;
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnected().Result.Equals(1))
            {
                string currentApp = await CurrentPackageName();
                RunCommand("am force-stop " + currentApp);
                RunCommand(Commands["StartApp"] + currentApp + " 1");
            }

        }

        private async void button13_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnected().Result.Equals(1))
            {
                if (MessageBox.Show("Is the device connected via USB?", "Device connected?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //Tcpip 5555 command
                    RunExternalCMDCommand(Commands["Tcpip"]);

                    //Wait 1 second -- needs to be here as otherwise the StandardOutput.ReadToEnd() functionality does not seem to work
                    System.Threading.Thread.Sleep(1000);

                    //IP route command
                    string[] tempResult = GetOutputFromCommand(Commands["Ip"]).Result.Split(" ");
                    string IpAddress = (tempResult.Length >= 11) ? tempResult[11].Trim() : tempResult.ToString();
                    MessageBox.Show(IpAddress);

                    if (MessageBox.Show("Please disconnect the device from the USB port and tap OK when ready!", "Disconnect the device!", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        //Disconnects all devices
                        RunExternalCMDCommand(Commands["Disconnect"]);

                        //Wait 1 second 
                        System.Threading.Thread.Sleep(1000);

                        //Connects over IP to the selected device
                        RunExternalCMDCommand(Commands["Connect"] + IpAddress);
                        MessageBox.Show("Connected to " + IpAddress);
                    }
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (
                !char.IsControl(e.KeyChar) && 
                !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.')
                )
                e.Handled = true;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (
                !char.IsControl(e.KeyChar) && 
                !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.')
                )
                e.Handled = true;
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnected().Result.Equals(1))
                RunExternalCMDCommand(textBox1.Text);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnected().Result.Equals(1))
            {
                //Removes any previous video from the set path on the device
                RunCommand("rm -f /sdcard/video.mp4");

                var device = AdbClient.Instance.GetDevices().First();
                var cancellationTokenSource = new CancellationTokenSource();
                var receiver = new ConsoleOutputReceiver();
                var task = AdbClient.Instance.ExecuteRemoteCommandAsync(Commands["RecordScreen"], device, receiver, cancellationTokenSource.Token, int.MaxValue);

                // Your code is now executing. You can now:
                // - Read the output using the receiver
                // - Cancel the task like shown below:

                if (MessageBox.Show("Click to stop recording!", "Recording...", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    //Stops recording
                    cancellationTokenSource.Cancel();
                    //Pulls the video from the device
                    string tempFilename = SaveFileDialogFilename("Save recording", "mp4", "Video (*.mp4)|*.mp4|All files (*.*)|*.*", "video");
                    DownloadFile("/sdcard/video.mp4", tempFilename);
                    //Removes the video on the device
                    RunCommand("rm -f /sdcard/video.mp4");
                }
            }

        }
    }
}
