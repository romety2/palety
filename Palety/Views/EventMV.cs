using Pallets.Controllers;
using Pallets.Models;
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
    public partial class EventMV : Form
    {
        private App a;
        private DataC dc;
        private Data data;
        private List<Company> firmy;
        private List<Palette> palety;
        private BindingList<Event.MojePalety> mpalety;
        private bool dodawanie;
        private string eid;

        public EventMV()
        {
            dc = new DataC();
            mpalety = new BindingList<Event.MojePalety>();
            InitializeComponent();
        }

        public EventMV(App a, DataC dc)
        {
            this.a = a;
            this.dc = dc;
            mpalety = new BindingList<Event.MojePalety>();
            dodawanie = true;
            InitializeComponent();
        }

        public EventMV(App a, string id, DataC dc)
        {
            this.a = a;
            this.dc = dc;
            mpalety = new BindingList<Event.MojePalety>();
            dodawanie = false;
            InitializeComponent();
            eid = id;
        }

        private void WydarzenieMV_Load(object sender, EventArgs e)
        {
            data = dc.getData();
            firmy = data.Firmy.OrderBy(o => o.Nazwa).ToList();
            comboBox1.DataSource = firmy;
            comboBox1.DisplayMember = "Nazwa";
            if (dodawanie)
            {
                palety = data.Palety.OrderBy(o => o.Nazwa).ToList();
                foreach (Palette p in palety)
                    mpalety.Add(new Event.MojePalety(p, 0, 0));
                comboBox2.DataSource = palety;
                comboBox2.DisplayMember = "Nazwa";
                if (comboBox1.Items.Count != 0)
                    comboBox1.SelectedItem = comboBox1.Items[0];
                if (comboBox2.Items.Count != 0)
                {
                    comboBox2.SelectedItem = comboBox2.Items[0];
                    textBox3.Text = mpalety[0].Plus.ToString();
                    textBox4.Text = mpalety[0].Minus.ToString();
                }
            }
            else
            {
                Event w = data.Wydarzenia.First(o => o.Id.ToString() == eid);
                dateTimePicker1.Text = w.Data;
                if (comboBox1.Items.Count != 0)
                    comboBox1.SelectedIndex = comboBox1.FindString(w.Firma.Nazwa);
                palety = data.Palety.OrderBy(o => o.Nazwa).ToList();
                mpalety = w.MPalety;
                comboBox2.DataSource = palety;
                comboBox2.DisplayMember = "Nazwa";
                if (comboBox2.Items.Count != 0)
                {
                    comboBox2.SelectedItem = comboBox2.Items[0];
                    textBox3.Text = mpalety[0].Plus.ToString();
                    textBox4.Text = mpalety[0].Minus.ToString();
                }
                textBox2.Text = w.Uwagi;
            }
        }

        private void WydarzenieMV_FormClosing(object sender, FormClosingEventArgs e)
        {
            a.deleteSmallWindow();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            firmy = data.Firmy.OrderBy(o => o.Nazwa).Where(o => o.Nazwa.ToLower().Contains(textBox1.Text.ToLower()) == true).ToList();
            bs.DataSource = firmy;
            comboBox1.DataSource = bs;
            if (comboBox1.Items.Count == 0)
                comboBox1.Text = "";
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            palety = data.Palety.OrderBy(o => o.Nazwa).Where(o => o.Nazwa.ToLower().Contains(textBox5.Text.ToLower()) == true).ToList();
            bs.DataSource = palety;
            comboBox2.DataSource = bs;
            if (comboBox2.Items.Count == 0)
            {
                comboBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
            }
            else
            {
                textBox3.Text = mpalety.First(o => o.Paleta.Id == palety[0].Id).Plus.ToString();
                textBox4.Text = mpalety.First(o => o.Paleta.Id == palety[0].Id).Minus.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && comboBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {
                if (dodawanie)
                    a.addData(dateTimePicker1.Value.ToShortDateString(), mpalety, firmy[comboBox1.SelectedIndex], textBox2.Text);
                else
                    a.editData(dateTimePicker1.Value.ToShortDateString(), mpalety, firmy[comboBox1.SelectedIndex], textBox2.Text);
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            int i = mpalety.IndexOf(mpalety.First(o => o.Paleta.Id == palety[comboBox2.SelectedIndex].Id));
            if (textBox3.Text != "")
                mpalety[i] = new Event.MojePalety(mpalety[i].Paleta, Int32.Parse(textBox3.Text), mpalety[i].Minus);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            int i = mpalety.IndexOf(mpalety.First(o => o.Paleta.Id == palety[comboBox2.SelectedIndex].Id));
            if(textBox4.Text != "")
                mpalety[i] = new Event.MojePalety(mpalety[i].Paleta, mpalety[i].Plus, Int32.Parse(textBox4.Text));
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Event.MojePalety mp = mpalety.First(o => o.Paleta.Id == palety[comboBox2.SelectedIndex].Id);
            textBox3.Text = mp.Plus.ToString();
            textBox4.Text = mp.Minus.ToString();
        }
    }
}
