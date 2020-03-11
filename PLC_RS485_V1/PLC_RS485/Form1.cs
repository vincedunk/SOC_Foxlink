using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using auto_charge_1;

namespace PLC_RS485
{
    public partial class Form1 : Form
    {
        
        byte[] bufCmd = new byte[3000];
        byte[] received_buf = new byte[3000];
        bool getdata = true;
        string data_receive = "";
        string sCfgFile;
        CONFIG _Config;
        public Form1()
        {
            InitializeComponent();
            //serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = (string)comboBox1.SelectedItem;
            serialPort1.Open();
            button2.Enabled = true;
            button4.Enabled = true;
            startProgress();
        }

        private void button2_Click(object sender, EventArgs e)
        {
                button2.Enabled = false;


                serialPort1.Write("SOUR:VOLT"+" "+ charge_voltage.Text+"\n");
                Thread.Sleep(200);

                serialPort1.Write("SOUR:CURR" + " " + charge_current.Text + "\n");
                Thread.Sleep(200);

                serialPort1.Write("SOUR:VOLT?\n");
                Thread.Sleep(200);


                int bytes = serialPort1.BytesToRead;
                string getdata = serialPort1.ReadLine();
                if (bytes > 0)
                {
                    MessageBox.Show(double.Parse(getdata).ToString("0.00"));
                }

                serialPort1.Write("SOUR:CURR?\n");
                Thread.Sleep(200);


                int bytes1 = serialPort1.BytesToRead;
                string getdata1 = serialPort1.ReadLine();
                if (bytes > 0)
                {
                    MessageBox.Show(double.Parse(getdata1).ToString("0.00"));
                }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
        }


/*        void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            String data = sp.ReadExisting();
        }
*/

        private void Form1_Load(object sender, EventArgs e)
        {
            _Config = new CONFIG();
            string[] serialPorts = SerialPort.GetPortNames();
            if (serialPorts.Length > 0)
            {
                foreach (string serialPort in serialPorts)
                {
                    comboBox1.Items.Add(serialPort);
                    if (comboBox1.Items.Count > 0)
                    {
                        comboBox1.SelectedIndex = 0;
                        button1.Enabled = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("找不到任何通訊埠!");
                button1.Enabled = false;
            }
            button2.Enabled = false;
            button4.Enabled = false;

            Init();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Init()
        {
            string sTemp;

            sTemp = Application.ExecutablePath;
            sCfgFile = Path.GetDirectoryName(sTemp) + "\\" + "Default.cfg";


            if (false == File.Exists(sCfgFile))
                _Config.SaveConfigFile(sCfgFile);
            else
                _Config.LoadConfigFile(sCfgFile);
            UpdateConfigUI();

        }

        private void UpdateConfigUI()
        {
            #region Charge Setting
            charge_voltage.Text = _Config.scharge_voltage;
            charge_current.Text = _Config.scharge_current;
            judge_time.Text = _Config.sjudge_time;
            judge_current.Text = _Config.sjudge_current;
            #endregion

            #region Threshold Setting
            end_voltage.Text = _Config.send_voltage;
            end_current.Text = _Config.send_current;
            #endregion

            #region Protect Setting
            max_charge_time.Text = _Config.smax_charge_time;
            #endregion

            #region Stall Setting
            stall_time.Text = _Config.sstall_time;
            #endregion
        }

        private void SetConfigUI()
        {
            #region Charge Setting
            _Config.scharge_voltage = charge_voltage.Text;
            _Config.scharge_current = charge_current.Text;
            _Config.sjudge_time = judge_time.Text;
            _Config.sjudge_current = judge_current.Text;
            #endregion

            #region Threshold Setting
            _Config.send_voltage = end_voltage.Text;
            _Config.send_current = end_current.Text;
            #endregion

            #region Protect Setting
            _Config.smax_charge_time = max_charge_time.Text;
            #endregion

            #region Stall Setting
            _Config.sstall_time = stall_time.Text;
            #endregion
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string sTemp;

            sTemp = Application.ExecutablePath;
            sCfgFile = Path.GetDirectoryName(sTemp) + "\\" + "Setting.cfg";


            if (true == File.Exists(sCfgFile))
                _Config.LoadConfigFile(sCfgFile);
            else
                MessageBox.Show("設定檔案遺失，請重新設定!");
            UpdateConfigUI();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            string sTemp;

            sTemp = Application.ExecutablePath;
            sCfgFile = Path.GetDirectoryName(sTemp) + "\\" + "Setting.cfg";

            SetConfigUI();
            _Config.SaveConfigFile(sCfgFile);
            MessageBox.Show("設定檔案存檔完成!");
        }

        private void startProgress()
        {
            pBar1.Visible = true;// 顯示進度條控制元件.
            pBar1.Minimum = 1;// 設定進度條最小值.
            pBar1.Maximum = 15;// 設定進度條最大值.
            pBar1.Value = 1;// 設定進度條初始值
            pBar1.Step = 1;// 設定每次增加的步長
            //建立Graphics物件
            Graphics g = this.pBar1.CreateGraphics();
            for (int x = 1; x <= 9; x++)
            {
                //執行PerformStep()函式
                pBar1.PerformStep();
                string str = Math.Round((100 * x / 15.0), 2).ToString("#0.00 ") + "%";
                Font font = new Font("Times New Roman", (float)10, FontStyle.Regular);
                PointF pt = new PointF(this.pBar1.Width / 2 - 17, this.pBar1.Height / 2 - 7);
                g.DrawString(str, font, Brushes.Blue, pt);
                //每次迴圈讓程式休眠300毫秒
                System.Threading.Thread.Sleep(300);
            }
            // pBar1.Visible = false;
            //MessageBox.Show("success!");
        }
    }
}