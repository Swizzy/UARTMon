namespace launcher {
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Ports;
    using System.Reflection;
    using System.Windows.Forms;
    using launcher.Properties;

    internal sealed partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
            UpdatePorts();
        }

        internal static byte[] GetBuiltInData(string fileName) {
            if(string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");
            using(var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("{0}.{1}", typeof(MainForm).Namespace, fileName))) {
                if(stream != null) {
                    var data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);
                    return data;
                }
                throw new Exception(string.Format("Can't find {0}!", fileName));
            }
        }

        private void UpdatePorts() {
            
            comport.Items.Clear();
            comport.Items.Add("demon");
            comport.Items.AddRange(SerialPort.GetPortNames());
            comport.SelectedIndex = 0;
        }

        private void Button1Click(object sender, EventArgs e) {
            File.WriteAllBytes("UARTMon.exe", GetBuiltInData("UARTMon.exe"));
            var procinfo = new ProcessStartInfo {
                                                FileName = "UARTMon.exe", Arguments = dolog.Checked ? string.Format("{0} \"{1}\"", comport.Text, logname.Text) : comport.Text
                                                };
            if(lowspeed.Checked)
                procinfo.Arguments += " -lowspeed";
            Process.Start(procinfo);
            Close();
        }

        private void UpdbtnClick(object sender, EventArgs e) {
            UpdatePorts();
        }

        private void Button2Click(object sender, EventArgs e) {
            var sfd = new SaveFileDialog {
                                         FileName = logname.Text, Title = Resources.MainForm_Button2Click_Select_where_to_save_the_log___
                                         };
            if(sfd.ShowDialog() == DialogResult.OK)
                logname.Text = sfd.FileName;
        }
    }
}