using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TactPlay
{
    public partial class AccuracyTest : Form
    {
        public AccuracyTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (GetResult() != -1)
            {
                CloseOK();
            }
        }

        private void CloseOK()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public int GetResult()
        {
            String text = textBox1.Text;
            if (text == null || text.Equals(""))
            {
                return -1;
            }
            try
            {
                return int.Parse(text);
            }
            catch (FormatException e)
            {
                return -1;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (GetResult() != -1)
                {
                    CloseOK();
                }
            }
        }
    }
}
