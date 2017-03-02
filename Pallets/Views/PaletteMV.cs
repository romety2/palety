using System;
using System.Windows.Forms;

namespace Pallets.Views
{
    public partial class PaletteMV : Form
    {

        private PaletteV p;
        private bool addition;

        public PaletteMV()
        {
            InitializeComponent();
        }

        public PaletteMV(PaletteV p)
        {
            this.p = p;
            addition = true;
            InitializeComponent();
        }

        public PaletteMV(PaletteV p, string name, string quantity)
        {
            this.p = p;
            addition = false;
            InitializeComponent();
            textBox1.Text = name;
            textBox2.Text = quantity;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                if (addition)
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

        private void PaletteMV_FormClosing(object sender, FormClosingEventArgs e)
        {
            p.deleteSmallWindow();
        }
    }
}
