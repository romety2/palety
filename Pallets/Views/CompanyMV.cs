using System;
using System.Windows.Forms;

namespace Pallets.Views
{
    public partial class CompanyMV : Form
    {
        private CompanyV f;
        private bool addition;

        public CompanyMV()
        {
            InitializeComponent();
        }

        public CompanyMV(CompanyV f)
        {
            this.f = f;
            f.Enabled = false;
            addition = true;
            InitializeComponent();
        }

        public CompanyMV(CompanyV f, string name)
        {
            this.f = f;
            f.Enabled = false;
            addition = false;
            InitializeComponent();
            textBox1.Text = name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if(addition)
                    f.addData(textBox1.Text);
                else
                    f.editData(textBox1.Text);
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FirmaMV_FormClosing(object sender, FormClosingEventArgs e)
        {
            f.Enabled = true;
            f.deleteSmallWindow();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1.PerformClick();
        }
    }
}
