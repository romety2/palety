using Pallets.Controllers;
using Pallets.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Pallets.Views
{
    public partial class EventMV : Form
    {
        private App app;
        private DataC dc;
        private Data data;
        private List<Company> companies;
        private List<Palette> pallets;
        private BindingList<Event.MyPalette> mpallets;
        private bool addition;
        private string eid;

        public EventMV()
        {
            dc = new DataC();
            mpallets = new BindingList<Event.MyPalette>();
            InitializeComponent();
        }

        public EventMV(App a, DataC dc)
        {
            app = a;
            this.dc = dc;
            mpallets = new BindingList<Event.MyPalette>();
            addition = true;
            InitializeComponent();
        }

        public EventMV(App a, string id, DataC dc)
        {
            app = a;
            this.dc = dc;
            mpallets = new BindingList<Event.MyPalette>();
            addition = false;
            InitializeComponent();
            eid = id;
        }

        private void EventMV_Load(object sender, EventArgs e)
        {
            data = dc.getData();
            companies = data.Companies.OrderBy(o => o.Name).ToList();
            comboBox1.DataSource = companies;
            comboBox1.DisplayMember = "Name";
            if (addition)
            {
                pallets = data.Pallets.OrderBy(o => o.Name).ToList();
                foreach (Palette p in pallets)
                    mpallets.Add(new Event.MyPalette(p, 0, 0));
                comboBox2.DataSource = pallets;
                comboBox2.DisplayMember = "Name";
                if (comboBox1.Items.Count != 0)
                    comboBox1.SelectedItem = comboBox1.Items[0];
                if (comboBox2.Items.Count != 0)
                {
                    comboBox2.SelectedItem = comboBox2.Items[0];
                    textBox3.Text = mpallets[0].Plus.ToString();
                    textBox4.Text = mpallets[0].Minus.ToString();
                }
            }
            else
            {
                Event w = data.Events.First(o => o.Id.ToString() == eid);
                dateTimePicker1.Text = w.Date;
                if (comboBox1.Items.Count != 0)
                    comboBox1.SelectedIndex = comboBox1.FindString(w.Company.Name);
                pallets = data.Pallets.OrderBy(o => o.Name).ToList();
                mpallets = w.MPalette;
                comboBox2.DataSource = pallets;
                comboBox2.DisplayMember = "Name";
                if (comboBox2.Items.Count != 0)
                {
                    comboBox2.SelectedItem = comboBox2.Items[0];
                    textBox3.Text = mpallets[0].Plus.ToString();
                    textBox4.Text = mpallets[0].Minus.ToString();
                }
                textBox2.Text = w.Comment;
            }
        }

        private void EventMV_FormClosing(object sender, FormClosingEventArgs e)
        {
            app.deleteSmallWindow();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            companies = data.Companies.OrderBy(o => o.Name).Where(o => o.Name.ToLower().Contains(textBox1.Text.ToLower()) == true).ToList();
            bs.DataSource = companies;
            comboBox1.DataSource = bs;
            if (comboBox1.Items.Count == 0)
                comboBox1.Text = "";
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            pallets = data.Pallets.OrderBy(o => o.Name).Where(o => o.Name.ToLower().Contains(textBox5.Text.ToLower())).ToList();
            bs.DataSource = pallets;
            comboBox2.DataSource = bs;
            if (comboBox2.Items.Count == 0)
            {
                comboBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
            }
            else
            {
                textBox3.Text = mpallets.First(o => o.Palette.Id == pallets[0].Id).Plus.ToString();
                textBox4.Text = mpallets.First(o => o.Palette.Id == pallets[0].Id).Minus.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && comboBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {
                if (addition)
                    app.addData(dateTimePicker1.Value.ToShortDateString(), mpallets, companies[comboBox1.SelectedIndex], textBox2.Text);
                else
                    app.editData(dateTimePicker1.Value.ToShortDateString(), mpallets, companies[comboBox1.SelectedIndex], textBox2.Text);
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
            int i = mpallets.IndexOf(mpallets.First(o => o.Palette.Id == pallets[comboBox2.SelectedIndex].Id));
            if (textBox3.Text != "")
                mpallets[i] = new Event.MyPalette(mpallets[i].Palette, Int32.Parse(textBox3.Text), mpallets[i].Minus);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            int i = mpallets.IndexOf(mpallets.First(o => o.Palette.Id == pallets[comboBox2.SelectedIndex].Id));
            if(textBox4.Text != "")
                mpallets[i] = new Event.MyPalette(mpallets[i].Palette, mpallets[i].Plus, Int32.Parse(textBox4.Text));
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Event.MyPalette mp = mpallets.First(o => o.Palette.Id == pallets[comboBox2.SelectedIndex].Id);
            textBox3.Text = mp.Plus.ToString();
            textBox4.Text = mp.Minus.ToString();
        }
    }
}
