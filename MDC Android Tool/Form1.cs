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
using MDC_Android_Tool;
using System.IO;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        //Dictionaries used to store values for Items, Commands, URIs etc.
        public IDictionary<string, string> Items = new Dictionary<string, string>();
        public IDictionary<string, string> Commands = new Dictionary<string, string>();
        public IDictionary<string, string> EOTBarcodes = new Dictionary<string, string>();
        public IDictionary<string, string> URIs = new Dictionary<string, string>();
        String Settings = Path.Combine(Environment.CurrentDirectory, "MDCAndroidTool.xml");
        public Form1()
        {
            ReadValues(Items, "Items");
            ReadValues(Commands, "Commands");
            ReadValues(EOTBarcodes, "EOTBarcodes");
            ReadValues(URIs, "URIs");
            InitializeComponent();
        }
        private void ReadValues(IDictionary<string, string> Dictionary, string TagName)
        {
            Dictionary.Clear();
            foreach (XElement level1Element in XElement.Load(Settings).Elements(TagName))
                foreach (XElement level2Element in level1Element.Elements(TagName[..^1]))
                    Dictionary.Add(level2Element.Attribute("name").Value, level2Element.Attribute("value").Value);
        }
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox1.Text = "TTT";

            //string anyCommand = "/C adb devices -l | find 'model: '";
            //textBox1.Text += System.Diagnostics.Process.Start("CMD.exe", anyCommand).StandardOutput.ReadToEnd();
        }
        private void GroupBox1_Enter_1(object sender, EventArgs e)
        {

        }
        private void Button9_Click(object sender, EventArgs e)
        {
            itemAddControl1.BringToFront();
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            logsControl1.BringToFront();
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            tripStartControl1.BringToFront();
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            deviceControl1.BringToFront();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
