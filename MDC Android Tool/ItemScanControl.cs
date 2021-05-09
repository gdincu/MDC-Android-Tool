using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MDC_Android_Tool
{
    public partial class ItemScanControl : UserControl
    {
        private String ScanItem(String ItemBarcode)
        {
            return "/C adb shell am broadcast -a 'barcodescanner.RECVR' --es 'com.motorolasolutions.emdk.datawedge.data_string' '" + ItemBarcode + "' --es 'com.motorolasolutions.emdk.datawedge.source' 'scanner' --es 'com.motorolasolutions.emdk.datawedge.label_type' 'LABEL - TYPE - EAN13'";
        }

        private void RunCommand(String command)
        {
            System.Diagnostics.Process.Start("CMD.exe", command);
        }

        private readonly IDictionary<string, string> tempList = new Dictionary<string, string>();
        public ItemScanControl()
        {
            InitializeComponent();
            tempList.Clear();
            tempList.Add("Regular_Item", "16000275270");
            tempList.Add("​Promotion_Item", "70847811299");
            tempList.Add("​Age_Restriction_21", "80660957159");
            tempList.Add("LidItemMessage_Item", "7000570550014");
            tempList.Add("​Liquidation_Discount", "8000570550020​");
            tempList.Add("​Forbidden_Item", "8000570550181​");
        }
        private void Button1_Click(object sender, EventArgs e)
        {

            if (tempList.ContainsKey(listBox1.SelectedItem.ToString()))
                RunCommand(ScanItem(tempList[listBox1.SelectedItem.ToString()]));
            else
                MessageBox.Show("Item not found!");

        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            this.itemScanControl3 = new MDC_Android_Tool.ItemScanControl();
            this.SuspendLayout();
            // 
            // itemScanControl3
            // 
            this.itemScanControl3.Location = new System.Drawing.Point(8, 8);
            this.itemScanControl3.Name = "itemScanControl3";
            this.itemScanControl3.Size = new System.Drawing.Size(550, 400);
            this.itemScanControl3.TabIndex = 11;
            this.itemScanControl3.Load += new System.EventHandler(this.itemScanControl3_Load);
            // 
            // ItemScanControl
            // 
            this.Controls.Add(this.itemScanControl3);
            this.Name = "ItemScanControl";
            this.Size = new System.Drawing.Size(550, 400);
            this.ResumeLayout(false);

        }

        private void itemScanControl3_Load(object sender, EventArgs e)
        {

        }
    }
}
