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
using SharpAdbClient.DeviceCommands;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        //Used to store values from the XML settings file
        readonly IDictionary<string, string> Items = new Dictionary<string, string>();
        readonly IDictionary<string, string> Commands = new Dictionary<string, string>();
        readonly IDictionary<string, string> EOTBarcodes = new Dictionary<string, string>();
        readonly IDictionary<string, string> URIs = new Dictionary<string, string>();
        readonly IDictionary<string, string> HandheldDevices = new Dictionary<string, string>();
        //Path to the settings file
        readonly string Settings = Path.Combine(Environment.CurrentDirectory, "MDCAndroidTool.xml");
        //Used to see whether the handheld filelogs are to be saved
        bool SaveMyScan40Folder;
        //Used to store the list of currently installed apps
        List<string> listOfCurrentlyInstalledApps;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Retrieve commands from the settings.xml file
            ReadValuesAsync(Commands, "Commands");

            //Starts the AdbServer
            AdbServer server = new AdbServer();
            //Extracts the ADB path from the settings file
            string ADBPath = XElement.Load(Settings).Element("ADBPath").Value;
            server.StartServer(ADBPath, restartServerIfNewer: false);

            // ChecksHowManyDevicesAreCurrentlyConnected            
            while (!NumberOfDevicesConnectedEqualsOne())
            {
                if (ReturnNumberOfDevicesConnected().Equals(0))
                    if (MessageBox.Show("Please connect a device to continue!", "Connect a device!", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        Environment.Exit(0);

                if (ReturnNumberOfDevicesConnected() > 1)
                    if (MessageBox.Show("Please ensure that only one device is connected!", "Check number of connected devices!", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        Environment.Exit(0);
            }

            ReadValuesAsync(Items, "Items");
            ReadValuesAsync(EOTBarcodes, "EOTBarcodes");
            ReadValuesAsync(URIs, "URIs");
            ReadValuesAsync(HandheldDevices, "HandheldDevices");

            PopulateListBox(listBox1, Items);             
            PopulateListBox(listBox2, EOTBarcodes);
            PopulateListBox(listBox3, URIs);
            PopulateListBox(listBox4, Commands);
            PopulateListBox(listBox5, ReturnListOfInstalledApps());
            
        }

        //Used to read data from the Settings xml file
        private async void ReadValuesAsync (IDictionary<string, string> Dictionary, string TagName)
        {
            await Task.Run(() =>
            {
                //https://docs.microsoft.com/en-us/dotnet/standard/linq/retrieve-value-element
                foreach (XElement level1Element in XElement.Load(Settings).Elements(TagName))
                    foreach (XElement level2Element in level1Element.Elements(TagName[..^1]))
                        Dictionary.Add(level2Element.Attribute("name").Value, level2Element.Value);
            });

        }

        //Used to populate listbox values based on a dictionary
        private void PopulateListBox(ListBox listbox, IDictionary<string, string> dictionary)
        {
            foreach (KeyValuePair<string, string> x in dictionary)
                listbox.Items.Add(x.Key);
        }   

        //Returns a list of all currently installed apps
        private Dictionary<string, string> ReturnListOfInstalledApps()
        {            
            DeviceData device = AdbClient.Instance.GetDevices().First();
            PackageManager manager = new PackageManager(device);
            return manager.Packages;
        }   

        //Used to return the scan item command
        private string ReturnScanItemCommand(String ItemBarcode)
        {
            //Checks whether there is only one device connected and if that is the case returns the scan item command
            return ReturnNumberOfDevicesConnected().Equals(1) ? Commands["ScanItem1"] + ItemBarcode + Commands["ScanItem2"] : "";
        }

        //Used to run cmd commands (using the AdbClient)
        private void RunCommand(String command)
        {
            if(NumberOfDevicesConnectedEqualsOne()) { 
                var device = AdbClient.Instance.GetDevices().First();
                AdbClient.Instance.ExecuteRemoteCommand(command, device, null);
            }
        }

        //Used to run cmd commands and retrieve output (using the AdbClient)
        private string GetOutputFromCommand(String command)
        {
            var device = AdbClient.Instance.GetDevices().First();
            var receiver = new ConsoleOutputReceiver();
            
            if (NumberOfDevicesConnectedEqualsOne())
                AdbClient.Instance.ExecuteRemoteCommand(command, device, receiver);

            return receiver.ToString();
        }

        //Returns the number of connected devices
        private int ReturnNumberOfDevicesConnected()
        {
            return AdbClient.Instance.GetDevices().Count();
        }

        //Returns true when the number of connected devices equals one
        private bool NumberOfDevicesConnectedEqualsOne() {
            return ReturnNumberOfDevicesConnected().Equals(1);
        }

        //Returns the package name for the current app
        private string CurrentPackageName()
        {
            string resultTemp = "";

            if (NumberOfDevicesConnectedEqualsOne()) { 
                resultTemp = GetOutputFromCommand("dumpsys activity recents");
            resultTemp = resultTemp.Substring(resultTemp.IndexOf('#'));
            }

            return ReturnNumberOfDevicesConnected().Equals(1) ? resultTemp.Substring(0, resultTemp.IndexOf('\r')).Split(" ")[3].Substring(2) : "";
        }

        //Downloads a file from the device (using the AdbClient)
        void DownloadFile(string devicePath, string localPath)
        {
            var device = AdbClient.Instance.GetDevices().First();

            using SyncService service = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device);
            using Stream stream = File.OpenWrite(localPath);
            service.Pull(devicePath, stream, null, CancellationToken.None);
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
        private int RunExternalCMDCommand(String command)
        {
            //Assumes that something goes wrong - 0 = Success / 1 = Error
            int result = 1;

            //https://stackoverflow.com/questions/19257041/run-cmd-command-without-displaying-it
            Process _myProcess = new Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = "/C " + command,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };

            if (NumberOfDevicesConnectedEqualsOne())
            {
                _myProcess.Start();
                _myProcess.WaitForExit();
                result = _myProcess.ExitCode;
            }

            return result;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnectedEqualsOne())
                RunCommand(ReturnScanItemCommand(Items[listBox1.SelectedItem.ToString()]));

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnectedEqualsOne())
                RunCommand(Commands["Undocked"]);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnectedEqualsOne())
            { 
                RunCommand(Commands["ClearLogcat"]);
                MessageBox.Show("Cleared!", "Clear logcat");
            }
        }

        private async void Button5_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnectedEqualsOne())
            {
                String DeviceDetails = AdbClient.Instance.GetDevices().First().Model + "_Android_" + GetOutputFromCommand(Commands["AndroidVersion"]);
                DeviceDetails = DeviceDetails.Trim();

                string tempFilename = SaveFileDialogFilename("Save logcat", "log", "Logcat (*.log)|*.log|All files (*.*)|*.*", DeviceDetails + "_");
                await File.WriteAllTextAsync(tempFilename, GetOutputFromCommand("logcat -d"));
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

        private void GroupBox1_Enter(object sender, EventArgs e)
        {
            
        }

        private void TabPage1_Click(object sender, EventArgs e)
        {
            
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnectedEqualsOne())
                RunCommand(Commands["Reboot"]);
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnectedEqualsOne())
                RunCommand(ReturnScanItemCommand(EOTBarcodes[listBox2.SelectedItem.ToString()]));
        }

        private void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void GroupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnectedEqualsOne())
            { 
                String _intent = Commands["Intent1"]
                + textBox2.Text
                + Commands["Intent2"]
                + URIs[listBox3.SelectedItem.ToString()]
                + Commands["Intent3"]
                + textBox3.Text+ "'";
            
                RunCommand(_intent);
            }
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnectedEqualsOne())
                RunCommand("am force-stop " + CurrentPackageName());

        }

        private void Button9_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnectedEqualsOne())
            { 
                String DeviceDetails = AdbClient.Instance.GetDevices().First().Model + "_Android_" + GetOutputFromCommand(Commands["AndroidVersion"]);
                MessageBox.Show(DeviceDetails, "Paste these details where needed!");
                Clipboard.SetText(DeviceDetails);
            }

        }

        private void ListBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
       
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnectedEqualsOne())
                RunCommand(Commands[listBox4.SelectedItem.ToString()]);

        }

        private void Button11_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnectedEqualsOne())
            {
                //https://stackoverflow.com/questions/27766712/using-adb-to-capture-the-screen
                String DeviceDetails = AdbClient.Instance.GetDevices().First().Model + "_Android_" + GetOutputFromCommand(Commands["AndroidVersion"]);
                string tempFilename = SaveFileDialogFilename("Save screenshot", "png", "PNG (*.png)|*.png|All files (*.*)|*.*", DeviceDetails.Trim() + "_");
                RunExternalCMDCommand(Commands["ScreenCap"] + tempFilename);
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            SaveMyScan40Folder = checkBox1.Checked;
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnectedEqualsOne())
            {
                string currentApp = CurrentPackageName();
                RunCommand("am force-stop " + currentApp);
                RunCommand(Commands["StartApp"] + currentApp + " 1");
            }
        }

        private void Button13_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnectedEqualsOne())
            {
                if (MessageBox.Show("Is the device connected via USB?", "Device connected?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //Tcpip 5555 command
                    RunExternalCMDCommand(Commands["Tcpip"]);

                    //Wait 1 second -- needs to be here as otherwise the StandardOutput.ReadToEnd() functionality does not seem to work
                    System.Threading.Thread.Sleep(1000);

                    //IP route command
                    string[] tempResult = GetOutputFromCommand(Commands["Ip"]).Split(" ");
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

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (
                !char.IsControl(e.KeyChar) && 
                !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.')
                )
                e.Handled = true;
        }

        private void TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (
                !char.IsControl(e.KeyChar) && 
                !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.')
                )
                e.Handled = true;
        }

        private void GroupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button14_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnectedEqualsOne())
                RunExternalCMDCommand(textBox1.Text);
        }

        private void Button15_Click(object sender, EventArgs e)
        {
            if (NumberOfDevicesConnectedEqualsOne())
            {
                //Removes any previous video from the set path on the device
                RunCommand("rm -f /sdcard/video.mp4");

                var device = AdbClient.Instance.GetDevices().First();
                var cancellationTokenSource = new CancellationTokenSource();
                var receiver = new ConsoleOutputReceiver();
                AdbClient.Instance.ExecuteRemoteCommandAsync(Commands["RecordScreen"], device, receiver, cancellationTokenSource.Token, int.MaxValue);

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

        private void button16_Click(object sender, EventArgs e)
        {
            //https://stackoverflow.com/questions/6373645/c-sharp-winforms-how-to-set-main-function-stathreadattribute/6373674
            var thread = new Thread(new ParameterizedThreadStart(param =>
            {
                //Opens a file dialog from where the user can select an apk file
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Title = "Open apk file",
                    Filter = "APK|*.apk",
                    InitialDirectory = @Environment.CurrentDirectory
                };

                //When the user taps on the OK button
                if (ofd.ShowDialog() == DialogResult.OK) { 
                 //Tries to install the app
                    int processExitCode = RunExternalCMDCommand(Commands["Install"] + "\"" + ofd.FileName + "\"");

                    //Based on the System_Diagnostics_Process_ExitCode returned by the process a messagebox is displayed
                    //https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.process.exitcode?redirectedfrom=MSDN&view=net-5.0#System_Diagnostics_Process_ExitCode
                    if (processExitCode == 0)
                        MessageBox.Show("Apk installed successfully!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    else
                        MessageBox.Show("Something went wrong! Please try again!", "Try again!", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                }
        
            }));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {
            //Tries to uninstall the app
            int processExitCode = RunExternalCMDCommand(Commands["Uninstall"] + listBox5.SelectedItem);

            //Based on the System_Diagnostics_Process_ExitCode returned by the process a messagebox is displayed
            if (processExitCode == 0)
                MessageBox.Show("App uninstalled successfully!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            else
                MessageBox.Show(@"Something went wrong! Try uninstalling this manually via the 'Others' tab using the package name!", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            //Reads the current list of values again
            listOfCurrentlyInstalledApps = ReturnListOfInstalledApps().Values.ToList();

            //Clears all items from the listbox
            listBox5.Items.Clear();

            if (textBox4.Text.Length == 0)
                //Adds all items back to the list
                listBox5.Items.AddRange(listOfCurrentlyInstalledApps.ToArray());
            
            else
                //Filters the items and adds them back to the list
                listBox5.Items.AddRange(listOfCurrentlyInstalledApps.Where(i => i.ToLower().Contains(textBox4.Text.ToLower())).ToArray());
        }
    }
}
