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
        public tstForm()
        {
            InitializeComponent();
        }
        private void commondBtn_Click(object sender, EventArgs e)
        {
            if (commondTxtBox.Text == "") return;
            Log("===Input Commond：『" + commondTxtBox.Text + "』===");
            commondTxtBox.Text = "";
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

        private void tstForm_Load(object sender, EventArgs e)
        {
            new Core();
        }
        
    }
}
