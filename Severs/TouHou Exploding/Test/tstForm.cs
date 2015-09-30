using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TouHou_Exploding
{
    public partial class tstForm : Form
    {
        Test t;
        public tstForm()
        {
            InitializeComponent();
        }
        private void commondBtn_Click(object sender, EventArgs e)
        {
            if (commendTxtBox.Text == "") return;
            Log("===Input Commond：『" + commendTxtBox.Text + "』===");
            TransCommend(commendTxtBox.Text);
            commendTxtBox.Text = "";
        }
        private void TransCommend(string commond)//翻译输入的命令
        {

        }
        public void Log(string txt)
        {
            if (logTxtBox.Text != "") logTxtBox.Text += "\r\n" + txt;
            else logTxtBox.Text = txt;
        
        }
        public void Log(int txt)
        {
            Log(txt.ToString());
        }
        public void Log(bool txt)
        {
            Log(txt.ToString());
        }
        private void logTxtBox_TextChanged(object sender, EventArgs e)
        {

        }
        public void Log(特殊 t)
        {
            switch (t)
            {
                case 特殊.单横:
                    Log("----------------------------------------------------");
                    break;
                case 特殊.双横:
                    Log("=============================");
                    break;
                case 特殊.空行:
                    Log("");
                    break;
                case 特殊.刷屏:
                    for (int a = 0; a < 20; a++) Log("");
                    break;
                case 特殊.清屏:
                    logTxtBox.Text = "";
                    break;
            }
        }
        public enum 特殊
        {
            单横, 双横, 空行, 刷屏, 清屏,
        }

        private void tstForm_Load(object sender, EventArgs e)
        {
            t = new Test(this);
            t.测试();
        }
        
    }
}
