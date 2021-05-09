using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MDC_Android_Tool
{
    public partial class TripStartControl : UserControl
    {
        private String ScanItem(String ItemBarcode)
        {
            return "/C adb shell am broadcast -a 'barcodescanner.RECVR' --es 'com.motorolasolutions.emdk.datawedge.data_string' '" + ItemBarcode + "' --es 'com.motorolasolutions.emdk.datawedge.source' 'scanner' --es 'com.motorolasolutions.emdk.datawedge.label_type' 'LABEL - TYPE - EAN13'";
        }

        private void RunCommand(String command)
        {
            System.Diagnostics.Process.Start("CMD.exe", command);
        }
        public TripStartControl()
        {
            InitializeComponent();
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            RunCommand(ScanItem(listBox2.SelectedItem.ToString()));
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            RunCommand("/C adb shell am broadcast -a 'com.symbol.intent.device.UNDOCKED'");
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            String _intent = "/C adb shell am broadcast -a 'com.mdcinternational.selfscanner.sendselfscannerconfig' --es 'storenumber' '"
                + textBox2.Text
                + "' --es 'soapendpoint' '"
                + listBox3.SelectedItem.ToString()
                + "' --es 'terminalid' '"
                + textBox3.Text
                + "'";

            RunCommand(_intent);
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("D:/repos/MDC Android Tool/MDC Android Tool/CloseCurrentApp.bat");
        }

        private void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
