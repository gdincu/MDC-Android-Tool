using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            FakeIntensiveOperation();

            button1.Enabled = true;

        }

        private async void FakeIntensiveOperation()
        {
            await Task.Run(() => 
            { 
                long sum = 0;
                for (int i = 0; i < int.MaxValue; i++)
                {
                    sum += i;
                }
            });

            MessageBox.Show("Operatia s-a terminat");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AdbServer server = new AdbServer();
            var result = server.StartServer(@"C:\Users\i.g.dincu\Downloads\platform-tools_r30.0.4-windows\platform-tools\adb.exe", restartServerIfNewer: false);

            var device = AdbClient.Instance.GetDevices().First();
            var receiver = new ConsoleOutputReceiver();

            AdbClient.Instance.ExecuteRemoteCommand("ip route", device, receiver);

            MessageBox.Show(receiver.ToString());
        }
    }
}
