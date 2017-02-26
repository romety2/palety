using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Palety.Controllers;
using Palety.Models;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Palety.Views
{
    public partial class PaletaV : Form
    {
        private App app;
        private DataC dc;
        private Data data;
        private PaletaMV pmv;
        public bool zapisano;

        public PaletaV()
        {
            dc = new DataC();
            InitializeComponent();
            zapisano = true;
        }

        public PaletaV(DataC dc)
        {
            this.dc = dc;
            InitializeComponent();
            zapisano = true;
        }

        public PaletaV(DataC dc, App app)
        {
            this.dc = dc;
            this.app = app;
            InitializeComponent();
            zapisano = true;
        }

        private BindingList<Paleta> getPalety()
        {
            data = dc.getData();
            return data.Palety;
        }

        private void getWidok()
        {
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = 150;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].HeaderText = "Ilość";
            dataGridView1.Columns[2].Width = 150;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        public void addData(string text, int ilosc)
        {
            int id;
            if (dataGridView1.Rows.Count != 0 && String.Compare(text, dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value.ToString(), true) < 0)
                id = dataGridView1.CurrentCellAddress.Y + 1;
            else
                id = dataGridView1.CurrentCellAddress.Y;
            label4.Text = "Dodano paletę " + text + "!";
            zapisano = false;
            BindingSource palety = new BindingSource();
            palety.DataSource = dc.AddPaleta(text, ilosc).OrderBy(o => o.Nazwa);
            dataGridView1.DataSource = palety;
            if (dataGridView1.Rows.Count != 1 && textBox1.Text == "")
                dataGridView1.CurrentCell = dataGridView1[1, id];
            else
                textBox1.Text = "";
        }

        public void editData(string text, int ilosc)
        {
            label4.Text = "Zmieniono nazwę palety z " + dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value + " na " + text + "!";
            label3.Text = text;
            zapisano = false;
            //dc.EditPaleta((ulong)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value, textBox1.Text);
            dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value = text;
            dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[2].Value = ilosc.ToString();
        }

        public void deleteSmallWindow()
        {
            pmv = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pmv == null)
            {
                pmv = new PaletaMV(this);
                pmv.Text = "Dodaj paletę";
                pmv.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pmv == null && dataGridView1.Rows.Count != 0)
            {
                pmv = new PaletaMV(this, dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value.ToString(), dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[2].Value.ToString());
                pmv.Text = "Zmień dane palety";
                pmv.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pmv == null && dataGridView1.Rows.Count != 0)
            {
                string mojaPaleta = dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value.ToString();
                DialogResult dr = MessageBox.Show("Na pewno usunąć " + mojaPaleta + "?", "Pytanie", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    label4.Text = "Usunięto paletę " + mojaPaleta + "!";
                    zapisano = false;
                    dc.DeletePaleta((ulong)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value);
                    dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCellAddress.Y);
                    app.refreshData();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label4.Text = "Zapisano zmiany!";
            zapisano = true;
            dc.SaveData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PaletaV_Load(object sender, EventArgs e)
        {
            BindingSource palety = new BindingSource();
            palety.DataSource = getPalety().OrderBy(o => o.Nazwa).ToList();
            dataGridView1.DataSource = palety;
            getWidok();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count != 0)
            {
                label3.Text = dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value.ToString();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = getPalety().OrderBy(o => o.Nazwa).Where(o => o.Nazwa.ToLower().Contains(textBox1.Text.ToLower()) == true).ToList();
            dataGridView1.DataSource = bs;
        }

        private void PaletaV_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!zapisano)
            {
                DialogResult dr = MessageBox.Show("Zapisać zmiany?", "Pytanie", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    zapisano = true;
                    dc.SaveData();
                }
                else if (dr == DialogResult.Cancel)
                    e.Cancel = true;
                if (dr != DialogResult.Cancel)
                {
                    if (pmv != null)
                        pmv.Close();
                }
            }
            else if (pmv != null)
                pmv.Close();
        }
    }
}
