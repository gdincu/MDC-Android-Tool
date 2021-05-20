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
            //var result = server.StartServer(@"E:\platform-tools\adb.exe", restartServerIfNewer: false);

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
            AdbClient.Instance.ExecuteRemoteCommand(command, device, null);
        }

        //Used to run cmd commands
        private async Task<String> GetOutputFromCommand(String command)
        {
            var device = AdbClient.Instance.GetDevices().First();
            var receiver = new ConsoleOutputReceiver();
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
            string resultTemp = GetOutputFromCommand("dumpsys activity recents").Result;
            resultTemp = resultTemp.Substring(resultTemp.IndexOf('#'));
            return resultTemp.Substring(0, resultTemp.IndexOf('\r')).Split(" ")[3].Substring(2);
        }

        //Downloads an entire folder
        void DownloadFolder(string devicePath, string localPath)
        {
            var device = AdbClient.Instance.GetDevices().First();

            using (SyncService service = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device))
            using (Stream stream = File.OpenWrite(localPath))
            {
                service.Pull(devicePath, stream, null, CancellationToken.None);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            
            RunCommand(scanItem(Items[listBox1.SelectedItem.ToString()]).Result);

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

            //Checks whether the device details include any of the handheld names recorded in the MDCAndroidTool.xml file
            //Also checks whether the user ticked the SaveMyScan40Folder checkbox
            //Downloads the entire /sdcard/mdc/myscan40 folder to the specified path and renames it using the device details
            if (HandheldDevices.Any(y => DeviceDetails.Contains(y.Key)) && SaveMyScan40Folder) {
                string tempDirectoryPath = Path.Combine(Path.GetDirectoryName(saveFileDialog1.FileName), DeviceDetails);
                //Checks whether the folder already exists and creates it if needed
                System.IO.Directory.CreateDirectory(tempDirectoryPath);
                //Saves the entire myscan40 folder to the above folder - to be updated - only saves one file
                DownloadFolder("/sdcard/mdc/myscan40/filelog.log", tempDirectoryPath + @"\filelog.log");
                //RunCommand(Commands["PullFolder"] + tempDirectoryPath);
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
            
            RunCommand("am force-stop " + await CurrentPackageName());

        }

        private async void button9_Click(object sender, EventArgs e)
        {
            
            String DeviceDetails = AdbClient.Instance.GetDevices().First().Model + "_Android_" + await GetOutputFromCommand(Commands["AndroidVersion"]);
            MessageBox.Show(DeviceDetails, "Paste these details where needed!");
            Clipboard.SetText(DeviceDetails);

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
                Clipboard.SetText(Commands["ScreenCap"] + @" > " + saveFileDialog1.FileName);
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
                 RunCommand(Commands["Tcpip"]);

                //Wait 1 second -- needs to be here as otherwise the StandardOutput.ReadToEnd() functionality does not seem to work
                System.Threading.Thread.Sleep(1000);

                //IP route command
                string[] tempResult = GetOutputFromCommand(Commands["Ip"]).Result.Split(" ");
                string IpAddress = (tempResult.Length >= 26) ? tempResult[26] : tempResult.ToString();
                MessageBox.Show(IpAddress);

                if (MessageBox.Show("Please disconnect the device from the USB port and tap OK when ready!", "Disconnect the device!", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                    //Disconnects all devices
                    System.Diagnostics.Process.Start("CMD.exe", "adb " + Commands["Disconnect"]);
                    
                    //Wait 1 second 
                    System.Threading.Thread.Sleep(1000);

                    //Connects over IP to the selected device
                    System.Diagnostics.Process.Start("CMD.exe", "adb " + Commands["Connect"] + IpAddress);                    
                    MessageBox.Show("Connected to "+ IpAddress);
                }
            }

            
        }
    }
}
