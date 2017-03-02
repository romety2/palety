using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pallets.Views
{
    public partial class CompanyMV : Form
    {
        private CompanyV f;
        private bool dodawanie;

        public CompanyMV()
        {
            InitializeComponent();
        }

        public CompanyMV(CompanyV f)
        {
            this.f = f;
            dodawanie = true;
            InitializeComponent();
        }

        public CompanyMV(CompanyV f, string nazwa)
        {
            this.f = f;
            dodawanie = false;
            InitializeComponent();
            textBox1.Text = nazwa;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if(dodawanie)
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
            f.deleteSmallWindow();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1.PerformClick();
        }
    }
}
