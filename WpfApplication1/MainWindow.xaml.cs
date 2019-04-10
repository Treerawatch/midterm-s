using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        SerialPort sp = new SerialPort();
        string hexString = string.Empty;
        StringBuilder csv = new StringBuilder();
        string filePath = "D:/TestOutPut.csv";

        bool _isRetFinished = false;
        string _Command = string.Empty;
        string stringck = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                Command(RandomCommand());
                _isRetFinished = false;

                int iSleep = 0;
                while (_isRetFinished == false)
                {

                    //check Time Out
                    if (iSleep > 60000)
                    {
                        csv.Clear();
                        var newLine = string.Format("{0},{1},{2}", _Command, "Time Out ACK", DateTime.Now);
                        csv.AppendLine(newLine);
                        File.AppendAllText(filePath, csv.ToString());
                        break;

                    }

                    Thread.Sleep(1);
                    iSleep++;

                }
            }
            
        }
        private string Command(string name)
        {

            byte[] bytestosend = null;
            sp.Close();
            sp.PortName = "COM1"; //เปลี่ยนเป็นคัวแปร
            //sp.PortName = _Port;
            sp.BaudRate = 9600;
            sp.Parity = Parity.None;
            sp.DataBits = 8;
            sp.StopBits = StopBits.One;
            //sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            //sp.Open();
            SerialOpen();

            //bytestosend = new byte[] { 0x02, 0x00, 0x35, 0x36, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x30, 0x32, 0x30, 0x30, 0x30, 0x30, 0x1C, 0x34, 0x30, 0x00, 0x12, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x30, 0x30, 0x1C, 0x03, 0x14 };
            if (_Command == "Sale command")
            {
                //02 00 35 36 30 30 30 30 30 30 30 30 30 31 30 32 30 30 30 30 1C 34 30 00 12 30 30 30 30 30 30 30 30 30 31 30 30 1C 03 14
                bytestosend = new byte[] { 0x02, 0x00, 0x35, 0x36, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x30, 0x32, 0x30, 0x30, 0x30, 0x30, 0x1C, 0x34, 0x30, 0x00, 0x12, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x30, 0x30, 0x1C, 0x03, 0x14 };
            }
            else if (_Command == "Check balance") // Error
            {
                //02 00 23 36 30 30 30 30 30 30 30 30 30 31 30 35 38 30 30 30 1C 30 30 00 00 1C 03 1A
                bytestosend = new byte[] { 0x02, 0x00, 0x23, 0x36, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x30, 0x35, 0x38, 0x30, 0x30, 0x30, 0x1C, 0x30, 0x30, 0x00, 0x00, 0x1C, 0x03, 0x1A };
            }
            else if (_Command == "Settlement")
            {
                //02 00 26 36 30 30 30 30 30 30 30 30 30 31 30 35 30 30 30 30 1C 48 4E 00 03 30 30 31 1C 03 23
                bytestosend = new byte[] { 0x02, 0x00, 0x26, 0x06, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x30, 0x35, 0x30, 0x30, 0x30, 0x30, 0x1C, 0x48, 0x4E, 0x00, 0x03, 0x30, 0x30, 0x31, 0x1C, 0x03, 0x23 };
            }
            sp.Write(bytestosend, 0, bytestosend.Length);

            return null;
        }
        public void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(1000);

            int length = sp.BytesToRead;
            byte[] buf = new byte[length];
            sp.Read(buf, 0, length);

        }
        private string RandomCommand()
        {
            var names = new List<string> { "Sale command", "Check balance", "Settlement" };

            int index = rnd.Next(names.Count);
            var name = names[index];
            names.RemoveAt(index);
            _Command = name;
            _Command = "Sale command";

            return name;
        }
        private bool SerialOpen()
        {
            bool ok = false;
            try
            {
                if (!(sp.IsOpen))
                    sp.Open();
                ok = true;
            }
            catch (Exception ex)
            {
                // MessageBox.Show("No Connect To EDC");
                Debug.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message);                
                //this.Close();
            }

            return ok;
        }
    }  
}
