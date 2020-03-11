using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace UseTool
{
    public class UseTool
    {
        Form passwordbox;
        System.Windows.Forms.TextBox passwordtextbox;
        System.Windows.Forms.Button passwordbutton;
        System.Windows.Forms.Label passwordlabel;
        string OwnPassword="foxlink2010";
        public UseTool()
        {
            
        }
        ~UseTool()
        {

        }
        public string Password
        {
            get
            {
                return OwnPassword;
            }
            set
            {
                OwnPassword = value;
            }
        }
        public bool PasswordMessageBox(string title, System.Drawing.Point point)
        {
            passwordbox = new Form();
            passwordtextbox = new System.Windows.Forms.TextBox();
            passwordbutton = new System.Windows.Forms.Button();
            passwordlabel = new System.Windows.Forms.Label();
            System.Windows.Forms.DialogResult dialogresult;
            passwordbox.SetBounds(point.X, point.Y, 120, 120);
            passwordbox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            passwordbox.MaximizeBox = false;
            passwordbox.MinimizeBox = false;
            passwordbox.Text = "Password";

            passwordlabel.Location = new System.Drawing.Point(8, 38);
            passwordlabel.Size = new System.Drawing.Size(120, 12);
            passwordlabel.Text = title;
            passwordbox.Controls.Add(passwordlabel);

            passwordtextbox.Location = new System.Drawing.Point(10, 10);
            passwordtextbox.Size = new System.Drawing.Size(100, 22);
            passwordtextbox.PasswordChar = '*';
            passwordbox.Controls.Add(passwordtextbox);
            passwordbutton.Location = new System.Drawing.Point(24, 58);
            passwordbutton.Size = new System.Drawing.Size(75, 23);
            passwordbutton.Text = "OK";
            passwordbox.Controls.Add(passwordbutton);
            passwordbutton.Click += new System.EventHandler(passwordbutton_Click);
            passwordtextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(passwordtextbox_KeyDown);
            dialogresult=passwordbox.ShowDialog();
            if (dialogresult == System.Windows.Forms.DialogResult.OK)
                return true;
            else
                return false;
        }
        private void passwordbutton_Click(object sender, EventArgs e)
        {
            if (passwordtextbox.Text == OwnPassword)
                passwordbox.DialogResult = System.Windows.Forms.DialogResult.OK;
            else
            {
                passwordtextbox.Text = "";
                passwordlabel.Text = "Password Wrong";
                passwordtextbox.Focus();
            }
        }
        private void passwordtextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                if (passwordtextbox.Text == "foxlink2010")
                {
                    passwordbox.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    passwordtextbox.Text = "";
                    passwordlabel.Text = "Password Wrong";
                    passwordtextbox.Focus();
                }
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.Escape)
                passwordbox.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        public void Delay(float secend)
        {
            double delaytime;
            delaytime = DateTime.Now.TimeOfDay.TotalSeconds + secend;
            do
            {
            } while (delaytime >= DateTime.Now.TimeOfDay.TotalSeconds);
        }
    }
    public class IniFile
    {
        string sPath;

        public IniFile(string IniPath)
        {
            sPath = IniPath;
        }

        public void WriteValue(string Section, string Key, string Value)
        {
            long ret = Win32Lib.Kernel.WritePrivateProfileString(Section, Key, Value, sPath);
        }

        public string ReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);

            Win32Lib.Kernel.GetPrivateProfileString(Section, Key, "", temp, 255, sPath);

            return temp.ToString();

        }
    }
}
