using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Pallets.Controllers;
using Pallets.Models;

namespace Pallets.Views
{
    public partial class PaletteV : Form
    {
        private App app;
        private DataC dc;
        private Data data;
        private PaletteMV pmv;
        public bool saved;

        public PaletteV()
        {
            dc = new DataC();
            InitializeComponent();
            saved = true;
        }

        public PaletteV(Data data)
        {
            dc = new DataC(data.Events);
            InitializeComponent();
            saved = true;
        }

        public PaletteV(App app)
        {
            dc = new DataC();
            this.app = app;
            InitializeComponent();
            saved = true;
        }

        public PaletteV(Data data, App app)
        {
            dc = new DataC(data.Events);
            this.app = app;
            InitializeComponent();
            saved = true;
        }

        private BindingList<Palette> getPallets()
        {
            data = dc.getData();
            return data.Pallets;
        }

        private void getView()
        {
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Paleta";
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
            saved = false;
            BindingSource palety = new BindingSource();
            palety.DataSource = dc.addPalette(text, ilosc).OrderBy(o => o.Name);
            dataGridView1.DataSource = palety;
            if (dataGridView1.Rows.Count != 1 && textBox1.Text == "")
                dataGridView1.CurrentCell = dataGridView1[1, id];
            else
                textBox1.Text = "";
        }

        public void editData(string text, int quantity)
        {
            label4.Text = "Zmieniono nazwę palety z " + dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value + " na " + text + "!";
            label3.Text = text;
            saved = false;
            //dc.EditPaleta((ulong)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value, textBox1.Text);
            dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value = text;
            dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[2].Value = quantity.ToString();
        }

        public void deleteSmallWindow()
        {
            pmv = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pmv == null)
            {
                pmv = new PaletteMV(this);
                pmv.Text = "Dodaj paletę";
                pmv.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pmv == null && dataGridView1.Rows.Count != 0)
            {
                pmv = new PaletteMV(this, dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value.ToString(), dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[2].Value.ToString());
                pmv.Text = "Zmień dane palety";
                pmv.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pmv == null && dataGridView1.Rows.Count != 0)
            {
                string myPalette = dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value.ToString();
                DialogResult dr = MessageBox.Show("Na pewno usunąć " + myPalette + "?", "Pytanie", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    label4.Text = "Usunięto paletę " + myPalette + "!";
                    saved = false;
                    dc.deletePalette((ulong)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value);
                    dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCellAddress.Y);
                    //app.refreshData();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label4.Text = "Zapisano zmiany!";
            saved = true;
            dc.saveData(false, true, false);
            app.refreshDC();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PaletteV_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!saved)
            {
                DialogResult dr = MessageBox.Show("Zapisać zmiany?", "Pytanie", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    saved = true;
                    dc.saveData(false, true, false);
                    app.refreshDC();
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

        private void PaletteV_Load(object sender, EventArgs e)
        {
            BindingSource pallets = new BindingSource();
            pallets.DataSource = getPallets().OrderBy(o => o.Name).ToList();
            dataGridView1.DataSource = pallets;
            getView();
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
            bs.DataSource = getPallets().OrderBy(o => o.Name).Where(o => o.Name.ToLower().Contains(textBox1.Text.ToLower()) == true).ToList();
            dataGridView1.DataSource = bs;
        }
    }
}
