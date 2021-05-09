using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MDC_Android_Tool
{
    public partial class LogsControl : UserControl
    {
        public LogsControl()
        {
            InitializeComponent();
        }

        private void RunCommand(String command)
        {
            System.Diagnostics.Process.Start("CMD.exe", command);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            RunCommand("/C adb logcat -c & adb logcat -c & EXIT");
            MessageBox.Show("Cleared!", "Clear logcat");
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("File saved to ....", "Save logcat");
            //System.Diagnostics.Process.Start("CMD.exe", "adb logcat -d > logcat.log");

        }
    }
}
