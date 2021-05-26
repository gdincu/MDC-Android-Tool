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

        private void ReadValues (IDictionary<string, string> Dictionary, string TagName)
        {

            //https://docs.microsoft.com/en-us/dotnet/standard/linq/retrieve-value-element
            foreach (XElement level1Element in XElement.Load(Settings).Elements(TagName))
                foreach (XElement level2Element in level1Element.Elements(TagName[..^1]))
                    Dictionary.Add(level2Element.Attribute("name").Value, level2Element.Value);

        }

        private async Task<String> ReturnScanItemCommand(String ItemBarcode)
        {
            return NumberOfDevicesConnected().Result.Equals(1) ? Commands["ScanItem1"] + ItemBarcode + Commands["ScanItem2"] : "";
        }

        //Used to run cmd commands
        private async void RunCommand(String command)
        {
            if(NumberOfDevicesConnected().Result.Equals(1)) { 
                var device = AdbClient.Instance.GetDevices().First();
                AdbClient.Instance.ExecuteRemoteCommand(command, device, null);
            }
        }

        //Used to run cmd commands and retrieve output
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

        //Downloads an entire folder
        void DownloadFile(string devicePath, string localPath)
        {
            var device = AdbClient.Instance.GetDevices().First();

            using (SyncService service = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device))
            using (Stream stream = File.OpenWrite(localPath))
            {
                service.Pull(devicePath, stream, null, CancellationToken.None);
            }
        }

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
            String DeviceDetails = AdbClient.Instance.GetDevices().First().Model + "_Android_" + await GetOutputFromCommand(Commands["AndroidVersion"]);
            DeviceDetails = DeviceDetails.Trim();

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
                FileName = DeviceDetails + "_"
            };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                await File.WriteAllTextAsync(saveFileDialog1.FileName, await GetOutputFromCommand("logcat -d"));
           
            //System.Diagnostics.Process.Start("CMD.exe", @$"/C adb logcat -d > ""{saveFileDialog1.FileName}"" ");

            /*
             * Checks whether the device details include any of the handheld names recorded in the MDCAndroidTool.xml file
             * Also checks whether the user ticked the SaveMyScan40Folder checkbox
             * Downloads the entire /sdcard/mdc/myscan40 folder to the specified path and renames it using the device details
             */
            if (HandheldDevices.Any(y => DeviceDetails.Contains(y.Key)) && SaveMyScan40Folder) {
                //Builds a directory path based on the dialog box selection & the device details
                string tempDirectoryPath = Path.Combine(Path.GetDirectoryName(saveFileDialog1.FileName), DeviceDetails);
                //Checks whether the folder already exists and creates it if needed
                System.IO.Directory.CreateDirectory(tempDirectoryPath);
                //Saves the entire myscan40 folder to the above folder
                RunExternalCMDCommand(Commands["PullFolder"] + tempDirectoryPath);


                //To be discussed
                //Saves the entire myscan40 folder to the above folder - to be updated - only saves one file
                //DownloadFolder("/sdcard/mdc/myscan40/filelog.log", tempDirectoryPath + @"\filelog.log");

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
            String _intent = Commands["Intent1"]
                + textBox2.Text
                + Commands["Intent2"]
                + URIs[listBox3.SelectedItem.ToString()]
                + Commands["Intent3"]
                + textBox3.Text+ "'";

            if (NumberOfDevicesConnected().Result.Equals(1))
                RunCommand(_intent);
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            
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

             RunCommand(Commands[listBox4.SelectedItem.ToString()]);

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
                RunExternalCMDCommand(Commands["ScreenCap"] + saveFileDialog1.FileName);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SaveMyScan40Folder = checkBox1.Checked;
        }

        private async void button12_Click(object sender, EventArgs e)
        {

            string currentApp = await CurrentPackageName();
            RunCommand("am force-stop " + currentApp);
            RunCommand(Commands["StartApp"] + currentApp + " 1");

        }

        private async void button13_Click(object sender, EventArgs e)
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

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
       (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                   (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            RunExternalCMDCommand(textBox1.Text);
        }

        private void button15_Click(object sender, EventArgs e)
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

            if (MessageBox.Show("Click to stop recording!", "Recording...", MessageBoxButtons.OK) == DialogResult.OK) {
                //Stops recording
                cancellationTokenSource.Cancel();
                //Pulls the video from the device
                //RunExternalCMDCommand("adb pull/sdcard/video.mp4");

                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    InitialDirectory = @Environment.CurrentDirectory,
                    Title = "Save recording",
                    //saveFileDialog1.CheckFileExists = true;
                    //saveFileDialog1.CheckPathExists = true;
                    DefaultExt = "mp4",
                    Filter = "Logcat (*.mp4)|*.mp4|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true,
                    FileName = "video"
                };
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    DownloadFile("/sdcard/video.mp4", saveFileDialog1.FileName);

                //Removes the video on the device
                RunCommand("rm -f /sdcard/video.mp4");
            }

        }
    }
}
