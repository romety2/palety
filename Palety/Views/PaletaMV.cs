using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palety.Views
{
    public partial class PaletaMV : Form
    {

        private PaletaV p;
        private bool dodawanie;

        public PaletaMV()
        {
            InitializeComponent();
        }

        public PaletaMV(PaletaV p)
        {
            this.p = p;
            dodawanie = true;
            InitializeComponent();
        }

        public PaletaMV(PaletaV p, string nazwa, string ilosc)
        {
            this.p = p;
            dodawanie = false;
            InitializeComponent();
            textBox1.Text = nazwa;
            textBox2.Text = ilosc;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                if (dodawanie)
                    p.addData(textBox1.Text, Int32.Parse(textBox2.Text));
                else
                    p.editData(textBox1.Text, Int32.Parse(textBox2.Text));
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PaletaMV_FormClosing(object sender, FormClosingEventArgs e)
        {
            p.deleteSmallWindow();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1.PerformClick();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1.PerformClick();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
    }
}
