using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MDC_Android_Tool
{
    public partial class DeviceControl : UserControl
    {
        private void RunCommand(String command)
        {
            System.Diagnostics.Process.Start("CMD.exe", command);
        }
       
        public DeviceControl()
        {
            InitializeComponent();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            RunCommand("/C adb reboot");
        }

    }
}
