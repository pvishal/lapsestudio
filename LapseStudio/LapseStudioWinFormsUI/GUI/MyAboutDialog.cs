using System;
using System.Windows.Forms;
using Timelapse_UI;

namespace LapseStudioWinFormsUI
{
    public partial class MyAboutDialog : Form
    {
        public MyAboutDialog()
        {
            InitializeComponent();
            richTextBox1.Text = GeneralValues.AbouText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
